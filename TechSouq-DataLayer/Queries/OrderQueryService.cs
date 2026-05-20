using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Reflection.Metadata.Ecma335;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.Queries;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Enums;
using TechSouq.Domain.Interfaces;
using TechSouq.Infrastructure.Data;
using Order = TechSouq.Domain.Entities.Order;

namespace TechSouq.Infrastructure.Queries
{
    public class OrderQueryService : IOrderQueryService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICouponRepository _couponRepository;
        private readonly ICartItemsQueryService _cartItemsQueryService;
        private readonly IMapper _mapper;
        private readonly IOrderItemRepository _OrderItemRepository;
        private readonly IOrderRepository _OrderRepository;
        private readonly ICartRepository _CartRepository;
        private readonly IConnectionMultiplexer _redis;

        private bool isAnyProductOutofStock = false;

        public OrderQueryService(AppDbContext appDbContext,ICouponRepository couponRepository,
            ICartItemsQueryService cartItemsQueryService, IMapper mapper, IOrderItemRepository orderItemRepository, 
            IOrderRepository orderRepository, ICartRepository CartRepository
            , IConnectionMultiplexer redis)
        {
            _appDbContext = appDbContext;
            _couponRepository = couponRepository;
            _cartItemsQueryService = cartItemsQueryService;
            _mapper = mapper;
            _OrderItemRepository = orderItemRepository;
            _OrderRepository = orderRepository;
            _CartRepository = CartRepository;
            _redis = redis;
        }

