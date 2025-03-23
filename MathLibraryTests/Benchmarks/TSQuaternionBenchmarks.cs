using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Numerics;

namespace MathLibraryTests.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class TSQuaternionBenchmarks
    {
        private const int IterationCount = 1000;

        // 输入值
        private readonly Quaternion[] _systemQuaternions;
        private readonly RVO.Arithmetic.TSQuaternion[] _originalQuaternions;
        private readonly RVO.Arithmetic.Optimized.TSQuaternion[] _optimizedQuaternions;

        // 向量，用于旋转测试
        private readonly Vector3[] _systemVectors;
        private readonly RVO.Arithmetic.TSVector[] _originalVectors;
        private readonly RVO.Arithmetic.Optimized.TSVector[] _optimizedVectors;

        public TSQuaternionBenchmarks()
        {
            // 初始化测试数据
            _systemQuaternions = new Quaternion[IterationCount];
            _originalQuaternions = new RVO.Arithmetic.TSQuaternion[IterationCount];
            _optimizedQuaternions = new RVO.Arithmetic.Optimized.TSQuaternion[IterationCount];

            _systemVectors = new Vector3[IterationCount];
            _originalVectors = new RVO.Arithmetic.TSVector[IterationCount];
            _optimizedVectors = new RVO.Arithmetic.Optimized.TSVector[IterationCount];

            var random = new Random(42); // 固定种子以确保可重复性

            for (int i = 0; i < IterationCount; i++)
            {
                // 随机角度（0到2π）
                float angle = (float)(random.NextDouble() * 2 * Math.PI);

                // 随机单位向量作为旋转轴
                float x = (float)(random.NextDouble() * 2 - 1);
                float y = (float)(random.NextDouble() * 2 - 1);
                float z = (float)(random.NextDouble() * 2 - 1);
                float magnitude = MathF.Sqrt(x * x + y * y + z * z);

                // 规范化轴向量
                if (magnitude > float.Epsilon)
                {
                    x /= magnitude;
                    y /= magnitude;
                    z /= magnitude;
                }
                else
                {
                    x = 0;
                    y = 0;
                    z = 1;
                }

                // 构造四元数
                _systemQuaternions[i] = Quaternion.CreateFromAxisAngle(new Vector3(x, y, z), angle);

                _originalQuaternions[i] = RVO.Arithmetic.TSQuaternion.AngleAxis(
                    (RVO.Arithmetic.FP)angle,
                    new RVO.Arithmetic.TSVector(
                        (RVO.Arithmetic.FP)x,
                        (RVO.Arithmetic.FP)y,
                        (RVO.Arithmetic.FP)z
                    )
                );

                _optimizedQuaternions[i] = RVO.Arithmetic.Optimized.TSQuaternion.AngleAxis(
                    (RVO.Arithmetic.Optimized.FP)angle,
                    new RVO.Arithmetic.Optimized.TSVector(
                        (RVO.Arithmetic.Optimized.FP)x,
                        (RVO.Arithmetic.Optimized.FP)y,
                        (RVO.Arithmetic.Optimized.FP)z
                    )
                );

                // 生成随机向量用于旋转测试
                x = (float)(random.NextDouble() * 200 - 100);
                y = (float)(random.NextDouble() * 200 - 100);
                z = (float)(random.NextDouble() * 200 - 100);

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

        // 四元数乘法
        [Benchmark(Baseline = true)]
        public Quaternion Multiplication_SystemQuaternion()
        {
            Quaternion result = Quaternion.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = Quaternion.Multiply(result, _systemQuaternions[i]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSQuaternion Multiplication_OriginalTSQuaternion()
        {
            RVO.Arithmetic.TSQuaternion result = RVO.Arithmetic.TSQuaternion.identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * _originalQuaternions[i];
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSQuaternion Multiplication_OptimizedTSQuaternion()
        {
            RVO.Arithmetic.Optimized.TSQuaternion result = RVO.Arithmetic.Optimized.TSQuaternion.identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * _optimizedQuaternions[i];
            }
            return result;
        }

        // 四元数共轭
        [Benchmark]
        public Quaternion Conjugate_SystemQuaternion()
        {
            Quaternion result = Quaternion.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = Quaternion.Multiply(result, Quaternion.Conjugate(_systemQuaternions[i]));
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSQuaternion Conjugate_OriginalTSQuaternion()
        {
            RVO.Arithmetic.TSQuaternion result = RVO.Arithmetic.TSQuaternion.identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * RVO.Arithmetic.TSQuaternion.Conjugate(_originalQuaternions[i]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSQuaternion Conjugate_OptimizedTSQuaternion()
        {
            RVO.Arithmetic.Optimized.TSQuaternion result = RVO.Arithmetic.Optimized.TSQuaternion.identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * RVO.Arithmetic.Optimized.TSQuaternion.Conjugate(_optimizedQuaternions[i]);
            }
            return result;
        }

        // 四元数逆
        [Benchmark]
        public Quaternion Inverse_SystemQuaternion()
        {
            Quaternion result = Quaternion.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = Quaternion.Multiply(result, Quaternion.Inverse(_systemQuaternions[i]));
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSQuaternion Inverse_OriginalTSQuaternion()
        {
            RVO.Arithmetic.TSQuaternion result = RVO.Arithmetic.TSQuaternion.identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * RVO.Arithmetic.TSQuaternion.Inverse(_originalQuaternions[i]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSQuaternion Inverse_OptimizedTSQuaternion()
        {
            RVO.Arithmetic.Optimized.TSQuaternion result = RVO.Arithmetic.Optimized.TSQuaternion.identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * RVO.Arithmetic.Optimized.TSQuaternion.Inverse(_optimizedQuaternions[i]);
            }
            return result;
        }

        // 向量旋转
        [Benchmark]
        public Vector3 RotateVector_SystemQuaternion()
        {
            Vector3 result = Vector3.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += Vector3.Transform(_systemVectors[i], _systemQuaternions[i]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSVector RotateVector_OriginalTSQuaternion()
        {
            RVO.Arithmetic.TSVector result = RVO.Arithmetic.TSVector.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _originalQuaternions[i] * _originalVectors[i];
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSVector RotateVector_OptimizedTSQuaternion()
        {
            RVO.Arithmetic.Optimized.TSVector result = RVO.Arithmetic.Optimized.TSVector.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _optimizedQuaternions[i] * _optimizedVectors[i];
            }
            return result;
        }

        // 四元数SLERP
        [Benchmark]
        public Quaternion Slerp_SystemQuaternion()
        {
            Quaternion result = Quaternion.Identity;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                float t = i / (float)IterationCount;
                result = Quaternion.Multiply(result, Quaternion.Slerp(_systemQuaternions[i], _systemQuaternions[i + 1], t));
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSQuaternion Slerp_OriginalTSQuaternion()
        {
            RVO.Arithmetic.TSQuaternion result = RVO.Arithmetic.TSQuaternion.identity;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                RVO.Arithmetic.FP t = (RVO.Arithmetic.FP)i / (RVO.Arithmetic.FP)IterationCount;
                result = result * RVO.Arithmetic.TSQuaternion.Slerp(_originalQuaternions[i], _originalQuaternions[i + 1], t);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSQuaternion Slerp_OptimizedTSQuaternion()
        {
            RVO.Arithmetic.Optimized.TSQuaternion result = RVO.Arithmetic.Optimized.TSQuaternion.identity;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                RVO.Arithmetic.Optimized.FP t = (RVO.Arithmetic.Optimized.FP)i / (RVO.Arithmetic.Optimized.FP)IterationCount;
                result = result * RVO.Arithmetic.Optimized.TSQuaternion.Slerp(_optimizedQuaternions[i], _optimizedQuaternions[i + 1], t);
            }
            return result;
        }

        // 四元数归一化
        [Benchmark]
        public Quaternion Normalize_SystemQuaternion()
        {
            Quaternion result = Quaternion.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = Quaternion.Multiply(result, _systemQuaternions[i]);
                result = Quaternion.Normalize(result);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSQuaternion Normalize_OriginalTSQuaternion()
        {
            RVO.Arithmetic.TSQuaternion result = RVO.Arithmetic.TSQuaternion.identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * _originalQuaternions[i];
                result.Normalize();
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSQuaternion Normalize_OptimizedTSQuaternion()
        {
            RVO.Arithmetic.Optimized.TSQuaternion result = RVO.Arithmetic.Optimized.TSQuaternion.identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * _optimizedQuaternions[i];
                result.Normalize();
            }
            return result;
        }
    }
}