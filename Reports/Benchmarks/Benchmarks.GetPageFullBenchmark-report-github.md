``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-UYRQFV : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=20  WarmupCount=2  

```
|                                            Method | IterationsCount |    Mean |    Error |   StdDev | Ratio | RatioSD |      Gen 0 |      Gen 1 | Allocated |
|-------------------------------------------------- |---------------- |--------:|---------:|---------:|------:|--------:|-----------:|-----------:|----------:|
|                               GetPageFull_Default |             100 | 3.413 s | 0.0402 s | 0.0463 s |  1.00 |    0.00 | 68000.0000 | 34000.0000 |    544 MB |
|                            GetPageFull_NoTracking |             100 | 2.994 s | 0.0411 s | 0.0473 s |  0.88 |    0.02 | 82000.0000 | 28000.0000 |    655 MB |
|                        GetPageFull_ContextPooling |             100 | 3.368 s | 0.0368 s | 0.0424 s |  0.99 |    0.02 | 66000.0000 | 33000.0000 |    528 MB |
|                         GetPageFull_CompiledQuery |             100 | 3.284 s | 0.0368 s | 0.0424 s |  0.96 |    0.01 | 66000.0000 | 33000.0000 |    529 MB |
|               GetPageFull_DisableConcurrencyCheck |             100 | 3.429 s | 0.0370 s | 0.0426 s |  1.00 |    0.02 | 66000.0000 | 33000.0000 |    532 MB |
| GetPageFull_ContextPoolingDisableConcurrencyCheck |             100 | 3.394 s | 0.0341 s | 0.0392 s |  0.99 |    0.02 | 64000.0000 | 32000.0000 |    516 MB |
|                  GetPageFull_CombinedImprovements |             100 | 2.836 s | 0.0344 s | 0.0396 s |  0.83 |    0.02 | 77000.0000 | 25000.0000 |    619 MB |
|                                GetPageFull_Dapper |             100 | 3.385 s | 0.0297 s | 0.0342 s |  0.99 |    0.02 | 60000.0000 | 19000.0000 |    485 MB |
