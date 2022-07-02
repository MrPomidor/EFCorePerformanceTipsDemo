``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-LTJRHJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=20  WarmupCount=2  

```
|                          Method | IterationsCount |    Mean |    Error |   StdDev | Ratio |       Gen 0 |      Gen 1 | Allocated |
|-------------------------------- |---------------- |--------:|---------:|---------:|------:|------------:|-----------:|----------:|
|                 GetPage_Default |             500 | 8.339 s | 0.0230 s | 0.0265 s |  1.00 | 167000.0000 | 80000.0000 |  1,331 MB |
|              GetPage_NoTracking |             500 | 6.602 s | 0.0318 s | 0.0327 s |  0.79 |  83000.0000 | 23000.0000 |    666 MB |
|          GetPage_ContextPooling |             500 | 8.103 s | 0.0243 s | 0.0270 s |  0.97 | 158000.0000 | 67000.0000 |  1,263 MB |
|           GetPage_CompiledQuery |             500 | 8.071 s | 0.0774 s | 0.0892 s |  0.97 | 162000.0000 | 77000.0000 |  1,295 MB |
| GetPage_DisableConcurrencyCheck |             500 | 8.362 s | 0.0464 s | 0.0534 s |  1.00 | 160000.0000 | 69000.0000 |  1,282 MB |
|    GetPage_CombinedImprovements |             500 | 6.035 s | 0.0433 s | 0.0481 s |  0.72 |  66000.0000 | 15000.0000 |    529 MB |
|                  GetPage_Dapper |             500 | 5.614 s | 0.0470 s | 0.0523 s |  0.67 |  63000.0000 | 13000.0000 |    501 MB |
