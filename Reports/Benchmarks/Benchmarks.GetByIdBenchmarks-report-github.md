``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-UYRQFV : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=20  WarmupCount=2  

```
|                                        Method | IterationsCount |       Mean |    Error |   StdDev | Ratio | RatioSD |      Gen 0 |     Gen 1 | Allocated |
|---------------------------------------------- |---------------- |-----------:|---------:|---------:|------:|--------:|-----------:|----------:|----------:|
|                               GetById_Default |            1000 | 1,067.5 ms | 35.93 ms | 38.44 ms |  1.00 |    0.00 | 14000.0000 | 2000.0000 |    112 MB |
|                            GetById_NoTracking |            1000 | 1,030.2 ms | 15.10 ms | 16.78 ms |  0.97 |    0.04 | 13000.0000 | 2000.0000 |    110 MB |
|                        GetById_ContextPooling |            1000 |   859.2 ms |  9.61 ms |  9.87 ms |  0.81 |    0.03 |  2000.0000 |         - |     23 MB |
|                         GetById_CompiledQuery |            1000 |   989.9 ms |  9.74 ms | 10.83 ms |  0.93 |    0.03 | 13000.0000 | 2000.0000 |    106 MB |
|               GetById_DisableConcurrencyCheck |            1000 | 1,055.8 ms | 12.74 ms | 13.63 ms |  0.99 |    0.04 | 14000.0000 | 2000.0000 |    113 MB |
| GetById_ContextPoolingDisableConcurrencyCheck |            1000 |   886.6 ms | 32.00 ms | 35.57 ms |  0.83 |    0.04 |  2000.0000 |         - |     22 MB |
|                  GetById_CombinedImprovements |            1000 |   818.3 ms | 10.07 ms | 10.35 ms |  0.77 |    0.03 |  1000.0000 |         - |     14 MB |
|                                GetById_Dapper |            1000 |   782.3 ms |  7.36 ms |  8.18 ms |  0.73 |    0.03 |  1000.0000 |         - |     14 MB |
