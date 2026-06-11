using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.interfaces;
using TechSouq.Application.Queries;
using TechSouq.Infrastructure.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TechSouq.Infrastructure.Queries
{
    public class DashboardQuery : IDashboardQuery
    {

        private AppDbContext _appDbContext;

        public DashboardQuery(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<BestSellingProductsDto>> BestSellingProducts()
        {
            var product = await _appDbContext.OrderItems.Where(x => x.Order.Status == Domain.Enums.OrderStatus.Delivered)
                .GroupBy(x => new
                {
                    x.ProductId,
                    x.Product.Name,
                    CategoryName = x.Product.Categorie.Name,

                })
                .Select(g => new BestSellingProductsDto
                {
                    ProductName = g.Key.Name,
                    CategoryName = g.Key.CategoryName,
                    TotalSell = g.Sum(x => x.Quantity),
                   

                })
                .Take(6)
                .OrderByDescending(x => x.TotalSell)
                .ToListAsync();

            var MaxSell = product.Max(x => x.TotalSell);

            foreach(var pro in product)
            {
                pro.PercentageOfSell = MaxSell == 0 ? 0 : (double)pro.TotalSell / MaxSell * 100;
            }

            return product;

        }

        public async Task<PagedResponse<RecentOrderDto>> RecentSales(RecentSaleceQueryParams queryParams)
        {
            var query = _appDbContext.OrderItems.AsNoTracking().AsQueryable();

            var totalrecords = await query.CountAsync();

           
           

            if (!string.IsNullOrEmpty(queryParams.SortField))
            {
                switch (queryParams.SortField.ToLower())
                {
                    case "inventorystatus":
                        query = queryParams.SortOrder == 1
                            ? query.OrderBy(x => x.Product.Stock)
                            : query.OrderByDescending(x => x.Product.Stock);

                        break;

                    case "name":
                        query = queryParams.SortOrder == 1
                            ? query.OrderBy(x => x.Product.Name)
                            : query.OrderByDescending(x => x.Product.Name);
                        break;

                    case "price":
                        query = queryParams.SortOrder == 1
                            ? query.OrderBy(x => x.Product.PriceAfterDiscount ?? x.Product.Price)
                            : query.OrderByDescending(x => x.Product.PriceAfterDiscount ?? x.Product.Price);
                        break;
                }
                Console.WriteLine($"SortOrder = '{queryParams.SortOrder}'");
                Console.WriteLine($"SortField = '{queryParams.SortField}'");
            }
            else
            {
                query = query.OrderByDescending(x => x.Order.OrderDate);
            }

            var data = await query.Skip((queryParams.PageNumber - 1) * queryParams.PageSize).Take(queryParams.PageSize).Select(x => new RecentOrderDto
            {
                Id = x.OrderId,
                ProductName = x.Product.Name,
                price = x.Product.PriceAfterDiscount ?? x.Product.Price,
                Status = x.Product.Stock == 0? "OUTOFSTOCK": x.Product.Stock<5 ?"LOWSTOCK": "INSTOCK",
                ProductImage = x.Product.ProductImages.Select(x=>x.ImageUrl).FirstOrDefault()
               
            }).IgnoreQueryFilters().ToListAsync();

           

            return new PagedResponse<RecentOrderDto>(data, totalrecords, queryParams.PageNumber, queryParams.PageSize);
        }

        public async Task<List<decimal>> SalesLast7Days()
        {
            var last7Days = Enumerable.Range(0, 7).Select(i => DateTime.Now.Date.AddDays(-i)).Reverse().ToList();
            var startDate = DateTime.Now.Date.AddDays(-6);

            var salesFromDb = await _appDbContext.Orders
                .Where(x => x.Status == Domain.Enums.OrderStatus.Delivered && x.OrderDate >= startDate)
                .GroupBy(x => x.OrderDate.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(x => x.TotalAmount) })
                .ToListAsync();

            var sellLast7DaysList = last7Days.Select(date =>
                salesFromDb.FirstOrDefault(x => x.Date == date)?.Total ?? 0
            ).ToList();

            return sellLast7DaysList;
        }

        public async Task<DashboardDto> ShowDashboardInfo()
        {
            var query = _appDbContext.Orders.AsQueryable();

           

            var DashboardDto = new DashboardDto
            {
                TotalSales = await query.Where(x => x.Status == Domain.Enums.OrderStatus.Delivered).SumAsync(x => x.TotalAmount),
                TotalOrders = await query.CountAsync(),
                TotalCustomers = await _appDbContext.Users.CountAsync(x => x.RoleId == 2),
                OutOfStockProducts = await _appDbContext.Products.CountAsync(x => x.Stock == 0),
            };

            return DashboardDto;
        }


    }
}