        public async Task<decimal> ConfirmOrderAsync(ConfirmOrderDto confirmOrderDto,int cartId, int userId)
        {
            await using var transAction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                var cartitemsAndProductDetils = await _cartItemsQueryService.GetAllCartItemsWithProductDetailsAsync(userId, true);

                if (!cartitemsAndProductDetils.Any())
                {
                    await transAction.RollbackAsync();
                    return 0;
                }

                Order order = new Order
                {
                    ShippingFullName = confirmOrderDto.ShippingFullName,
                    Email = confirmOrderDto.Email,
                    Phone = confirmOrderDto.Phone,
                    ShippingCity = confirmOrderDto.ShippingCity,
                    ShippingStreet = confirmOrderDto.ShippingStreet,
                    Building = confirmOrderDto.Building,
                    PaymentWayId = confirmOrderDto.PaymentWayId,
                    Country = confirmOrderDto.Country,

                    UserId = userId,

                    SubTotal = CartTotal(cartitemsAndProductDetils)
                };

                if (!string.IsNullOrWhiteSpace(confirmOrderDto.Code))
                {
                    var coupon = await _couponRepository.GetOnlyActiveCouponByCode(confirmOrderDto.Code);

                    if (coupon != null)
                    {
                        if (coupon.UsageLimit <= 0)
                        {
                            await transAction.RollbackAsync();
                            return 0;
                        }

                        order.CouponId = coupon.Id;

                        order.DiscountAmount = CalculateDiscountAmount( coupon,cartitemsAndProductDetils);

                        coupon.UsageLimit--;

                        _appDbContext.Coupons.Update(coupon);
                    }
                }

                order.DeliveryCost =await CalculateDeliveryCost(order.SubTotal,confirmOrderDto.ShippingCity,cartitemsAndProductDetils);

                if (order.DeliveryCost == -1)
                {
                    await transAction.RollbackAsync();
                    return 0;
                }

                 

                order.TotalAmount =(order.SubTotal + order.DeliveryCost) - (order.DiscountAmount);

                var OrderId = await _OrderRepository.AddOrder(order);

                if(OrderId == 0)
                {

                    await transAction.RollbackAsync();
                    return 0;
                }


                var productIds = cartitemsAndProductDetils.Select(x => x.ProductId).ToList();

                var productsToUpdate = await _appDbContext.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();

                List<OrderItem> OrderItems = new List<OrderItem>();

                foreach (var item in cartitemsAndProductDetils)
                {
                    var currentProduct = productsToUpdate.FirstOrDefault(p => p.Id == item.ProductId);

                    if (currentProduct == null || currentProduct.Stock < item.Quantity)
                    {
                        await transAction.RollbackAsync();
                        return 0; 
                    }

                    currentProduct.Stock -= item.Quantity;
                    _appDbContext.Products.Update(currentProduct);

                    if (currentProduct.Stock == 0)
                    {
                        isAnyProductOutofStock = true; 
                    }

                    OrderItem itemItem = new OrderItem();
                    itemItem.Quantity = item.Quantity;
                    itemItem.UnitPrice = item.PriceAfterDiscount ?? item.ProductPrice;
                    itemItem.OrderId = OrderId;
                    itemItem.ProductId = item.ProductId;

                    OrderItems.Add(itemItem);
                }

                if (!OrderItems.Any())
                {
                    await transAction.RollbackAsync();
                    return 0;
                }

                if (!await _OrderItemRepository.AddOrderItems(OrderItems))
                {
                    await transAction.RollbackAsync();
                    return 0;
                }
                var IsChanged = await _CartRepository.ChangeCartStatus(cartitemsAndProductDetils[0].CartId, CartStatus.CheckedOut);

                if (!IsChanged)
                {
                    await transAction.RollbackAsync();
                    return 0;
                }

                await transAction.CommitAsync();

                if (isAnyProductOutofStock)
                {
                    ClearProductPagesCache.ClearProductPagesCacheAsync(_redis);
                }

                    return order.TotalAmount;
            }
            catch
            {
                await transAction.RollbackAsync();
                throw;
            }
        }

        private decimal CartTotal(
            List<CartItemsWithProductDetailsDto> cartitemsAndProductDetils)
        {
            return cartitemsAndProductDetils.Sum(x => x.Subtotal);
        }

        private decimal TotalWithoutDiscountedProducts(
            List<CartItemsWithProductDetailsDto> cartitemsAndProductDetils)
        {
            return cartitemsAndProductDetils
                .Where(x => x.PriceAfterDiscount == null)
                .Sum(x => x.Subtotal);
        }

        private decimal CalculateDiscountAmount(
            Coupon coupon,
            List<CartItemsWithProductDetailsDto> cartitemsAndProductDetils)
        {
            decimal total = CartTotal(cartitemsAndProductDetils);

            if (!coupon.IsApplicableOnDiscountedItems)
            {
                total = TotalWithoutDiscountedProducts( cartitemsAndProductDetils);
            }

            if (coupon.DiscountType == DiscountType.Percentage)
            {
                return (total * coupon.DiscountValue) / 100;
            }

            return Math.Min(total, coupon.DiscountValue);
        }

        private async Task<decimal> CalculateDeliveryCost(decimal subTotal,string shippingCity,List<CartItemsWithProductDetailsDto> cartitemsAndProductDetils)
        {
            if (!cartitemsAndProductDetils.Any())
            {
                return -1;
            }

            var shippingCost =
                await _appDbContext.DeliveryZones
                .AsNoTracking()
                .Where(x => x.Name == shippingCity)
                .Select(x => (decimal?)x.ShippingCost)
                .FirstOrDefaultAsync();

            if (shippingCost == null)
            {
                return -1;
            }

            var freeShippingThreshold =
                await _appDbContext.SystemSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.SettingKey == "FreeShippingThreshold");

            if (freeShippingThreshold != null)
            {
                if (subTotal >=
                    Convert.ToDecimal(freeShippingThreshold.SettingValue))
                {
                    return 0;
                }
            }

            return shippingCost.Value;
        }

        public async Task<List<OrderSummarieDto>> GetOrderSummaryAsync(int cartId, int userId)
        {
            var Orders = await _appDbContext.Orders.AsNoTracking().Where(x => x.UserId == userId).OrderByDescending(x => x.Id).Select(x => new OrderSummarieDto
            {
                Id = x.Id,
                Date = x.OrderDate,
                Status = x.Status,
                DeliveryAddress = $"{x.ShippingCity} ,{x.ShippingStreet},{x.Country}",
                PaymentWayId = x.PaymentWayId,
                Subtotal = x.SubTotal,
                DeliveryCost = x.DeliveryCost,
                DiscountAmount = x.DiscountAmount,
                TotalAmount = x.TotalAmount,
                Items = x.OrderItems.Select(o => new ItemDto
                {
                    Id = o.Product.Id,
                    Name = o.Product.Name,
                    Image = o.Product.ProductImages.Select(img => img.ImageUrl).FirstOrDefault(),
                    Price = o.Product.Price,
                    Quantity = o.Quantity,
                }).ToList()

            }).ToListAsync();

            return Orders;
        }
        
    }
}