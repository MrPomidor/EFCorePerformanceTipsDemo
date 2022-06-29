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

        public async Task EditProductName(int productId, string productName)
        {
            var product = await _context.Products.AsQueryable().FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product == null)
                throw new ProductNotFoundException();

            product.Name = productName;
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProduct(int productId, CancellationToken cancellationToken = default)
        {
            return await _context.Products.AsQueryable().FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);
        }

        public async Task<Product> GetProductFull(int productId, CancellationToken cancellationToken = default)
        {
            return await _context.Products.AsQueryable()
                .Include(x => x.ProductModel)
                .Include(x => x.ProductSubcategory).ThenInclude(x => x.ProductCategory)
                .FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);
        }

        public async Task<List<Product>> GetProductsPage(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Products.AsQueryable()
                .OrderBy(x => x.ProductId)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Product>> GetProductsPageFull(int page, int pageSize, CancellationToken cancellationToken)
        {
            var products = await _context.Products.AsQueryable()
                .Include(x => x.ProductModel)
                .Include(x => x.ProductSubcategory).ThenInclude(x => x.ProductCategory)
                .OrderBy(x => x.ProductId)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync(cancellationToken);

            /**
             * Entity Framework Core will automatically fix-up navigation properties to any other entities that were previously loaded into the context instance. 
             * So even if you don't explicitly include the data for a navigation property, 
             * the property may still be populated if some or all of the related entities were previously loaded.
             * https://docs.microsoft.com/en-us/ef/core/querying/related-data/eager
             */
            foreach (var product in products)
            {
                if (product?.ProductModel != null)
                    product.ProductModel.Products = null;

                if (product?.ProductSubcategory != null)
                {
                    product.ProductSubcategory.Products = null;

                    if (product.ProductSubcategory?.ProductCategory != null)
                    {
                        product.ProductSubcategory.ProductCategory.ProductSubcategories = null;
                    }
                }
            }

            return products;
        }

        public async Task<long> GetTotalProducts(CancellationToken cancellationToken = default)
        {
            return await _context.Products.AsQueryable().LongCountAsync(cancellationToken);
        }
    }
}
