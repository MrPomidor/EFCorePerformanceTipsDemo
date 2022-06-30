﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Benchmarks.Repositories.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        protected ServiceProvider EfCoreCombineImprovementsServiceProvider;

        protected int[] Pages;
        protected int[] ProductIds;
        protected Random Random;

        protected async Task Setup()
        {
            BuildDefaultServiceProvider();
            BuildNoTrackingServiceProvider();
            BuildContextPoolingServiceProvider();
            BuildCompiledQueryServiceProvider();
            BuildCombinedImprovementsServiceProvider();

            await SetupProductIds();
            Random = new Random();
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

        private ServiceProvider BuildServiceProvider<TProductsRepositoryImpl>()
            where TProductsRepositoryImpl : class, IProductsRepository
        {
            var config = GetConfiguration();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AdventureWorksContext>((dbContextConfig) =>
            {
                dbContextConfig.UseSqlServer(config.GetConnectionString(ConnectionStringName));
            });
            serviceCollection.AddScoped<IProductsRepository, TProductsRepositoryImpl>();

            return serviceCollection.BuildServiceProvider();
        }

        private void BuildContextPoolingServiceProvider()
        {
            EfCoreContextPoolingServiceProvider = BuildPoolingServiceProvider<EFCoreProductsRepository>();
        }

        private void BuildCombinedImprovementsServiceProvider()
        {
            EfCoreCombineImprovementsServiceProvider = BuildPoolingServiceProvider<EFCoreCombineImprovementsProductsRepository>();
        }

        private ServiceProvider BuildPoolingServiceProvider<TProductsRepositoryImpl>()
            where TProductsRepositoryImpl : class, IProductsRepository
        {
            const int defaultPoolSize = 1024;

            var config = GetConfiguration();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContextPool<AdventureWorksContext>(
                dbContextConfig => dbContextConfig.UseSqlServer(config.GetConnectionString(ConnectionStringName)),
                poolSize: defaultPoolSize);
            serviceCollection.AddScoped<IProductsRepository, TProductsRepositoryImpl>();

            return serviceCollection.BuildServiceProvider();
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
        }

        protected void CleanupVariables()
        {
            Pages = null;
            ProductIds = null;
            Random = null;
        }
    }
}