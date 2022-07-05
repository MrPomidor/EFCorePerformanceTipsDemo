``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-UYRQFV : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=20  WarmupCount=2  

```
|                                            Method | IterationsCount |       Mean |    Error |   StdDev | Ratio | RatioSD |      Gen 0 |     Gen 1 | Allocated |
|-------------------------------------------------- |---------------- |-----------:|---------:|---------:|------:|--------:|-----------:|----------:|----------:|
|                               GetByIdFull_Default |            1000 | 1,186.6 ms | 21.53 ms | 23.94 ms |  1.00 |    0.00 | 17000.0000 | 3000.0000 |    136 MB |
|                            GetByIdFull_NoTracking |            1000 | 1,143.4 ms | 12.89 ms | 14.85 ms |  0.96 |    0.03 | 16000.0000 | 3000.0000 |    130 MB |
|                        GetByIdFull_ContextPooling |            1000 |   984.8 ms | 11.33 ms | 11.63 ms |  0.83 |    0.02 |  5000.0000 |         - |     47 MB |
|                         GetByIdFull_CompiledQuery |            1000 | 1,100.6 ms | 17.13 ms | 19.04 ms |  0.93 |    0.02 | 15000.0000 | 3000.0000 |    124 MB |
|               GetByIdFull_DisableConcurrencyCheck |            1000 | 1,246.2 ms | 57.65 ms | 66.39 ms |  1.05 |    0.06 | 17000.0000 | 3000.0000 |    137 MB |
| GetByIdFull_ContextPoolingDisableConcurrencyCheck |            1000 |   973.0 ms | 22.56 ms | 23.17 ms |  0.82 |    0.03 |  5000.0000 |         - |     45 MB |
|                  GetByIdFull_CombinedImprovements |            1000 |   870.8 ms | 11.35 ms | 11.65 ms |  0.74 |    0.02 |  3000.0000 |         - |     28 MB |
|                                GetByIdFull_Dapper |            1000 |   848.6 ms | 22.19 ms | 23.74 ms |  0.72 |    0.02 |  3000.0000 |         - |     26 MB |
