﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reusables.Exceptions;
using Reusables.Models;
using Reusables.Storage;
using Reusables.Storage.Models;

namespace Reusables.Repositories.EFCore
{
    public class EFCoreImprovedProductsRepository : IProductsRepository
    {
        private readonly AdventureWorksContext _context;
        public EFCoreImprovedProductsRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        private static Func<AdventureWorksContext, int, CancellationToken, Task<Product>> _getProductByIdNoTrackingQuery =
            EF.CompileAsyncQuery<AdventureWorksContext, int, Product>((ctx, productId, ct) =>
                ctx.Products.AsQueryable().AsNoTracking().FirstOrDefault(x => x.ProductId == productId));

        private static Func<AdventureWorksContext, int, CancellationToken, Task<Product>> _getProductByIdFullQuery =
            EF.CompileAsyncQuery<AdventureWorksContext, int, Product>((ctx, productId, ct) =>
                ctx.Products.AsQueryable().AsNoTracking()
                    .Include(x => x.ProductModel)
                    .Include(x => x.ProductSubcategory).ThenInclude(x => x.ProductCategory)
                    .FirstOrDefault(x => x.ProductId == productId));

        private static Func<AdventureWorksContext, int, int, IAsyncEnumerable<Product>> _getProductsPageQuery =
            EF.CompileAsyncQuery<AdventureWorksContext, int, int, Product>((ctx, page, pageSize) =>
                ctx.Products.AsQueryable().AsNoTracking()
                    .OrderBy(x => x.ProductId)
                    .Skip((page - 1) * pageSize).Take(pageSize));

        private static Func<AdventureWorksContext, int, int, IAsyncEnumerable<Product>> _getProductsPageFullQuery =
            EF.CompileAsyncQuery<AdventureWorksContext, int, int, Product>((ctx, page, pageSize) =>
                ctx.Products.AsQueryable().AsNoTracking()
                    .Include(x => x.ProductModel)
                    .Include(x => x.ProductSubcategory).ThenInclude(x => x.ProductCategory)
                    .OrderBy(x => x.ProductId)
                    .Skip((page - 1) * pageSize).Take(pageSize));

        public async Task<int> CreateProduct(AddProductModel newProduct)
        {
            // we have nothing to do here for improvement :(
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
            var product = new Product { ProductId = productId, Name = productName };

            _context.Products.Attach(product);
            _context.Entry(product).Property(x => x.Name).IsModified = true;

            try
            {
                _ = await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ProductNotFoundException();
            }
        }

        public async Task<Product> GetProduct(int productId, CancellationToken cancellationToken = default)
        {
            return await _getProductByIdNoTrackingQuery.Invoke(_context, productId, cancellationToken);
        }

        public async Task<Product> GetProductFull(int productId, CancellationToken cancellationToken = default)
        {
            var product = await _getProductByIdFullQuery.Invoke(_context, productId, cancellationToken);

            SanitizeProduct(product);

            return product;
        }

        public async Task<List<Product>> GetProductsPage(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _getProductsPageQuery.Invoke(_context, page, pageSize).ToListAsync(cancellationToken);
        }

        public async Task<List<Product>> GetProductsPageFull(int page, int pageSize, CancellationToken cancellationToken)
        {
            var products = await _getProductsPageFullQuery.Invoke(_context, page, pageSize).ToListAsync(cancellationToken);

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
