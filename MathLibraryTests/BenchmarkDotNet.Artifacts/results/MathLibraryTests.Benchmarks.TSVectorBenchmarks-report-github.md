```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.0 (24A335) [Darwin 24.0.0]
Apple M1 Max, 1 CPU, 10 logical and 10 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD


```
| Method                         | Mean         | Error      | StdDev     | Ratio  | RatioSD | Allocated | Alloc Ratio |
|------------------------------- |-------------:|-----------:|-----------:|-------:|--------:|----------:|------------:|
| Addition_OriginalTSVector      |     7.246 μs |  0.0209 μs |  0.0195 μs |   0.76 |    0.00 |         - |          NA |
| Magnitude_OptimizedTSVector    |     8.518 μs |  0.0318 μs |  0.0248 μs |   0.89 |    0.00 |         - |          NA |
| Addition_OptimizedTSVector     |     8.971 μs |  0.1287 μs |  0.1141 μs |   0.93 |    0.01 |         - |          NA |
| Addition_SystemVector3         |     9.596 μs |  0.0196 μs |  0.0163 μs |   1.00 |    0.00 |         - |          NA |
| Magnitude_SystemVector3        |    10.060 μs |  0.0320 μs |  0.0250 μs |   1.05 |    0.00 |         - |          NA |
| DotProduct_SystemVector3       |    14.122 μs |  0.0931 μs |  0.0777 μs |   1.47 |    0.01 |         - |          NA |
| Normalize_SystemVector3        |    14.706 μs |  0.0501 μs |  0.0444 μs |   1.53 |    0.01 |         - |          NA |
| Distance_SystemVector3         |    14.897 μs |  0.0350 μs |  0.0292 μs |   1.55 |    0.00 |         - |          NA |
| CrossProduct_SystemVector3     |    19.331 μs |  0.0823 μs |  0.0687 μs |   2.01 |    0.01 |         - |          NA |
| DotProduct_OriginalTSVector    |    37.751 μs |  0.0717 μs |  0.0671 μs |   3.93 |    0.01 |         - |          NA |
| DotProduct_OptimizedTSVector   |    37.876 μs |  0.1298 μs |  0.1084 μs |   3.95 |    0.01 |         - |          NA |
| CrossProduct_OptimizedTSVector |    60.993 μs |  0.1358 μs |  0.1204 μs |   6.36 |    0.02 |         - |          NA |
| CrossProduct_OriginalTSVector  |    78.053 μs |  0.2226 μs |  0.1858 μs |   8.13 |    0.02 |         - |          NA |
| Magnitude_OriginalTSVector     | 1,416.142 μs |  8.8877 μs |  7.4216 μs | 147.58 |    0.78 |       1 B |          NA |
| Distance_OriginalTSVector      | 1,442.375 μs |  6.0260 μs |  5.3419 μs | 150.31 |    0.59 |       1 B |          NA |
| Distance_OptimizedTSVector     | 1,466.869 μs |  3.8084 μs |  3.3761 μs | 152.87 |    0.42 |       1 B |          NA |
| Normalize_OptimizedTSVector    | 1,640.634 μs |  7.4540 μs |  6.2244 μs | 170.98 |    0.69 |       1 B |          NA |
| Normalize_OriginalTSVector     | 1,654.490 μs | 18.9363 μs | 16.7865 μs | 172.42 |    1.71 |       1 B |          NA |
