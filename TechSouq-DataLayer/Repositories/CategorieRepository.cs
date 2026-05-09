using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;
using TechSouq.Domain.Interfaces;
using TechSouq.Infrastructure.Data;

namespace TechSouq.Infrastructure.Repositories
{
    public class CategorieRepository : ICategorieRepository
    {

        private readonly AppDbContext _appDbContext;

        public CategorieRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> AddCategorie(Categorie categorie)
        {
            _appDbContext.Add(categorie);
           var save = await _appDbContext.SaveChangesAsync();
            return save > 0 ? categorie.Id : 0;
        }

        public async Task<bool> DeleteCategorie(int categorieId)
        {
           return await _appDbContext.Categories.Where(x=>x.Id == categorieId).ExecuteDeleteAsync() > 0;
        }

        public async Task<Categorie> GetCategorie(int categorieId, bool trackingChanges = true)
        {
            var query = _appDbContext.Categories.AsQueryable();

            if (!trackingChanges)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == categorieId); 
        }

        public async Task<bool> UpdateCategorie(Categorie categorie)
        {

           return await _appDbContext.SaveChangesAsync()>0;
        }

        public async Task<bool> IsCategorieExists(int CategorieId)
        {
            return await _appDbContext.Categories.AnyAsync(x => x.Id == CategorieId);
        }

       
    }
}
