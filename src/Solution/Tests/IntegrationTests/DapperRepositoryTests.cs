using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reusables.DI;
using Reusables.Repositories;
using Xunit;

namespace IntegrationTests
{
    public class DapperRepositoryTests : IDisposable
    {
        private ServiceProvider EfCoreDefaultServiceProvider;
        private ServiceProvider DapperServiceProvider;

        public DapperRepositoryTests()
        {
            EfCoreDefaultServiceProvider = BuildEFServiceProvider();
            DapperServiceProvider = BuildDapperServiceProvider();
        }

        private ServiceProvider BuildEFServiceProvider()
        {
            var config = GetConfiguration();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddEfCore(config);

            return serviceCollection.BuildServiceProvider();
        }

        private ServiceProvider BuildDapperServiceProvider()
        {
            var config = GetConfiguration();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfiguration>(config);
            serviceCollection.AddDapper();

            return serviceCollection.BuildServiceProvider();
        }

        private IConfigurationRoot GetConfiguration() => new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

        [Fact]
        public async Task GetById_ShouldBeEquivalentToEF()
        {
            const int productId = 995;

            await InServiceProvidersScope(async (efRepository, dapperRepository) =>
            {
                var resultFromEf = await efRepository.GetProduct(productId);
                var resultFromDapper = await dapperRepository.GetProduct(productId);

                Assert.NotNull(resultFromEf);
                Assert.NotNull(resultFromDapper);

                resultFromDapper.Should().BeEquivalentTo(resultFromEf);
            });
        }

        [Fact]
        public async Task GetByIdFull_ShouldBeEquivalentToEF()
        {
            const int productId = 995;

            await InServiceProvidersScope(async (efRepository, dapperRepository) =>
            {
                var resultFromEf = await efRepository.GetProductFull(productId);
                var resultFromDapper = await dapperRepository.GetProductFull(productId);

                Assert.NotNull(resultFromEf);
                Assert.NotNull(resultFromDapper);

                resultFromDapper.Should().BeEquivalentTo(resultFromEf);
            });
        }

        [Fact]
        public async Task GetPage_ShouldBeEquivalentToEF()
        {
            const int pageSize = 50;

            await InServiceProvidersScope(async (efRepository, dapperRepository) =>
            {
                var totalCountEf = await efRepository.GetTotalProducts();
                var totalCountDapper = await dapperRepository.GetTotalProducts();

                Assert.Equal(totalCountEf, totalCountDapper);

                var pages = (totalCountDapper / pageSize) + 1;
                for (int currentPage =1; currentPage <= pages; currentPage++)
{
                    var resultFromEf = await efRepository.GetProductsPage(currentPage, pageSize);
                    var resultFromDapper = await dapperRepository.GetProductsPage(currentPage, pageSize);

                    Assert.NotNull(resultFromEf);
                    Assert.NotNull(resultFromDapper);

                    resultFromDapper.Should().BeEquivalentTo(resultFromEf, $"page {currentPage} results should be the same");
                }
            });
        }

        [Fact]
        public async Task GetPageFull_ShouldBeEquivalentToEF()
        {
            const int pageSize = 50;

            await InServiceProvidersScope(async (efRepository, dapperRepository) =>
            {
                var totalCountEf = await efRepository.GetTotalProducts();
                var totalCountDapper = await dapperRepository.GetTotalProducts();

                Assert.Equal(totalCountEf, totalCountDapper);

                var pages = (totalCountDapper / pageSize) + 1;
                for (int currentPage = 1; currentPage <= pages; currentPage++)
                {
                    var resultFromEf = await efRepository.GetProductsPageFull(currentPage, pageSize);
                    var resultFromDapper = await dapperRepository.GetProductsPageFull(currentPage, pageSize);

                    Assert.NotNull(resultFromEf);
                    Assert.NotNull(resultFromDapper);

                    resultFromDapper.Should().BeEquivalentTo(resultFromEf, $"page {currentPage} results should be the same");
                }
            });
        }

        [Fact]
        public async Task EditProduct_ShouldBeEquivalentToEF()
        {
            const int productId = 995;
            var productNameDataset = new Bogus.DataSets.Commerce();
            string newProductName = $"{productNameDataset.ProductName().PadLeft(35, ' ').Substring(0, 35)}-{Guid.NewGuid().ToString().Substring(0, 10)}";

            await InServiceProvidersScope(async (efRepository, dapperRepository) =>
            {
                await dapperRepository.EditProductName(productId, newProductName);

                var resultFromEf = await efRepository.GetProductFull(productId);
                var resultFromDapper = await dapperRepository.GetProductFull(productId);

                Assert.NotNull(resultFromEf);
                Assert.NotNull(resultFromDapper);

                Assert.True(resultFromEf.Name.Equals(newProductName));
                Assert.True(resultFromDapper.Name.Equals(newProductName));

                resultFromDapper.Should().BeEquivalentTo(resultFromEf);
            });
        }

        private async Task InServiceProvidersScope(Func<IProductsRepository, IProductsRepository, Task> doTestAsync)
        {
            using var efServiceProviderScope = EfCoreDefaultServiceProvider.CreateScope();
            using var dapperServiceProviderScope = DapperServiceProvider.CreateScope();

            var efRepository = efServiceProviderScope.ServiceProvider.GetRequiredService<IProductsRepository>();
            var dapperRepository = dapperServiceProviderScope.ServiceProvider.GetRequiredService<IProductsRepository>();

            await doTestAsync(efRepository, dapperRepository);
        }

        public void Dispose()
        {
            try
            {
                EfCoreDefaultServiceProvider?.Dispose();
                EfCoreDefaultServiceProvider = null;
            }
            catch { }

            try
            {
                DapperServiceProvider?.Dispose();
                DapperServiceProvider = null;
            }
            catch { }
        }
    }
}
