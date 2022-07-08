using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Benchmarks.Repositories.EFCore;
using Microsoft.Extensions.DependencyInjection;
using Reusables.Repositories;
using Reusables.Utils;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class EditBenchmark : BenchmarkBase
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
        public async Task Edit_CombinedImprovements()
        {
            await Do_Edit(EfCoreCombineImprovementsServiceProvider);
        }

        [Benchmark]
        public async Task Edit_RawSql()
        {
            await Do_Edit(EfCoreRawSqlServiceProvider);
        }

        [Benchmark]
        public async Task Edit_ContextPoolingRawSql()
        {
            await Do_Edit(EfCoreRawSqlPooledServiceProvider);
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

                await repository.EditProductName(ProductIds[i % (ProductIds.Length - 1)], ProductsGenerator.Instance.GenerateProductName());
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
                EfCoreRawSqlPooledServiceProvider?.Dispose();
                EfCoreRawSqlPooledServiceProvider = null;
            }
            catch { }

            CleanupVariables();
        }
    }
}
