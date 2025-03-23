using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Numerics;

namespace MathLibraryTests.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class TSVector2Benchmarks
    {
        private const int IterationCount = 10000;

        // 输入值
        private readonly Vector2[] _systemVectors;
        private readonly RVO.Arithmetic.TSVector2[] _originalVectors;
        private readonly RVO.Arithmetic.Optimized.TSVector2[] _optimizedVectors;

        public TSVector2Benchmarks()
        {
            // 初始化测试数据
            _systemVectors = new Vector2[IterationCount];
            _originalVectors = new RVO.Arithmetic.TSVector2[IterationCount];
            _optimizedVectors = new RVO.Arithmetic.Optimized.TSVector2[IterationCount];

            var random = new Random(42); // 固定种子以确保可重复性

            for (int i = 0; i < IterationCount; i++)
            {
                // 生成-100到100范围内的随机值
                float x = (float)((random.NextDouble() * 200) - 100);
                float y = (float)((random.NextDouble() * 200) - 100);

                _systemVectors[i] = new Vector2(x, y);
                _originalVectors[i] = new RVO.Arithmetic.TSVector2(
                    (RVO.Arithmetic.FP)x,
                    (RVO.Arithmetic.FP)y
                );
                _optimizedVectors[i] = new RVO.Arithmetic.Optimized.TSVector2(
                    (RVO.Arithmetic.Optimized.FP)x,
                    (RVO.Arithmetic.Optimized.FP)y
                );
            }
        }

        // 向量加法
        [Benchmark(Baseline = true)]
        public Vector2 Addition_SystemVector2()
        {
            Vector2 result = Vector2.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _systemVectors[i];
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSVector2 Addition_OriginalTSVector2()
        {
            RVO.Arithmetic.TSVector2 result = RVO.Arithmetic.TSVector2.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _originalVectors[i];
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSVector2 Addition_OptimizedTSVector2()
        {
            RVO.Arithmetic.Optimized.TSVector2 result = RVO.Arithmetic.Optimized.TSVector2.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _optimizedVectors[i];
            }
            return result;
        }

        // 向量点乘
        [Benchmark]
        public float DotProduct_SystemVector2()
        {
            float result = 0;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += Vector2.Dot(_systemVectors[i], _systemVectors[i + 1]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.FP DotProduct_OriginalTSVector2()
        {
            RVO.Arithmetic.FP result = RVO.Arithmetic.FP.Zero;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += RVO.Arithmetic.TSVector2.Dot(_originalVectors[i], _originalVectors[i + 1]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.FP DotProduct_OptimizedTSVector2()
        {
            RVO.Arithmetic.Optimized.FP result = RVO.Arithmetic.Optimized.FP.Zero;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += RVO.Arithmetic.Optimized.TSVector2.Dot(_optimizedVectors[i], _optimizedVectors[i + 1]);
            }
            return result;
        }

        // 向量长度
        [Benchmark]
        public float Magnitude_SystemVector2()
        {
            float result = 0;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _systemVectors[i].Length();
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.FP Magnitude_OriginalTSVector2()
        {
            RVO.Arithmetic.FP result = RVO.Arithmetic.FP.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _originalVectors[i].magnitude;
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.FP Magnitude_OptimizedTSVector2()
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
        public Vector2 Normalize_SystemVector2()
        {
            Vector2 result = Vector2.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                if (_systemVectors[i].Length() > 1e-6f)
                {
                    result += Vector2.Normalize(_systemVectors[i]);
                }
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSVector2 Normalize_OriginalTSVector2()
        {
            RVO.Arithmetic.TSVector2 result = RVO.Arithmetic.TSVector2.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                if (_originalVectors[i].LengthSquared() > RVO.Arithmetic.FP.Epsilon)
                {
                    result += _originalVectors[i].normalized;
                }
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSVector2 Normalize_OptimizedTSVector2()
        {
            RVO.Arithmetic.Optimized.TSVector2 result = RVO.Arithmetic.Optimized.TSVector2.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                if (_optimizedVectors[i].LengthSquared() > RVO.Arithmetic.Optimized.FP.Epsilon)
                {
                    result += _optimizedVectors[i].normalized;
                }
            }
            return result;
        }

        // 距离计算
        [Benchmark]
        public float Distance_SystemVector2()
        {
            float result = 0;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += Vector2.Distance(_systemVectors[i], _systemVectors[i + 1]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.FP Distance_OriginalTSVector2()
        {
            RVO.Arithmetic.FP result = RVO.Arithmetic.FP.Zero;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += RVO.Arithmetic.TSVector2.Distance(_originalVectors[i], _originalVectors[i + 1]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.FP Distance_OptimizedTSVector2()
        {
            RVO.Arithmetic.Optimized.FP result = RVO.Arithmetic.Optimized.FP.Zero;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += RVO.Arithmetic.Optimized.TSVector2.Distance(_optimizedVectors[i], _optimizedVectors[i + 1]);
            }
            return result;
        }

        // 向量角度
        [Benchmark]
        public float Angle_SystemVector2()
        {
            float result = 0;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                // 计算两个向量之间的夹角（弧度）
                float dot = Vector2.Dot(Vector2.Normalize(_systemVectors[i]), Vector2.Normalize(_systemVectors[i + 1]));
                result += MathF.Acos(Math.Clamp(dot, -1.0f, 1.0f));
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.FP Angle_OriginalTSVector2()
        {
            RVO.Arithmetic.FP result = RVO.Arithmetic.FP.Zero;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += RVO.Arithmetic.TSVector2.Angle(_originalVectors[i], _originalVectors[i + 1]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.FP Angle_OptimizedTSVector2()
        {
            RVO.Arithmetic.Optimized.FP result = RVO.Arithmetic.Optimized.FP.Zero;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result += RVO.Arithmetic.Optimized.TSVector2.Angle(_optimizedVectors[i], _optimizedVectors[i + 1]);
            }
            return result;
        }
    }
}