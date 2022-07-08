using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Reusables.Models;
using Reusables.Storage;

namespace Reusables.Repositories
{
    public interface IProductsRepository
    {
        Task<Product> GetProduct(int productId, CancellationToken cancellationToken = default);
        Task<Product> GetProductFull(int productId, CancellationToken cancellationToken = default);
        Task<List<Product>> GetProductsPage(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<List<Product>> GetProductsPageFull(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<int> GetTotalProducts(CancellationToken cancellationToken = default);
        Task EditProductName(int productId, string productName);
        Task<int> CreateProduct(AddProductModel newProduct);
    }
}
