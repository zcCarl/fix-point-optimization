```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.0 (24A335) [Darwin 24.0.0]
Apple M1 Max, 1 CPU, 10 logical and 10 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD


```
| Method                               | Mean       | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------------------------- |-----------:|----------:|----------:|------:|--------:|----------:|------------:|
| RotateVector_SystemQuaternion        |   4.484 μs | 0.0083 μs | 0.0069 μs |  0.44 |    0.00 |         - |          NA |
| Inverse_SystemQuaternion             |   8.202 μs | 0.0201 μs | 0.0178 μs |  0.80 |    0.00 |         - |          NA |
| Multiplication_SystemQuaternion      |  10.246 μs | 0.0252 μs | 0.0236 μs |  1.00 |    0.00 |         - |          NA |
| Conjugate_SystemQuaternion           |  10.260 μs | 0.0279 μs | 0.0247 μs |  1.00 |    0.00 |         - |          NA |
| Normalize_SystemQuaternion           |  18.376 μs | 0.0240 μs | 0.0212 μs |  1.79 |    0.00 |         - |          NA |
| Slerp_SystemQuaternion               |  22.355 μs | 0.2349 μs | 0.2083 μs |  2.18 |    0.02 |         - |          NA |
| Multiplication_OriginalTSQuaternion  |  23.025 μs | 0.1007 μs | 0.0786 μs |  2.25 |    0.01 |         - |          NA |
| Multiplication_OptimizedTSQuaternion |  24.165 μs | 0.0277 μs | 0.0216 μs |  2.36 |    0.01 |         - |          NA |
| Conjugate_OriginalTSQuaternion       |  24.502 μs | 0.0887 μs | 0.0692 μs |  2.39 |    0.01 |         - |          NA |
| Conjugate_OptimizedTSQuaternion      |  24.790 μs | 0.0632 μs | 0.0591 μs |  2.42 |    0.01 |         - |          NA |
| RotateVector_OriginalTSQuaternion    |  34.425 μs | 0.0780 μs | 0.0692 μs |  3.36 |    0.01 |         - |          NA |
| RotateVector_OptimizedTSQuaternion   |  35.225 μs | 0.0736 μs | 0.0652 μs |  3.44 |    0.01 |         - |          NA |
| Inverse_OriginalTSQuaternion         |  53.987 μs | 0.1446 μs | 0.1129 μs |  5.27 |    0.02 |         - |          NA |
| Inverse_OptimizedTSQuaternion        |  54.477 μs | 0.1485 μs | 0.1389 μs |  5.32 |    0.02 |         - |          NA |
| Normalize_OriginalTSQuaternion       |  74.854 μs | 0.2194 μs | 0.2052 μs |  7.31 |    0.03 |         - |          NA |
| Normalize_OptimizedTSQuaternion      | 110.413 μs | 0.6963 μs | 0.6172 μs | 10.78 |    0.06 |         - |          NA |
| Slerp_OptimizedTSQuaternion          | 262.345 μs | 1.2235 μs | 0.9552 μs | 25.61 |    0.11 |         - |          NA |
| Slerp_OriginalTSQuaternion           | 271.313 μs | 1.2386 μs | 1.1586 μs | 26.48 |    0.12 |         - |          NA |
