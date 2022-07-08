``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-TVCVMH : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Runtime=.NET 6.0  InvocationCount=1  IterationCount=20  
UnrollFactor=1  WarmupCount=2  

```
|                                       Method | IterationsCount |       Mean |    Error |   StdDev | Ratio |     Gen 0 |     Gen 1 | Allocated |
|--------------------------------------------- |---------------- |-----------:|---------:|---------:|------:|----------:|----------:|----------:|
|                               Create_Default |             500 | 2,015.9 ms | 18.58 ms | 21.40 ms |  1.00 | 9000.0000 | 1000.0000 |     77 MB |
|                        Create_ContextPooling |             500 | 1,867.9 ms | 14.01 ms | 16.13 ms |  0.93 | 4000.0000 | 1000.0000 |     32 MB |
|               Create_DisableConcurrencyCheck |             500 | 2,031.8 ms | 11.40 ms | 13.13 ms |  1.01 | 9000.0000 | 1000.0000 |     78 MB |
| Create_ContextPoolingDisableConcurrencyCheck |             500 | 1,876.7 ms | 11.33 ms | 12.12 ms |  0.93 | 4000.0000 | 1000.0000 |     32 MB |
|                                Create_RawSql |             500 | 1,069.0 ms |  9.39 ms | 10.44 ms |  0.53 | 6000.0000 | 1000.0000 |     53 MB |
|                  Create_ContextPoolingRawSql |             500 |   947.9 ms | 10.20 ms | 11.75 ms |  0.47 | 1000.0000 | 1000.0000 |      9 MB |
|                                Create_Dapper |             500 |   941.3 ms | 12.76 ms | 14.70 ms |  0.47 |         - |         - |      7 MB |
