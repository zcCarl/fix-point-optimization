```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.0 (24A335) [Darwin 24.0.0]
Apple M1 Max, 1 CPU, 10 logical and 10 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD


```
| Method                     | Mean         | Error     | StdDev    | Ratio  | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |-------------:|----------:|----------:|-------:|--------:|----------:|------------:|
| Addition_OptimizedFP       |     4.809 μs | 0.0117 μs | 0.0110 μs |   0.50 |    0.00 |         - |          NA |
| Addition_OriginalFP        |     4.822 μs | 0.0097 μs | 0.0086 μs |   0.50 |    0.00 |         - |          NA |
| Addition_Float             |     9.561 μs | 0.0172 μs | 0.0161 μs |   1.00 |    0.00 |         - |          NA |
| Addition_Double            |     9.569 μs | 0.0169 μs | 0.0150 μs |   1.00 |    0.00 |         - |          NA |
| Sqrt_Double                |     9.640 μs | 0.0244 μs | 0.0228 μs |   1.01 |    0.00 |         - |          NA |
| Sqrt_Float                 |     9.791 μs | 0.0319 μs | 0.0299 μs |   1.02 |    0.00 |         - |          NA |
| Multiplication_Double      |    12.767 μs | 0.0235 μs | 0.0209 μs |   1.33 |    0.00 |         - |          NA |
| Multiplication_Float       |    12.770 μs | 0.0384 μs | 0.0340 μs |   1.33 |    0.00 |         - |          NA |
| Multiplication_OriginalFP  |    40.375 μs | 0.1209 μs | 0.1010 μs |   4.22 |    0.01 |         - |          NA |
| Multiplication_OptimizedFP |    40.385 μs | 0.1480 μs | 0.1236 μs |   4.22 |    0.01 |         - |          NA |
| Trigonometry_Float         |   306.116 μs | 2.0684 μs | 1.9347 μs |  31.99 |    0.20 |         - |          NA |
| Trigonometry_Double        |   332.176 μs | 1.2874 μs | 1.1412 μs |  34.72 |    0.13 |         - |          NA |
| Trigonometry_OptimizedFP   |   335.014 μs | 5.4745 μs | 4.8530 μs |  35.01 |    0.49 |         - |          NA |
| Trigonometry_OriginalFP    |   340.773 μs | 5.6883 μs | 5.0425 μs |  35.61 |    0.51 |         - |          NA |
| Sqrt_OptimizedFP           | 1,296.917 μs | 2.4449 μs | 2.1673 μs | 135.54 |    0.30 |       1 B |          NA |
| Sqrt_OriginalFP            | 1,298.996 μs | 3.5457 μs | 2.9608 μs | 135.76 |    0.36 |       1 B |          NA |
