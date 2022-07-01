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

            await CompareWithEf(async (repo) => await repo.GetProduct(productId));
        }

        [Fact]
        public async Task GetByIdFull_ShouldBeEquivalentToEF()
        {
            const int productId = 995;

            await CompareWithEf(async (repo) => await repo.GetProductFull(productId));
        }

        [Fact]
        public async Task GetPage_ShouldBeEquivalentToEF()
        {
            const int pageNumber = 1;
            const int pageSize = 50;

            await CompareWithEf(async (repo) => await repo.GetProductsPage(pageNumber, pageSize));
        }

        [Fact]
        public async Task GetPageFull_ShouldBeEquivalentToEF()
        {
            const int pageNumber = 1;
            const int pageSize = 50;

            await CompareWithEf(async (repo) => await repo.GetProductsPageFull(pageNumber, pageSize));
        }

        [Fact]
        public async Task EditProduct_ShouldBeEquivalentToEF()
        {
            const int productId = 995;
            var productNameDataset = new Bogus.DataSets.Commerce();
            string newProductName = $"{productNameDataset.ProductName().PadLeft(35, ' ').Substring(0, 35)}-{Guid.NewGuid().ToString().Substring(0, 10)}";

            await CompareWithEf(
                getResultsToCompare: async (repo) => await repo.GetProduct(productId),
                doActionWithDapper: async (dapperRepo) => await dapperRepo.EditProductName(productId, newProductName));
        }

        private async Task CompareWithEf<T>(Func<IProductsRepository, Task<T>> getResultsToCompare, Func<IProductsRepository, Task> doActionWithDapper = null)
        {
            using var efServiceProviderScope = EfCoreDefaultServiceProvider.CreateScope();
            using var dapperServiceProviderScope = DapperServiceProvider.CreateScope();

            var efRepository = efServiceProviderScope.ServiceProvider.GetRequiredService<IProductsRepository>();
            var dapperRepository = dapperServiceProviderScope.ServiceProvider.GetRequiredService<IProductsRepository>();

            if (doActionWithDapper != null)
                await doActionWithDapper(dapperRepository);

            var resultFromEf = await getResultsToCompare(efRepository);
            var resultFromDapper = await getResultsToCompare(dapperRepository);

            Assert.NotNull(resultFromEf);
            Assert.NotNull(resultFromDapper);

            resultFromDapper.Should().BeEquivalentTo(resultFromEf);
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
