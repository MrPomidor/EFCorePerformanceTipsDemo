``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-STGOUZ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=20  WarmupCount=2  

```
|                                     Method | IterationsCount |       Mean |    Error |   StdDev | Ratio |     Gen 0 |     Gen 1 | Allocated |
|------------------------------------------- |---------------- |-----------:|---------:|---------:|------:|----------:|----------:|----------:|
|                               Edit_Default |             500 | 2,404.7 ms | 20.28 ms | 23.35 ms |  1.00 | 8000.0000 | 1000.0000 |     70 MB |
|                            Edit_NoTracking |             500 | 2,391.9 ms | 10.62 ms | 11.80 ms |  1.00 | 8000.0000 | 1000.0000 |     70 MB |
|                        Edit_ContextPooling |             500 | 2,245.6 ms | 13.36 ms | 15.38 ms |  0.93 | 3000.0000 |         - |     26 MB |
|                         Edit_CompiledQuery |             500 | 2,358.0 ms | 12.61 ms | 14.01 ms |  0.98 | 8000.0000 | 1000.0000 |     68 MB |
|               Edit_DisableConcurrencyCheck |             500 | 2,400.6 ms |  6.64 ms |  7.10 ms |  1.00 | 8000.0000 | 1000.0000 |     71 MB |
| Edit_ContextPoolingDisableConcurrencyCheck |             500 | 2,258.2 ms | 12.33 ms | 14.20 ms |  0.94 | 3000.0000 |         - |     25 MB |
|            Edit_ContextPoolingRawSqlUpdate |             500 |   920.5 ms |  4.87 ms |  5.00 ms |  0.38 |         - |         - |      4 MB |
|                  Edit_CombinedImprovements |             500 | 1,780.5 ms |  9.07 ms | 10.08 ms |  0.74 | 2000.0000 |         - |     21 MB |
|                                Edit_Dapper |             500 |   916.6 ms |  5.40 ms |  5.54 ms |  0.38 |         - |         - |      3 MB |
