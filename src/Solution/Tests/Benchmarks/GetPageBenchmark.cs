using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Reusables.Repositories;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class GetPageBenchmark : BenchmarkBase
    {
        [Params(500)]
        public int IterationsCount;

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            await Setup();
        }

        [Benchmark(Baseline = true)]
        public async Task GetPage_Default()
        {
            await Do_GetPage(EfCoreDefaultServiceProvider);
        }

        [Benchmark]
        public async Task GetPage_NoTracking()
        {
            await Do_GetPage(EfCoreNoTrackingServiceProvider);
        }

        [Benchmark]
        public async Task GetPage_ContextPooling()
        {
            await Do_GetPage(EfCoreContextPoolingServiceProvider);
        }

        [Benchmark]
        public async Task GetPage_CompiledQuery()
        {
            await Do_GetPage(EfCoreCompiledQueryServiceProvider);
        }

        [Benchmark]
        public async Task GetPage_CombinedImprovements()
        {
            await Do_GetPage(EfCoreCombineImprovementsServiceProvider);
        }

        private async Task Do_GetPage(IServiceProvider serviceProvider)
        {
            for (int i = 0; i < IterationsCount; i++)
            {
                using var scope = serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IProductsRepository>();

                for (int j = 0; j < Pages.Length; j++)
                {
                    var pageNumber = Pages[j];
                    _ = await repository.GetProductsPage(pageNumber, PageSize);
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
