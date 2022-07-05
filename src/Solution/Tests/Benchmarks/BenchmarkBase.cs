using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Benchmarks.Repositories.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reusables.DI;
using Reusables.Repositories;
using Reusables.Repositories.EFCore;
using Reusables.Storage.Models;

namespace Benchmarks
{
    public abstract class BenchmarkBase
    {
        private const string ConnectionStringName = "AdventureWorks";

        protected const int PageSize = 50;

        protected ServiceProvider EfCoreDefaultServiceProvider;
        protected ServiceProvider EfCoreNoTrackingServiceProvider;
        protected ServiceProvider EfCoreContextPoolingServiceProvider;
        protected ServiceProvider EfCoreCompiledQueryServiceProvider;
        protected ServiceProvider EfCoreNoConcurrencyCheckServiceProvider;
        protected ServiceProvider EfCoreContextPoolingNoConcurrencyCheckServiceProvider;
        protected ServiceProvider EfCoreCombineImprovementsServiceProvider;
        protected ServiceProvider DapperDefaultServiceProvider;

        protected int[] Pages;
        protected int[] ProductIds;

        protected async Task Setup()
        {
            BuildDefaultServiceProvider();
            BuildNoTrackingServiceProvider();
            BuildContextPoolingServiceProvider();
            BuildCompiledQueryServiceProvider();
            BuildNoConcurrencyCheckServiceProvider();
            BuildContextPoolingNoConcurrencyCheckServiceProvider();
            BuildCombinedImprovementsServiceProvider();
            BuildDapperDefaultServiceProvider();

            await SetupProductIds();
        }

        private void BuildDefaultServiceProvider()
        {
            EfCoreDefaultServiceProvider = BuildServiceProvider<EFCoreProductsRepository>();
        }

        private void BuildNoTrackingServiceProvider()
        {
            EfCoreNoTrackingServiceProvider = BuildServiceProvider<EFCoreNoTrackingProductsRepository>();
        }

        private void BuildCompiledQueryServiceProvider()
        {
            EfCoreCompiledQueryServiceProvider = BuildServiceProvider<EFCoreCompiledQueryProductsRepository>();
        }

        private void BuildNoConcurrencyCheckServiceProvider()
        {
            EfCoreNoConcurrencyCheckServiceProvider = BuildServiceProvider<EFCoreProductsRepository>(disableConcurrencyCheck: true);
        }

        protected ServiceProvider BuildServiceProvider<TProductsRepositoryImpl>(bool disableConcurrencyCheck = false)
            where TProductsRepositoryImpl : class, IProductsRepository
        {
            var config = GetConfiguration();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AdventureWorksContext>((dbContextConfig) =>
            {
                dbContextConfig.UseSqlServer(config.GetConnectionString(ConnectionStringName));
                if (disableConcurrencyCheck)
                    dbContextConfig.EnableThreadSafetyChecks(enableChecks: false);
            });
            serviceCollection.AddScoped<IProductsRepository, TProductsRepositoryImpl>();

            return serviceCollection.BuildServiceProvider();
        }

        private void BuildContextPoolingServiceProvider()
        {
            EfCoreContextPoolingServiceProvider = BuildPoolingServiceProvider<EFCoreProductsRepository>();
        }

        private void BuildContextPoolingNoConcurrencyCheckServiceProvider()
        {
            EfCoreContextPoolingNoConcurrencyCheckServiceProvider = BuildPoolingServiceProvider<EFCoreProductsRepository>(disableConcurrencyCheck: true);
        }

        private void BuildCombinedImprovementsServiceProvider()
        {
            EfCoreCombineImprovementsServiceProvider = BuildPoolingServiceProvider<EFCoreImprovedProductsRepository>(disableConcurrencyCheck: true);
        }

        protected ServiceProvider BuildPoolingServiceProvider<TProductsRepositoryImpl>(bool disableConcurrencyCheck = false)
            where TProductsRepositoryImpl : class, IProductsRepository
        {
            const int defaultPoolSize = 1024;

            var config = GetConfiguration();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContextPool<AdventureWorksContext>(
                dbContextConfig => 
                { 
                    dbContextConfig.UseSqlServer(config.GetConnectionString(ConnectionStringName));
                    if (disableConcurrencyCheck)
                        dbContextConfig.EnableThreadSafetyChecks(enableChecks: false);
                },
                poolSize: defaultPoolSize);
            serviceCollection.AddScoped<IProductsRepository, TProductsRepositoryImpl>();

            return serviceCollection.BuildServiceProvider();
        }

        private void BuildDapperDefaultServiceProvider()
        {
            var config = GetConfiguration();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfiguration>(config);
            serviceCollection.AddDapper();

            DapperDefaultServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private IConfigurationRoot GetConfiguration() => new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

        private async Task SetupProductIds()
        {
            using var scope = EfCoreDefaultServiceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IProductsRepository>();
            var totalCount = await repository.GetTotalProducts();
            var pages = (totalCount / PageSize) + 1;
            var productIds = new List<int>(totalCount);
            for (int i = 1; i <= pages; i++)
            {
                var productsPage = await repository.GetProductsPage(i, PageSize);
                productIds.AddRange(productsPage.Select(x => x.ProductId));
            }

            ProductIds = productIds.Distinct().ToArray();
            Pages = Enumerable.Range(1, pages).ToArray();
        }

        protected void CleanupServiceProviders()
        {
            try
            {
                EfCoreDefaultServiceProvider?.Dispose();
                EfCoreDefaultServiceProvider = null;
            }
            catch { }

            try
            {
                EfCoreNoTrackingServiceProvider?.Dispose();
                EfCoreNoTrackingServiceProvider = null;
            }
            catch { }

            try
            {
                EfCoreContextPoolingServiceProvider?.Dispose();
                EfCoreContextPoolingServiceProvider = null;
            }
            catch { }

            try
            {
                EfCoreCompiledQueryServiceProvider?.Dispose();
                EfCoreCompiledQueryServiceProvider = null;
            }
            catch { }

            try
            {
                EfCoreCombineImprovementsServiceProvider?.Dispose();
                EfCoreCombineImprovementsServiceProvider = null;
            }
            catch { }

            try
            {
                EfCoreNoConcurrencyCheckServiceProvider?.Dispose();
                EfCoreNoConcurrencyCheckServiceProvider = null;
            }
            catch { }

            try
            {
                DapperDefaultServiceProvider?.Dispose();
                DapperDefaultServiceProvider = null;
            }
            catch { }

            try
            {
                EfCoreContextPoolingNoConcurrencyCheckServiceProvider?.Dispose();
                EfCoreContextPoolingNoConcurrencyCheckServiceProvider = null;
            }
            catch { }
        }

        protected void CleanupVariables()
        {
            Pages = null;
            ProductIds = null;
        }
    }
}
