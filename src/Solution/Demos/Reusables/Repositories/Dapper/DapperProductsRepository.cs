using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Reusables.Exceptions;
using Reusables.Storage;

namespace Reusables.Repositories.Dapper
{
    public class DapperProductsRepository : IProductsRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public DapperProductsRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private const string EditProductNameQuery = @"UPDATE [Production].[Product]
        SET [Name] = @newName
        WHERE [ProductID] = @id
        SELECT @@ROWCOUNT";
        public async Task EditProductName(int productId, string productName)
        {
            using var connection = _connectionFactory.GetConnection();
            var affectedRows = await connection.QueryFirstOrDefaultAsync<int>(EditProductNameQuery, new { id = productId, newName = productName });
            if (affectedRows == 0)
                throw new ProductNotFoundException();
        }

        private const string GetProductSelect = @"SELECT 
	        [p].[ProductID], [p].[Class], [p].[Color], [p].[DaysToManufacture], [p].[DiscontinuedDate], [p].[FinishedGoodsFlag], 
	        [p].[ListPrice], [p].[MakeFlag], [p].[ModifiedDate], [p].[Name], [p].[ProductLine], [p].[ProductModelID], 
	        [p].[ProductNumber], [p].[ProductSubcategoryID], [p].[ReorderPoint], [p].[rowguid], [p].[SafetyStockLevel], 
	        [p].[SellEndDate], [p].[SellStartDate], [p].[Size], [p].[SizeUnitMeasureCode], [p].[StandardCost], 
	        [p].[Style], [p].[Weight], [p].[WeightUnitMeasureCode]
        FROM [Production].[Product] AS [p]";
        private static readonly string GetProductQuery = $@"{GetProductSelect}
        WHERE [p].ProductID = @id";
        public async Task<Product> GetProduct(int productId, CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.GetConnection();
            return await connection.QuerySingleOrDefaultAsync<Product>(
                new CommandDefinition(GetProductQuery, parameters: new { id = productId }, cancellationToken: cancellationToken));
        }

        private const string GetProductFullSelect = @"SELECT 
	        [p].[ProductID], [p].[Class], [p].[Color], [p].[DaysToManufacture], [p].[DiscontinuedDate], [p].[FinishedGoodsFlag], 
	        [p].[ListPrice], [p].[MakeFlag], [p].[ModifiedDate], [p].[Name], [p].[ProductLine], [p].[ProductModelID], 
	        [p].[ProductNumber], [p].[ProductSubcategoryID], [p].[ReorderPoint], [p].[rowguid], [p].[SafetyStockLevel], 
	        [p].[SellEndDate], [p].[SellStartDate], [p].[Size], [p].[SizeUnitMeasureCode], [p].[StandardCost], [p].[Style], 
	        [p].[Weight], [p].[WeightUnitMeasureCode], 
	        [p0].[ProductModelID], [p0].[CatalogDescription], [p0].[Instructions], [p0].[ModifiedDate], [p0].[Name], [p0].[rowguid], 
	        [p1].[ProductSubcategoryID], [p1].[ModifiedDate], [p1].[Name], [p1].[ProductCategoryID], [p1].[rowguid], 
	        [p2].[ProductCategoryID], [p2].[ModifiedDate], [p2].[Name], [p2].[rowguid]
        FROM [Production].[Product] AS [p]
        LEFT JOIN [Production].[ProductModel] AS [p0] ON [p].[ProductModelID] = [p0].[ProductModelID]
        LEFT JOIN [Production].[ProductSubcategory] AS [p1] ON [p].[ProductSubcategoryID] = [p1].[ProductSubcategoryID]
        LEFT JOIN [Production].[ProductCategory] AS [p2] ON [p1].[ProductCategoryID] = [p2].[ProductCategoryID]";
        private static readonly string GetProductFullQuery = $@"{GetProductFullSelect}
        WHERE [p].[ProductID] = @id";
        public async Task<Product> GetProductFull(int productId, CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.GetConnection();
            var product = (await connection.QueryAsync<Product, ProductModel, ProductSubcategory, ProductCategory, Product>(
                new CommandDefinition(GetProductFullQuery, parameters: new { id = productId }, cancellationToken: cancellationToken),
                map: (product, productModel, productSubcategory, productCategory) =>
                {
                    product.ProductModel = productModel;
                    product.ProductSubcategory = productSubcategory;
                    if (productSubcategory != null)
                        product.ProductSubcategory.ProductCategory = productCategory;
                    return product;
                },
                splitOn: "ProductModelID,ProductSubcategoryID,ProductCategoryID"))
                .SingleOrDefault();

            SanitizeProduct(product);
            return product;
        }

        private static readonly string GetProductsPageQuery = $@"{GetProductSelect}
        ORDER BY [p].[ProductID]
        OFFSET @offset ROWS
        FETCH NEXT @fetch ROWS ONLY";
        public async Task<List<Product>> GetProductsPage(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var offset = (page - 1) * pageSize;
            var fetch = pageSize;

            using var connection = _connectionFactory.GetConnection();
            return (await connection.QueryAsync<Product>(
                new CommandDefinition(GetProductsPageQuery, parameters: new { offset = offset, fetch = fetch }, cancellationToken: cancellationToken)))
                .AsList();
        }

        private static readonly string GetProductsPageFullQuery = $@"{GetProductFullSelect}
        ORDER BY [p].[ProductID]
        OFFSET @offset ROWS
        FETCH NEXT @fetch ROWS ONLY";
        public async Task<List<Product>> GetProductsPageFull(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var offset = (page - 1) * pageSize;
            var fetch = pageSize;

            using var connection = _connectionFactory.GetConnection();
            return (await connection.QueryAsync<Product, ProductModel, ProductSubcategory, ProductCategory, Product>(
                new CommandDefinition(GetProductsPageFullQuery, parameters: new { offset = offset, fetch = fetch }, cancellationToken: cancellationToken),
                map: (product, productModel, productSubcategory, productCategory) =>
                {
                    product.ProductModel = productModel;
                    product.ProductSubcategory = productSubcategory;
                    if (productSubcategory != null)
                        product.ProductSubcategory.ProductCategory = productCategory;
                    return product;
                },
                splitOn: "ProductModelID,ProductSubcategoryID,ProductCategoryID"))
                .Select(x => { SanitizeProduct(x); return x; })
                .AsList();
        }

        private const string GetTotalProductsQuery = @"SELECT COUNT(*) FROM Production.Product";
        public async Task<int> GetTotalProducts(CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.GetConnection();
            return await connection.QueryFirstAsync<int>(new CommandDefinition(GetTotalProductsQuery, cancellationToken: cancellationToken));
        }

        private void SanitizeProduct(Product product)
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
    }
}
