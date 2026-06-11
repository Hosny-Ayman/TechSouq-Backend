using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;
using TechSouq.Domian.Interfaces;
using TechSouq.Infrastructure.Data;

namespace TechSouq.DataLayer.Repositories
{
    public class BrandRepository : IBrandRepository
    {

        private readonly AppDbContext _AppDbContext;

        public BrandRepository (AppDbContext AppDbContext)
        {
            _AppDbContext = AppDbContext;
        }

        public async Task<int> AddBrand(Brand brand)
        {
             _AppDbContext.Brands.Add(brand);

           var save = await _AppDbContext.SaveChangesAsync();

            return save > 0 ? brand.Id : 0;
        }

        public async Task<bool> DeleteBrand(int brandId)
        {
           return await _AppDbContext.Brands.Where(x => x.Id == brandId).ExecuteDeleteAsync() > 0;

        }

        public async Task<Brand> GetBrand(int BrandId, bool trackingChanges = true)
        {
            var query = _AppDbContext.Brands.AsQueryable();

            if(!trackingChanges)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == BrandId);
        }

        public async Task<bool> UpdateBrand(Brand brand)
        {
         

            return await _AppDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsBrandExists(int CartItemId)
        {
            return await _AppDbContext.Brands.AnyAsync(x => x.Id == CartItemId);
        }

        public async Task<List<Brand>> GetAllBrands()
        {
            return await _AppDbContext.Brands.AsNoTracking().ToListAsync();
        }
    }
}
