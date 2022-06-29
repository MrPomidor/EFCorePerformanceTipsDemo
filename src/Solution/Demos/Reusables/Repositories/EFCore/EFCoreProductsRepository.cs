using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reusables.Exceptions;
using Reusables.Storage;
using Reusables.Storage.Models;

namespace Reusables.Repositories.EFCore
{
    internal class EFCoreProductsRepository : IProductsRepository
    {
        private readonly AdventureWorksContext _context;
        public EFCoreProductsRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        public async Task EditProductName(long productId, string productName)
        {
            var product = await _context.Products.AsQueryable().FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product == null)
                throw new ProductNotFoundException();

            product.Name = productName;
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProduct(long productId, CancellationToken cancellationToken = default)
        {
            return await _context.Products.AsQueryable().FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);
        }

        public async Task<Product> GetProductFull(long productId, CancellationToken cancellationToken = default)
        {
            return await _context.Products.AsQueryable()
                .Include(x => x.ProductModel)
                .Include(x => x.ProductSubcategory)
                .Include(x => x.SizeUnitMeasureCodeNavigation)
                .Include(x => x.WeightUnitMeasureCodeNavigation)
                .FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);
        }

        public async Task<List<Product>> GetProductsPage(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Products.AsQueryable()
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Product>> GetProductsPageFull(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Products.AsQueryable()
                .Include(x => x.ProductModel)
                .Include(x => x.ProductSubcategory)
                .Include(x => x.SizeUnitMeasureCodeNavigation)
                .Include(x => x.WeightUnitMeasureCodeNavigation)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<long> GetTotalProducts(CancellationToken cancellationToken = default)
        {
            return await _context.Products.AsQueryable().LongCountAsync(cancellationToken);
        }
    }
}
