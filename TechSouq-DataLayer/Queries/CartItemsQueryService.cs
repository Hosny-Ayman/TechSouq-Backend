using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Queries;
using TechSouq.Infrastructure.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechSouq.Infrastructure.Queries
{
    public class CartItemsQueryService : ICartItemsQueryService
    {

        private readonly AppDbContext _appDbContext;

        public CartItemsQueryService(AppDbContext AppDbContext)
        {
            _appDbContext = AppDbContext;
        }

        public async Task<List<CartItemsWithProductDetailsDto>> GetAllCartItemsWithProductDetailsAsync(int UserId, bool trackingChanges = true)
        {
            var query = _appDbContext.CartItems.AsQueryable();

            if (!trackingChanges)
            {
                query = query.AsNoTracking();
            }

           var data = await query.Where(item=> item.Cart.UserId == UserId && item.Cart.Status == Domain.Enums.CartStatus.Active)
                      .Select(item=> new CartItemsWithProductDetailsDto
                      {
                          CartId = item.Cart.Id,
                          CartItemId = item.Id,
                          ProductId = item.ProductId,
                          ProductName = item.Product.Name,
                          ProductImage = item.Product.ProductImages.Select(img=>img.ImageUrl).FirstOrDefault(), 
                          ProductPrice = item.Product.Price,
                          Quantity = item.Quantity,
                          PriceAfterDiscount = item.Product.PriceAfterDiscount,
                          IsFreeShipping = item.Product.IsFreeShipping,
                          Subtotal = item.Quantity *(item.Product.PriceAfterDiscount ?? item.Product.Price),
                          Stock = item.Product.Stock,
                      }).ToListAsync();

            return data;
        }
    }
}
