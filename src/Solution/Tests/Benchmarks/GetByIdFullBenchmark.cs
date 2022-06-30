using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Reusables.Repositories;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class GetByIdFullBenchmark : BenchmarkBase
    {
        [Params(1000)]
        public int IterationsCount;

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            await Setup();
        }

        [Benchmark(Baseline = true)]
        public async Task GetByIdFull_Default()
        {
            await Do_GetByIdFull(EfCoreDefaultServiceProvider);
        }

        [Benchmark]
        public async Task GetByIdFull_NoTracking()
        {
            await Do_GetByIdFull(EfCoreNoTrackingServiceProvider);
        }

        [Benchmark]
        public async Task GetByIdFull_ContextPooling()
        {
            await Do_GetByIdFull(EfCoreContextPoolingServiceProvider);
        }

        [Benchmark]
        public async Task GetByIdFull_CompiledQuery()
        {
            await Do_GetByIdFull(EfCoreCompiledQueryServiceProvider);
        }

        [Benchmark]
        public async Task GetByIdFull_CombinedImprovements()
        {
            await Do_GetByIdFull(EfCoreCombineImprovementsServiceProvider);
        }

        private async Task Do_GetByIdFull(IServiceProvider serviceProvider)
        {
            for (int i = 0; i < IterationsCount; i++)
            {
                using var scope = serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IProductsRepository>();

                var product = await repository.GetProductFull(ProductIds[Random.Next(0, ProductIds.Length - 1)]);
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
