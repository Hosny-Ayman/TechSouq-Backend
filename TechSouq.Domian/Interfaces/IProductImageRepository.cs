using System.Threading.Tasks;
using TechSouq.Domain.Entities;


namespace TechSouq.Domain.Interfaces
{
    public interface IProductImageRepository
    {
        Task<int> AddProductImage(ProductImage productImage);
        Task<ProductImage> GetProductImage(int productImageId, bool trackingChanges = true);
        Task<bool> UpdateProductImage(ProductImage productImage);
        Task<bool> DeleteProductImage(int productImageId);
        Task<bool> IsProductImageExists(int productImageId);
    }
}