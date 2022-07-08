``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-EAVWRY : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=20  WarmupCount=2  

```
|                                     Method | IterationsCount |       Mean |    Error |   StdDev | Ratio |     Gen 0 |     Gen 1 | Allocated |
|------------------------------------------- |---------------- |-----------:|---------:|---------:|------:|----------:|----------:|----------:|
|                               Edit_Default |             500 | 2,425.0 ms | 13.96 ms | 15.52 ms |  1.00 | 8000.0000 | 1000.0000 |     70 MB |
|                            Edit_NoTracking |             500 | 2,420.3 ms | 11.37 ms | 13.09 ms |  1.00 | 8000.0000 | 1000.0000 |     70 MB |
|                        Edit_ContextPooling |             500 | 2,275.8 ms | 16.43 ms | 18.92 ms |  0.94 | 3000.0000 |         - |     26 MB |
|                         Edit_CompiledQuery |             500 | 2,382.3 ms | 13.84 ms | 15.38 ms |  0.98 | 8000.0000 | 1000.0000 |     68 MB |
|               Edit_DisableConcurrencyCheck |             500 | 2,438.3 ms | 15.69 ms | 17.44 ms |  1.01 | 8000.0000 | 1000.0000 |     71 MB |
| Edit_ContextPoolingDisableConcurrencyCheck |             500 | 2,282.6 ms | 16.59 ms | 19.10 ms |  0.94 | 3000.0000 |         - |     25 MB |
|                  Edit_CombinedImprovements |             500 | 1,810.6 ms | 11.40 ms | 13.12 ms |  0.75 | 2000.0000 |         - |     21 MB |
|                                Edit_RawSql |             500 | 1,058.7 ms |  5.03 ms |  5.39 ms |  0.44 | 6000.0000 | 1000.0000 |     48 MB |
|                  Edit_ContextPoolingRawSql |             500 |   940.6 ms |  9.83 ms | 11.32 ms |  0.39 |         - |         - |      4 MB |
|                                Edit_Dapper |             500 |   935.2 ms |  6.88 ms |  7.92 ms |  0.39 |         - |         - |      3 MB |
