``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-UYRQFV : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=20  WarmupCount=2  

```
|                                        Method | IterationsCount |    Mean |    Error |   StdDev | Ratio | RatioSD |       Gen 0 |      Gen 1 | Allocated |
|---------------------------------------------- |---------------- |--------:|---------:|---------:|------:|--------:|------------:|-----------:|----------:|
|                               GetPage_Default |             500 | 8.752 s | 0.1859 s | 0.2141 s |  1.00 |    0.00 | 166000.0000 | 83000.0000 |  1,329 MB |
|                            GetPage_NoTracking |             500 | 6.784 s | 0.1543 s | 0.1777 s |  0.78 |    0.03 |  83000.0000 | 21000.0000 |    666 MB |
|                        GetPage_ContextPooling |             500 | 8.105 s | 0.0575 s | 0.0615 s |  0.93 |    0.02 | 158000.0000 | 66000.0000 |  1,261 MB |
|                         GetPage_CompiledQuery |             500 | 8.063 s | 0.0206 s | 0.0229 s |  0.92 |    0.02 | 162000.0000 | 75000.0000 |  1,295 MB |
|               GetPage_DisableConcurrencyCheck |             500 | 8.426 s | 0.0193 s | 0.0206 s |  0.96 |    0.02 | 160000.0000 | 66000.0000 |  1,280 MB |
| GetPage_ContextPoolingDisableConcurrencyCheck |             500 | 8.102 s | 0.0841 s | 0.0968 s |  0.93 |    0.03 | 152000.0000 | 65000.0000 |  1,211 MB |
|                  GetPage_CombinedImprovements |             500 | 6.126 s | 0.0312 s | 0.0334 s |  0.70 |    0.02 |  66000.0000 | 16000.0000 |    529 MB |
|                                GetPage_Dapper |             500 | 5.658 s | 0.0514 s | 0.0528 s |  0.65 |    0.02 |  63000.0000 | 13000.0000 |    501 MB |
