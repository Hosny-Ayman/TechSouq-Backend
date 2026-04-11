using System.Threading.Tasks;
using TechSouq.Domain.Entities;

namespace TechSouq.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<int> AddProduct(Product product);
        Task<Product> GetProduct(int productId, bool trackingChanges = true);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int productId);
        Task<bool> IsProductExists(int productId);
    }
}