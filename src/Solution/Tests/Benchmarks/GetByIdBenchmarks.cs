﻿using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Reusables.Repositories;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class GetByIdBenchmarks : BenchmarkBase
    {
        [Params(1000)]
        public int IterationsCount;

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            await Setup();
        }

        [Benchmark(Baseline = true)]
        public async Task GetById_Default()
        {
            await Do_GetById(EfCoreDefaultServiceProvider);
        }

        [Benchmark]
        public async Task GetById_NoTracking()
        {
            await Do_GetById(EfCoreNoTrackingServiceProvider);
        }

        [Benchmark]
        public async Task GetById_ContextPooling()
        {
            await Do_GetById(EfCoreContextPoolingServiceProvider);
        }

        [Benchmark]
        public async Task GetById_CompiledQuery()
        {
            await Do_GetById(EfCoreCompiledQueryServiceProvider);
        }

        [Benchmark]
        public async Task GetById_CombinedImprovements()
        {
            await Do_GetById(EfCoreCombineImprovementsServiceProvider);
        }

        private async Task Do_GetById(IServiceProvider serviceProvider)
        {
            for (int i = 0; i < IterationsCount; i++)
            {
                using var scope = serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IProductsRepository>();

                var product = await repository.GetProduct(ProductIds[Random.Next(0, ProductIds.Length - 1)]);
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