``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-LTJRHJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=20  WarmupCount=2  

```
|                          Method | IterationsCount |       Mean |   Error |  StdDev | Ratio |      Gen 0 |     Gen 1 | Allocated |
|-------------------------------- |---------------- |-----------:|--------:|--------:|------:|-----------:|----------:|----------:|
|                 GetById_Default |            1000 | 1,029.0 ms | 3.29 ms | 3.65 ms |  1.00 | 14000.0000 | 2000.0000 |    112 MB |
|              GetById_NoTracking |            1000 | 1,010.4 ms | 5.14 ms | 5.50 ms |  0.98 | 13000.0000 | 2000.0000 |    110 MB |
|          GetById_ContextPooling |            1000 |   840.8 ms | 4.35 ms | 4.83 ms |  0.82 |  2000.0000 |         - |     23 MB |
|           GetById_CompiledQuery |            1000 |   982.7 ms | 3.99 ms | 4.44 ms |  0.95 | 13000.0000 | 2000.0000 |    106 MB |
| GetById_DisableConcurrencyCheck |            1000 | 1,036.4 ms | 5.14 ms | 5.72 ms |  1.01 | 14000.0000 | 2000.0000 |    113 MB |
|    GetById_CombinedImprovements |            1000 |   782.7 ms | 4.86 ms | 5.60 ms |  0.76 |  1000.0000 |         - |     14 MB |
|                  GetById_Dapper |            1000 |   765.0 ms | 4.18 ms | 4.65 ms |  0.74 |  1000.0000 |         - |     14 MB |
