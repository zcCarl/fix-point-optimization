```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.0 (24A335) [Darwin 24.0.0]
Apple M1 Max, 1 CPU, 10 logical and 10 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD


```
| Method                        | Mean         | Error      | StdDev     | Ratio  | RatioSD | Allocated | Alloc Ratio |
|------------------------------ |-------------:|-----------:|-----------:|-------:|--------:|----------:|------------:|
| Addition_OptimizedTSVector2   |     6.093 μs |  0.0145 μs |  0.0135 μs |   0.64 |    0.00 |         - |          NA |
| Addition_OriginalTSVector2    |     6.121 μs |  0.0440 μs |  0.0344 μs |   0.64 |    0.00 |         - |          NA |
| Addition_SystemVector2        |     9.589 μs |  0.0283 μs |  0.0251 μs |   1.00 |    0.00 |         - |          NA |
| DotProduct_SystemVector2      |     9.621 μs |  0.0401 μs |  0.0355 μs |   1.00 |    0.00 |         - |          NA |
| Distance_SystemVector2        |     9.626 μs |  0.0319 μs |  0.0283 μs |   1.00 |    0.00 |         - |          NA |
| Magnitude_SystemVector2       |     9.627 μs |  0.0228 μs |  0.0202 μs |   1.00 |    0.00 |         - |          NA |
| Normalize_SystemVector2       |    12.822 μs |  0.0330 μs |  0.0292 μs |   1.34 |    0.00 |         - |          NA |
| DotProduct_OptimizedTSVector2 |    27.188 μs |  0.0672 μs |  0.0596 μs |   2.84 |    0.01 |         - |          NA |
| DotProduct_OriginalTSVector2  |    27.304 μs |  0.0648 μs |  0.0575 μs |   2.85 |    0.01 |         - |          NA |
| Angle_SystemVector2           |   141.285 μs |  2.2956 μs |  2.0350 μs |  14.73 |    0.21 |         - |          NA |
| Magnitude_OriginalTSVector2   | 1,418.986 μs |  7.1229 μs |  5.9480 μs | 147.99 |    0.70 |       1 B |          NA |
| Magnitude_OptimizedTSVector2  | 1,419.153 μs |  4.8336 μs |  4.5213 μs | 148.00 |    0.59 |       1 B |          NA |
| Distance_OptimizedTSVector2   | 1,434.505 μs |  5.6718 μs |  4.7362 μs | 149.60 |    0.61 |       1 B |          NA |
| Distance_OriginalTSVector2    | 1,447.311 μs |  9.2315 μs |  8.1834 μs | 150.94 |    0.91 |       1 B |          NA |
| Normalize_OptimizedTSVector2  | 1,605.772 μs |  3.7248 μs |  3.1104 μs | 167.47 |    0.53 |       1 B |          NA |
| Normalize_OriginalTSVector2   | 1,608.441 μs |  6.6163 μs |  5.5249 μs | 167.74 |    0.70 |       1 B |          NA |
| Angle_OriginalTSVector2       | 7,000.152 μs | 24.0395 μs | 22.4865 μs | 730.05 |    2.92 |       6 B |          NA |
| Angle_OptimizedTSVector2      | 7,081.301 μs | 17.4216 μs | 14.5478 μs | 738.51 |    2.37 |       6 B |          NA |
