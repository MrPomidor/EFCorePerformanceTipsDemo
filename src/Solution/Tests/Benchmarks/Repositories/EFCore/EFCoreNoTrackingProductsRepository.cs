using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reusables;
using Reusables.Exceptions;
using Reusables.Models;
using Reusables.Repositories;
using Reusables.Storage;
using Reusables.Storage.Models;

namespace Benchmarks.Repositories.EFCore
{
    public class EFCoreNoTrackingProductsRepository : IProductsRepository
    {
        private readonly AdventureWorksContext _context;
        public EFCoreNoTrackingProductsRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        public async Task<int> CreateProduct(AddProductModel newProduct)
        {
            var product = new Product
            {
                Name = newProduct.Name,
                ProductNumber = newProduct.ProductNumber,
                SafetyStockLevel = newProduct.SafetyStockLevel,
                ReorderPoint = newProduct.ReorderPoint,
                StandardCost = newProduct.StandartCost,
                ListPrice = newProduct.ListPrice,
                Class = newProduct.Class,
                Style = newProduct.Style,
                Color = Consts.ApplicationProductsColor,
                DaysToManufacture = newProduct.DaysToManifacture,
                SellStartDate = newProduct.SellStartDate,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.ProductId;
        }

        public async Task EditProductName(int productId, string productName)
        {
            // IMPORTANT: we should track entities when we need to update/delete
            var product = await _context.Products.AsQueryable().FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product == null)
                throw new ProductNotFoundException();

            product.Name = productName;
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProduct(int productId, CancellationToken cancellationToken = default)
        {
            return await _context.Products.AsQueryable().AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);
        }

        public async Task<Product> GetProductFull(int productId, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.AsQueryable().AsNoTracking()
                .Include(x => x.ProductModel)
                .Include(x => x.ProductSubcategory).ThenInclude(x => x.ProductCategory)
                .FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);

            SanitizeProduct(product);

            return product;
        }

        public async Task<List<Product>> GetProductsPage(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Products.AsQueryable().AsNoTracking()
                .OrderBy(x => x.ProductId)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Product>> GetProductsPageFull(int page, int pageSize, CancellationToken cancellationToken)
        {
            var products = await _context.Products.AsQueryable().AsNoTracking()
                .Include(x => x.ProductModel)
                .Include(x => x.ProductSubcategory).ThenInclude(x => x.ProductCategory)
                .OrderBy(x => x.ProductId)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync(cancellationToken);

            foreach (var product in products)
            {
                SanitizeProduct(product);
            }

            return products;
        }

        public async Task<int> GetTotalProducts(CancellationToken cancellationToken = default)
        {
            return await _context.Products.AsQueryable().CountAsync(cancellationToken);
        }

        private void SanitizeProduct(Product product)
        {
            // in order to prepare entities for consuming code (serialzer) we need to cleanup all references we don't want to expose
            // usually this responsibility is on "mapper" and DTO models, but for simplicity we will left it here
            /**
             * Entity Framework Core will automatically fix-up navigation properties to any other entities that were previously loaded into the context instance. 
             * So even if you don't explicitly include the data for a navigation property, 
             * the property may still be populated if some or all of the related entities were previously loaded.
             * https://docs.microsoft.com/en-us/ef/core/querying/related-data/eager
             */

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
    }
}
