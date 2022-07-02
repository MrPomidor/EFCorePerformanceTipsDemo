``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-LTJRHJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=20  WarmupCount=2  

```
|                              Method | IterationsCount |    Mean |    Error |   StdDev | Ratio | RatioSD |      Gen 0 |      Gen 1 | Allocated |
|------------------------------------ |---------------- |--------:|---------:|---------:|------:|--------:|-----------:|-----------:|----------:|
|                 GetPageFull_Default |             100 | 3.421 s | 0.0231 s | 0.0257 s |  1.00 |    0.00 | 68000.0000 | 34000.0000 |    543 MB |
|              GetPageFull_NoTracking |             100 | 2.958 s | 0.0331 s | 0.0381 s |  0.87 |    0.01 | 82000.0000 | 27000.0000 |    655 MB |
|          GetPageFull_ContextPooling |             100 | 3.340 s | 0.0292 s | 0.0336 s |  0.98 |    0.01 | 66000.0000 | 33000.0000 |    529 MB |
|           GetPageFull_CompiledQuery |             100 | 3.297 s | 0.0269 s | 0.0309 s |  0.96 |    0.01 | 66000.0000 | 33000.0000 |    529 MB |
| GetPageFull_DisableConcurrencyCheck |             100 | 3.402 s | 0.0431 s | 0.0496 s |  0.99 |    0.02 | 66000.0000 | 33000.0000 |    532 MB |
|    GetPageFull_CombinedImprovements |             100 | 2.821 s | 0.0305 s | 0.0351 s |  0.83 |    0.01 | 77000.0000 | 25000.0000 |    619 MB |
|                  GetPageFull_Dapper |             100 | 3.367 s | 0.0334 s | 0.0385 s |  0.98 |    0.01 | 60000.0000 | 18000.0000 |    485 MB |
