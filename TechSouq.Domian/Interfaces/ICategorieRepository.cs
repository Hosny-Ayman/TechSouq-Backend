using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Domain.Entities;


namespace TechSouq.Domain.Interfaces
{
    public interface ICategorieRepository
    {
        Task<int> AddCategorie(Categorie categorie);

        Task<Categorie> GetCategorie(int categorieId, bool trackingChanges = true);

        Task<bool> UpdateCategorie(Categorie categorie);

        Task<bool> DeleteCategorie(int categorieId);

        Task<bool> IsCategorieExists(int CategorieId);

    }
}
