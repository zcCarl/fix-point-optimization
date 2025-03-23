```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.0 (24A335) [Darwin 24.0.0]
Apple M1 Max, 1 CPU, 10 logical and 10 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD


```
| Method                                 | Mean       | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------------------- |-----------:|----------:|----------:|------:|--------:|----------:|------------:|
| VectorTransform_SystemMatrix           |   1.514 μs | 0.0073 μs | 0.0064 μs |  0.22 |    0.00 |         - |          NA |
| Determinant_SystemMatrix               |   4.059 μs | 0.0257 μs | 0.0228 μs |  0.58 |    0.00 |         - |          NA |
| MatrixMultiplication_SystemMatrix      |   7.035 μs | 0.0325 μs | 0.0272 μs |  1.00 |    0.01 |         - |          NA |
| Transpose_SystemMatrix                 |   7.070 μs | 0.0134 μs | 0.0112 μs |  1.00 |    0.00 |         - |          NA |
| Inverse_SystemMatrix                   |  20.823 μs | 0.1120 μs | 0.1048 μs |  2.96 |    0.02 |         - |          NA |
| VectorTransform_OriginalTSMatrix       |  24.970 μs | 0.1313 μs | 0.1097 μs |  3.55 |    0.02 |         - |          NA |
| VectorTransform_OptimizedTSMatrix      |  25.121 μs | 0.1144 μs | 0.0955 μs |  3.57 |    0.02 |         - |          NA |
| Determinant_OptimizedTSMatrix          |  40.708 μs | 0.1420 μs | 0.1329 μs |  5.79 |    0.03 |         - |          NA |
| Determinant_OriginalTSMatrix           |  40.977 μs | 0.3797 μs | 0.3170 μs |  5.82 |    0.05 |         - |          NA |
| MatrixMultiplication_OriginalTSMatrix  |  92.069 μs | 0.4330 μs | 0.4050 μs | 13.09 |    0.07 |         - |          NA |
| MatrixMultiplication_OptimizedTSMatrix |  98.496 μs | 0.4910 μs | 0.4352 μs | 14.00 |    0.08 |         - |          NA |
| Transpose_OriginalTSMatrix             |  99.803 μs | 0.1534 μs | 0.1435 μs | 14.19 |    0.06 |         - |          NA |
| Transpose_OptimizedTSMatrix            | 103.705 μs | 0.4820 μs | 0.4273 μs | 14.74 |    0.08 |         - |          NA |
| Inverse_OriginalTSMatrix               | 303.760 μs | 2.0024 μs | 1.8730 μs | 43.18 |    0.30 |         - |          NA |
| Inverse_OptimizedTSMatrix              | 311.839 μs | 0.4891 μs | 0.4336 μs | 44.33 |    0.18 |         - |          NA |
