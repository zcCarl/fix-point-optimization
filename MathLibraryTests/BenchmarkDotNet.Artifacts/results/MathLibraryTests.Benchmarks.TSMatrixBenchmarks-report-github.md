```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.0 (24A335) [Darwin 24.0.0]
Apple M1 Max, 1 CPU, 10 logical and 10 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD


```
| Method                                 | Mean       | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------------------- |-----------:|----------:|----------:|------:|--------:|----------:|------------:|
| VectorTransform_SystemMatrix           |   1.600 μs | 0.0123 μs | 0.0109 μs |  0.22 |    0.01 |         - |          NA |
| Determinant_SystemMatrix               |   4.057 μs | 0.0337 μs | 0.0281 μs |  0.56 |    0.01 |         - |          NA |
| Transpose_SystemMatrix                 |   7.041 μs | 0.0076 μs | 0.0067 μs |  0.98 |    0.02 |         - |          NA |
| MatrixMultiplication_SystemMatrix      |   7.216 μs | 0.1375 μs | 0.1689 μs |  1.00 |    0.03 |         - |          NA |
| VectorTransform_OptimizedTSMatrix      |  10.167 μs | 0.0717 μs | 0.0599 μs |  1.41 |    0.03 |         - |          NA |
| VectorTransform_OriginalTSMatrix       |  15.453 μs | 0.0899 μs | 0.0841 μs |  2.14 |    0.05 |         - |          NA |
| Determinant_OptimizedTSMatrix          |  15.747 μs | 0.1213 μs | 0.1075 μs |  2.18 |    0.05 |         - |          NA |
| Determinant_OriginalTSMatrix           |  15.817 μs | 0.1030 μs | 0.0964 μs |  2.19 |    0.05 |         - |          NA |
| Inverse_SystemMatrix                   |  20.727 μs | 0.0692 μs | 0.0614 μs |  2.87 |    0.06 |         - |          NA |
| MatrixMultiplication_OriginalTSMatrix  |  42.136 μs | 0.2285 μs | 0.2138 μs |  5.84 |    0.13 |         - |          NA |
| Transpose_OptimizedTSMatrix            |  43.231 μs | 0.1788 μs | 0.1585 μs |  5.99 |    0.13 |         - |          NA |
| MatrixMultiplication_OptimizedTSMatrix |  44.449 μs | 0.1162 μs | 0.1087 μs |  6.16 |    0.14 |         - |          NA |
| Transpose_OriginalTSMatrix             |  46.140 μs | 0.0711 μs | 0.0665 μs |  6.40 |    0.14 |         - |          NA |
| Inverse_OptimizedTSMatrix              | 116.130 μs | 0.6280 μs | 0.5874 μs | 16.10 |    0.37 |         - |          NA |
| Inverse_OriginalTSMatrix               | 337.225 μs | 1.9638 μs | 1.7409 μs | 46.76 |    1.06 |         - |          NA |
