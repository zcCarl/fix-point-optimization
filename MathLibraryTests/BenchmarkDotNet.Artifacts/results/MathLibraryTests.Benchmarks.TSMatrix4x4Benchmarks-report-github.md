```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.0 (24A335) [Darwin 24.0.0]
Apple M1 Max, 1 CPU, 10 logical and 10 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD


```
| Method                                 | Mean       | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------------------- |-----------:|----------:|----------:|------:|--------:|----------:|------------:|
| VectorTransform_SystemMatrix           |   1.493 μs | 0.0046 μs | 0.0043 μs |  0.22 |    0.00 |         - |          NA |
| Determinant_SystemMatrix               |   3.988 μs | 0.0117 μs | 0.0109 μs |  0.58 |    0.00 |         - |          NA |
| MatrixMultiplication_SystemMatrix      |   6.903 μs | 0.0156 μs | 0.0130 μs |  1.00 |    0.00 |         - |          NA |
| Transpose_SystemMatrix                 |   6.932 μs | 0.0136 μs | 0.0121 μs |  1.00 |    0.00 |         - |          NA |
| Inverse_SystemMatrix                   |  20.426 μs | 0.0525 μs | 0.0438 μs |  2.96 |    0.01 |         - |          NA |
| VectorTransform_OriginalTSMatrix       |  24.445 μs | 0.0981 μs | 0.0766 μs |  3.54 |    0.01 |         - |          NA |
| VectorTransform_OptimizedTSMatrix      |  24.838 μs | 0.0581 μs | 0.0515 μs |  3.60 |    0.01 |         - |          NA |
| Determinant_OptimizedTSMatrix          |  40.506 μs | 0.1132 μs | 0.0946 μs |  5.87 |    0.02 |         - |          NA |
| Determinant_OriginalTSMatrix           |  40.537 μs | 0.1369 μs | 0.1143 μs |  5.87 |    0.02 |         - |          NA |
| MatrixMultiplication_OptimizedTSMatrix |  90.480 μs | 0.2807 μs | 0.2626 μs | 13.11 |    0.04 |         - |          NA |
| MatrixMultiplication_OriginalTSMatrix  |  91.032 μs | 0.7460 μs | 0.6230 μs | 13.19 |    0.09 |         - |          NA |
| Transpose_OptimizedTSMatrix            |  98.106 μs | 0.5403 μs | 0.4790 μs | 14.21 |    0.07 |         - |          NA |
| Transpose_OriginalTSMatrix             |  98.331 μs | 0.5042 μs | 0.4469 μs | 14.24 |    0.07 |         - |          NA |
| Inverse_OptimizedTSMatrix              | 297.076 μs | 0.6533 μs | 0.5455 μs | 43.04 |    0.11 |         - |          NA |
| Inverse_OriginalTSMatrix               | 297.865 μs | 0.9079 μs | 0.7581 μs | 43.15 |    0.13 |         - |          NA |
