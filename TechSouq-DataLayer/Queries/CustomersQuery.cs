using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.IO;
using TechSouq.Application.Dtos;
using TechSouq.Application.Helper;
using TechSouq.Application.Queries;
using TechSouq.Domain.Enums;
using TechSouq.Infrastructure.Data;

namespace TechSouq.Infrastructure.Queries
{
    public class CustomersQuery:ICustomersQuery
    {
        private readonly AppDbContext _appDbContext;
       

        public CustomersQuery(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
           
        }

        public async Task<PagedResponse<CustomerSummaryDto>> GetAllCustomersPaged(int pageNumber, int pageSize, string? EmailSearch)
        {
            var query = _appDbContext.Users.AsNoTracking();



            if (!string.IsNullOrWhiteSpace(EmailSearch))
            {
                query = query.Where(x => x.Email.Contains(EmailSearch));
            }

            var TotalRecords = await query.CountAsync();

            var data = await query.OrderBy(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => new CustomerSummaryDto
            {

                Id = x.Id,
                FullName = x.FirstName + " " + x.SecondName,
                Email = x.Email,
                TotalOrders = x.Orders.Count,
                TotalAmountSpent = x.Orders.Where(t => t.Status == Domain.Enums.OrderStatus.Delivered).Sum(o => (decimal?)o.TotalAmount) ?? 0,
                IsActive = x.IsActive

            }).ToListAsync();


            return new PagedResponse<CustomerSummaryDto>(data, TotalRecords, pageNumber, pageSize);
        }

        public async Task<CustomerDetailsDto?> GetCustomerDetails(int customerId)
        {
            return await _appDbContext.Users
        .AsNoTracking()
        .Where(x => x.Id == customerId)
        .Select(x => new CustomerDetailsDto
        {
            Id = x.Id,
            FullName = $"{x.FirstName} {x.SecondName}",
            Email = x.Email,

            Addresses = x.Addresses
                .Select(a => new AddressOrderDetilsDto
                {
                    Id = a.Id,
                    City = a.City,
                    Street = a.Street,
                    Building = a.building,
                    Country = a.country,
                    Active = a.Active
                })
                .ToList(),

            RecentOrders = x.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .Select(o => new CustomerOrderDto
                {
                    Id = o.Id,
                    OrderNumber = $"#TS-{100000 + o.Id}",
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalAmount,
                    OrderStatus = o.Status,
                    TotalItems = o.OrderItems.Count
                })
                .ToList()
        })
        .SingleOrDefaultAsync();
        }

        public async Task<bool> SetActive(int CustomerId)
        {
            return await _appDbContext.Users.Where(s => s.Id == CustomerId).ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActive, p => !p.IsActive)) > 0 ;
        }
    }
}
