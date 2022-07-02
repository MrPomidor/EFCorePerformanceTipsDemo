``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-LTJRHJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=20  WarmupCount=2  

```
|                              Method | IterationsCount |       Mean |   Error |  StdDev | Ratio |      Gen 0 |     Gen 1 | Allocated |
|------------------------------------ |---------------- |-----------:|--------:|--------:|------:|-----------:|----------:|----------:|
|                 GetByIdFull_Default |            1000 | 1,150.9 ms | 8.39 ms | 9.32 ms |  1.00 | 16000.0000 | 3000.0000 |    135 MB |
|              GetByIdFull_NoTracking |            1000 | 1,118.2 ms | 6.01 ms | 6.68 ms |  0.97 | 16000.0000 | 3000.0000 |    131 MB |
|          GetByIdFull_ContextPooling |            1000 |   954.4 ms | 7.10 ms | 7.59 ms |  0.83 |  5000.0000 |         - |     46 MB |
|           GetByIdFull_CompiledQuery |            1000 | 1,066.6 ms | 3.56 ms | 4.10 ms |  0.93 | 15000.0000 | 3000.0000 |    125 MB |
| GetByIdFull_DisableConcurrencyCheck |            1000 | 1,156.7 ms | 4.37 ms | 5.04 ms |  1.00 | 17000.0000 | 3000.0000 |    139 MB |
|    GetByIdFull_CombinedImprovements |            1000 |   850.6 ms | 4.49 ms | 5.17 ms |  0.74 |  3000.0000 |         - |     27 MB |
|                  GetByIdFull_Dapper |            1000 |   824.3 ms | 2.35 ms | 2.31 ms |  0.72 |  3000.0000 |         - |     27 MB |
