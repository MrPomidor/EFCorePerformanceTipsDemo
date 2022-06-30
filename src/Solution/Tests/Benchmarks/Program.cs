using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // uncomment to run specific benchmark
            //BenchmarkRunner.Run<GetPageFullBenchmark>(GetGlobalConfig());

            // uncomment to run all benchmarks
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
                            .Run(args, GetGlobalConfig());
        }

        static IConfig GetGlobalConfig()
            => DefaultConfig.Instance
            .AddJob(CoreRuntime.Core60);
    }

    public static class ConfigExtensions
    {
        public static ManualConfig AddJob(this IConfig config, Runtime runtime)
        {
            return config.AddJob(
                Job.Default
                    .WithWarmupCount(2)
                    .WithIterationCount(20)
                    .WithRuntime(runtime)
                );
        }
    }
}