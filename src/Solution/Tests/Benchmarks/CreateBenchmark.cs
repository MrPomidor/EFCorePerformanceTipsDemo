using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Benchmarks.Repositories.EFCore;
using Microsoft.Extensions.DependencyInjection;
using Reusables;
using Reusables.Models;
using Reusables.Repositories;
using Reusables.Storage.Models;
using Reusables.Utils;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class CreateBenchmark : BenchmarkBase
    {
        [Params(500)]
        public int IterationsCount;

        protected ServiceProvider EfCoreRawSqlServiceProvider;
        protected ServiceProvider EfCoreRawSqlPooledServiceProvider;

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            await Setup();

            EfCoreRawSqlServiceProvider = BuildServiceProvider<EFCoreRawSqlEditAndCreateProductsRepository>();
            EfCoreRawSqlPooledServiceProvider = BuildPoolingServiceProvider<EFCoreRawSqlEditAndCreateProductsRepository>();
        }

        private List<AddProductModel> _addProductModels;

        [IterationSetup]
        public void IterationSetup()
        {
            _addProductModels = Enumerable.Range(0, IterationsCount)
                .Select(i => ProductsGenerator.Instance.GenerateProduct())
                .ToList();
        }

        [Benchmark(Baseline = true)]
        public async Task Create_Default()
        {
            await Do_Create(EfCoreDefaultServiceProvider);
        }

        [Benchmark]
        public async Task Create_ContextPooling()
        {
            await Do_Create(EfCoreContextPoolingServiceProvider);
        }

        [Benchmark]
        public async Task Create_DisableConcurrencyCheck()
        {
            await Do_Create(EfCoreNoConcurrencyCheckServiceProvider);
        }

        [Benchmark]
        public async Task Create_ContextPoolingDisableConcurrencyCheck()
        {
            await Do_Create(EfCoreContextPoolingNoConcurrencyCheckServiceProvider);
        }

        [Benchmark]
        public async Task Create_RawSql()
        {
            await Do_Create(EfCoreRawSqlServiceProvider);
        }

        [Benchmark]
        public async Task Create_ContextPoolingRawSql()
        {
            await Do_Create(EfCoreRawSqlPooledServiceProvider);
        }

        [Benchmark]
        public async Task Create_Dapper()
        {
            await Do_Create(DapperDefaultServiceProvider);
        }

        private async Task Do_Create(IServiceProvider serviceProvider)
        {
            for (int i = 0; i < IterationsCount; i++)
            {
                var createModel = _addProductModels[i];

                using var scope = serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IProductsRepository>();

                _ = await repository.CreateProduct(createModel);
            }
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            _addProductModels = null;

            DeleteTestProducts();
        }

        private void DeleteTestProducts()
        {
            const int itemsToTake = 5_000;
            const string testColor = Consts.ApplicationProductsColor;

            using var efServiceProviderScope = EfCoreDefaultServiceProvider.CreateScope();
            var dbContext = efServiceProviderScope.ServiceProvider.GetRequiredService<AdventureWorksContext>();

            long removedItems = 0;
            while (true)
            {
                var testColors = dbContext.Products.Where(x => x.Color == testColor).Take(itemsToTake).ToList();
                if (testColors.Count == 0)
                    break;

                dbContext.Products.RemoveRange(testColors);
                dbContext.SaveChanges();

                removedItems += testColors.Count;
            }
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            CleanupServiceProviders();

            try
            {
                EfCoreRawSqlServiceProvider?.Dispose();
                EfCoreRawSqlServiceProvider = null;
            }
            catch { }

            try
            {
                EfCoreContextPoolingNoConcurrencyCheckServiceProvider?.Dispose();
                EfCoreContextPoolingNoConcurrencyCheckServiceProvider = null;
            }
            catch { }

            CleanupVariables();
        }
    }
}
