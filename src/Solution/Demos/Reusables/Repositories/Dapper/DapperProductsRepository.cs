using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Reusables.Storage;

namespace Reusables.Repositories.Dapper
{
    // TODO implement
    public class DapperProductsRepository : IProductsRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public DapperProductsRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task EditProductName(int productId, string productName)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProduct(int productId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductFull(int productId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetProductsPage(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetProductsPageFull(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalProducts(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
