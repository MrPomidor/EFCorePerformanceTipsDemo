using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Reusables.Repositories;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class GetPageFullBenchmark : BenchmarkBase
    {
        [Params(100)]
        public int IterationsCount;

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            await Setup();
        }

        [Benchmark(Baseline = true)]
        public async Task GetPageFull_Default()
        {
            await Do_GetPageFull(EfCoreDefaultServiceProvider);
        }

        [Benchmark]
        public async Task GetPageFull_NoTracking()
        {
            await Do_GetPageFull(EfCoreNoTrackingServiceProvider);
        }

        [Benchmark]
        public async Task GetPageFull_ContextPooling()
        {
            await Do_GetPageFull(EfCoreContextPoolingServiceProvider);
        }

        [Benchmark]
        public async Task GetPageFull_CompiledQuery()
        {
            await Do_GetPageFull(EfCoreCompiledQueryServiceProvider);
        }

        [Benchmark]
        public async Task GetPageFull_DisableConcurrencyCheck()
        {
            await Do_GetPageFull(EfCoreNoConcurrencyCheckServiceProvider);
        }

        [Benchmark]
        public async Task GetPageFull_CombinedImprovements()
        {
            await Do_GetPageFull(EfCoreCombineImprovementsServiceProvider);
        }

        private async Task Do_GetPageFull(IServiceProvider serviceProvider)
        {
            for (int i = 0; i < IterationsCount; i++)
            {
                using var scope = serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IProductsRepository>();

                for (int j = 0; j < Pages.Length; j++)
                {
                    var pageNumber = Pages[j];
                    _ = await repository.GetProductsPageFull(pageNumber, PageSize);
                }
            }
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            CleanupServiceProviders();
            CleanupVariables();
        }
    }
}
