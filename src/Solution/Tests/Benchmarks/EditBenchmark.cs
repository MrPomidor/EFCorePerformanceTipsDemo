using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Benchmarks.Repositories.EFCore;
using Microsoft.Extensions.DependencyInjection;
using Reusables.Repositories;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class EditBenchmark : BenchmarkBase
    {
        [Params(500)]
        public int IterationsCount;

        protected ServiceProvider EfCoreRawSqlUpdatePooledServiceProvider;

        private Bogus.DataSets.Commerce _productNameDataset;

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            await Setup();

            EfCoreRawSqlUpdatePooledServiceProvider = BuildPoolingServiceProvider<EFCoreRawSqlEditProductsRepository>();

            _productNameDataset = new Bogus.DataSets.Commerce();
        }

        [Benchmark(Baseline = true)]
        public async Task Edit_Default()
        {
            await Do_Edit(EfCoreDefaultServiceProvider);
        }

        [Benchmark]
        public async Task Edit_NoTracking()
        {
            await Do_Edit(EfCoreNoTrackingServiceProvider);
        }

        [Benchmark]
        public async Task Edit_ContextPooling()
        {
            await Do_Edit(EfCoreContextPoolingServiceProvider);
        }

        [Benchmark]
        public async Task Edit_CompiledQuery()
        {
            await Do_Edit(EfCoreCompiledQueryServiceProvider);
        }

        [Benchmark]
        public async Task Edit_DisableConcurrencyCheck()
        {
            await Do_Edit(EfCoreNoConcurrencyCheckServiceProvider);
        }

        [Benchmark]
        public async Task Edit_ContextPoolingDisableConcurrencyCheck()
        {
            await Do_Edit(EfCoreContextPoolingNoConcurrencyCheckServiceProvider);
        }

        [Benchmark]
        public async Task Edit_ContextPoolingRawSqlUpdate()
        {
            await Do_Edit(EfCoreRawSqlUpdatePooledServiceProvider);
        }

        [Benchmark]
        public async Task Edit_CombinedImprovements()
        {
            await Do_Edit(EfCoreCombineImprovementsServiceProvider);
        }

        [Benchmark]
        public async Task Edit_Dapper()
        {
            await Do_Edit(DapperDefaultServiceProvider);
        }

        private async Task Do_Edit(IServiceProvider serviceProvider)
        {
            for (int i = 0; i < IterationsCount; i++)
            {
                using var scope = serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IProductsRepository>();

                await repository.EditProductName(ProductIds[i % (ProductIds.Length - 1)], GetNewName());
            }
        }

        private string GetNewName() => $"{_productNameDataset.ProductName().PadLeft(35, ' ').Substring(0, 35)}-{Guid.NewGuid().ToString().Substring(0, 10)}";

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            CleanupServiceProviders();

            try
            {
                EfCoreContextPoolingNoConcurrencyCheckServiceProvider?.Dispose();
                EfCoreContextPoolingNoConcurrencyCheckServiceProvider = null;
            }
            catch { }

            CleanupVariables();

            _productNameDataset = null;
        }
    }
}
