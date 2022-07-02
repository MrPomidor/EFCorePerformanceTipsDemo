``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1766 (21H2)
AMD Ryzen 7 3800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-LTJRHJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Runtime=.NET 6.0  IterationCount=20  WarmupCount=2  

```
|                       Method | IterationsCount |       Mean |    Error |   StdDev | Ratio |     Gen 0 |     Gen 1 | Allocated |
|----------------------------- |---------------- |-----------:|---------:|---------:|------:|----------:|----------:|----------:|
|                 Edit_Default |             500 | 2,453.1 ms | 17.35 ms | 19.98 ms |  1.00 | 8000.0000 | 1000.0000 |     70 MB |
|              Edit_NoTracking |             500 | 2,447.5 ms | 11.28 ms | 12.54 ms |  1.00 | 8000.0000 | 1000.0000 |     70 MB |
|          Edit_ContextPooling |             500 | 2,282.9 ms |  9.91 ms | 11.42 ms |  0.93 | 3000.0000 |         - |     26 MB |
|           Edit_CompiledQuery |             500 | 2,400.8 ms | 14.14 ms | 14.52 ms |  0.98 | 8000.0000 | 1000.0000 |     68 MB |
| Edit_DisableConcurrencyCheck |             500 | 2,465.7 ms | 10.51 ms | 11.69 ms |  1.01 | 8000.0000 | 1000.0000 |     71 MB |
|    Edit_CombinedImprovements |             500 | 2,251.1 ms | 15.83 ms | 18.23 ms |  0.92 | 2000.0000 |         - |     23 MB |
|                  Edit_Dapper |             500 |   943.5 ms |  8.09 ms |  8.66 ms |  0.38 |         - |         - |      3 MB |
