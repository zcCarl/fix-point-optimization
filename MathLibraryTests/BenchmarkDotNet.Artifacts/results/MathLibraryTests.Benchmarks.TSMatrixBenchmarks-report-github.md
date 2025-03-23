```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.0 (24A335) [Darwin 24.0.0]
Apple M1 Max, 1 CPU, 10 logical and 10 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD


```
| Method                                 | Mean       | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------------------- |-----------:|----------:|----------:|------:|--------:|----------:|------------:|
| VectorTransform_SystemMatrix           |   1.571 μs | 0.0075 μs | 0.0059 μs |  0.23 |    0.00 |         - |          NA |
| Determinant_SystemMatrix               |   3.978 μs | 0.0147 μs | 0.0131 μs |  0.57 |    0.00 |         - |          NA |
| Transpose_SystemMatrix                 |   6.905 μs | 0.0227 μs | 0.0177 μs |  1.00 |    0.00 |         - |          NA |
| MatrixMultiplication_SystemMatrix      |   6.939 μs | 0.0106 μs | 0.0089 μs |  1.00 |    0.00 |         - |          NA |
| VectorTransform_OptimizedTSMatrix      |   9.959 μs | 0.0351 μs | 0.0329 μs |  1.44 |    0.00 |         - |          NA |
| VectorTransform_OriginalTSMatrix       |  15.145 μs | 0.0591 μs | 0.0523 μs |  2.18 |    0.01 |         - |          NA |
| Determinant_OptimizedTSMatrix          |  15.403 μs | 0.0506 μs | 0.0448 μs |  2.22 |    0.01 |         - |          NA |
| Determinant_OriginalTSMatrix           |  15.532 μs | 0.0273 μs | 0.0242 μs |  2.24 |    0.00 |         - |          NA |
| Inverse_SystemMatrix                   |  20.378 μs | 0.0585 μs | 0.0488 μs |  2.94 |    0.01 |         - |          NA |
| MatrixMultiplication_OptimizedTSMatrix |  41.297 μs | 0.0705 μs | 0.0660 μs |  5.95 |    0.01 |         - |          NA |
| MatrixMultiplication_OriginalTSMatrix  |  41.329 μs | 0.0904 μs | 0.0845 μs |  5.96 |    0.01 |         - |          NA |
| Transpose_OriginalTSMatrix             |  45.216 μs | 0.1276 μs | 0.1193 μs |  6.52 |    0.02 |         - |          NA |
| Transpose_OptimizedTSMatrix            |  45.235 μs | 0.1429 μs | 0.1267 μs |  6.52 |    0.02 |         - |          NA |
| Inverse_OptimizedTSMatrix              | 114.462 μs | 0.3652 μs | 0.3237 μs | 16.50 |    0.05 |         - |          NA |
| Inverse_OriginalTSMatrix               | 328.702 μs | 1.3705 μs | 1.2149 μs | 47.37 |    0.18 |         - |          NA |
