using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Numerics;

namespace MathLibraryTests.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class TSVectorBenchmarks
    {
        private const int IterationCount = 10000;

        // 输入值
        private readonly Vector3[] _systemVectors;
        private readonly RVO.Arithmetic.TSVector[] _originalVectors;
        private readonly RVO.Arithmetic.Optimized.TSVector[] _optimizedVectors;

        public TSVectorBenchmarks()
        {
            // 初始化测试数据
            _systemVectors = new Vector3[IterationCount];
            _originalVectors = new RVO.Arithmetic.TSVector[IterationCount];
            _optimizedVectors = new RVO.Arithmetic.Optimized.TSVector[IterationCount];

            var random = new Random(42); // 固定种子以确保可重复性

            for (int i = 0; i < IterationCount; i++)
            {
                // 生成-100到100范围内的随机值
                float x = (float)((random.NextDouble() * 200) - 100);
                float y = (float)((random.NextDouble() * 200) - 100);
                float z = (float)((random.NextDouble() * 200) - 100);

                _systemVectors[i] = new Vector3(x, y, z);
                _originalVectors[i] = new RVO.Arithmetic.TSVector(
                    (RVO.Arithmetic.FP)x,
                    (RVO.Arithmetic.FP)y,
                    (RVO.Arithmetic.FP)z
                );
                _optimizedVectors[i] = new RVO.Arithmetic.Optimized.TSVector(
                    (RVO.Arithmetic.Optimized.FP)x,
                    (RVO.Arithmetic.Optimized.FP)y,
                    (RVO.Arithmetic.Optimized.FP)z
                );
            }
        }

        // 向量加法
        [Benchmark(Baseline = true)]
        public Vector3 Addition_SystemVector3()
        {
            Vector3 result = Vector3.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _systemVectors[i];
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSVector Addition_OriginalTSVector()
        {
            RVO.Arithmetic.TSVector result = RVO.Arithmetic.TSVector.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _originalVectors[i];
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSVector Addition_OptimizedTSVector()
        {
            RVO.Arithmetic.Optimized.TSVector result = RVO.Arithmetic.Optimized.TSVector.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _optimizedVectors[i];
            }
            return result;
        }

        // 向量点乘
        [Benchmark]
        public float DotProduct_SystemVector3()
        {
            float result = 0;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += Vector3.Dot(_systemVectors[i], _systemVectors[i + 1]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.FP DotProduct_OriginalTSVector()
        {
            RVO.Arithmetic.FP result = RVO.Arithmetic.FP.Zero;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += RVO.Arithmetic.TSVector.Dot(_originalVectors[i], _originalVectors[i + 1]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.FP DotProduct_OptimizedTSVector()
        {
            RVO.Arithmetic.Optimized.FP result = RVO.Arithmetic.Optimized.FP.Zero;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += RVO.Arithmetic.Optimized.TSVector.Dot(_optimizedVectors[i], _optimizedVectors[i + 1]);
            }
            return result;
        }

        // 向量叉乘
        [Benchmark]
        public Vector3 CrossProduct_SystemVector3()
        {
            Vector3 result = Vector3.Zero;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += Vector3.Cross(_systemVectors[i], _systemVectors[i + 1]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSVector CrossProduct_OriginalTSVector()
        {
            RVO.Arithmetic.TSVector result = RVO.Arithmetic.TSVector.zero;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += RVO.Arithmetic.TSVector.Cross(_originalVectors[i], _originalVectors[i + 1]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSVector CrossProduct_OptimizedTSVector()
        {
            RVO.Arithmetic.Optimized.TSVector result = RVO.Arithmetic.Optimized.TSVector.zero;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += RVO.Arithmetic.Optimized.TSVector.Cross(_optimizedVectors[i], _optimizedVectors[i + 1]);
            }
            return result;
        }

        // 向量长度
        [Benchmark]
        public float Magnitude_SystemVector3()
        {
            float result = 0;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _systemVectors[i].Length();
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.FP Magnitude_OriginalTSVector()
        {
            RVO.Arithmetic.FP result = RVO.Arithmetic.FP.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _originalVectors[i].magnitude;
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.FP Magnitude_OptimizedTSVector()
        {
            RVO.Arithmetic.Optimized.FP result = RVO.Arithmetic.Optimized.FP.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _optimizedVectors[i].magnitude;
            }
            return result;
        }

        // 向量归一化
        [Benchmark]
        public Vector3 Normalize_SystemVector3()
        {
            Vector3 result = Vector3.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                if (_systemVectors[i].Length() > 1e-6f)
                {
                    result += Vector3.Normalize(_systemVectors[i]);
                }
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSVector Normalize_OriginalTSVector()
        {
            RVO.Arithmetic.TSVector result = RVO.Arithmetic.TSVector.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                if (_originalVectors[i].sqrMagnitude > RVO.Arithmetic.FP.Epsilon)
                {
                    result += _originalVectors[i].normalized;
                }
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSVector Normalize_OptimizedTSVector()
        {
            RVO.Arithmetic.Optimized.TSVector result = RVO.Arithmetic.Optimized.TSVector.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                if (_optimizedVectors[i].sqrMagnitude > RVO.Arithmetic.Optimized.FP.Epsilon)
                {
                    result += _optimizedVectors[i].normalized;
                }
            }
            return result;
        }

        // 距离计算
        [Benchmark]
        public float Distance_SystemVector3()
        {
            float result = 0;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += Vector3.Distance(_systemVectors[i], _systemVectors[i + 1]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.FP Distance_OriginalTSVector()
        {
            RVO.Arithmetic.FP result = RVO.Arithmetic.FP.Zero;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += RVO.Arithmetic.TSVector.Distance(_originalVectors[i], _originalVectors[i + 1]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.FP Distance_OptimizedTSVector()
        {
            RVO.Arithmetic.Optimized.FP result = RVO.Arithmetic.Optimized.FP.Zero;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                // 计算两点之间的距离
                RVO.Arithmetic.Optimized.TSVector diff = _optimizedVectors[i] - _optimizedVectors[i + 1];
                result += diff.magnitude;
            }
            return result;
        }
    }
}