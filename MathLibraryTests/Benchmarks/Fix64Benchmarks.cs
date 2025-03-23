using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Numerics;

namespace MathLibraryTests.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class Fix64Benchmarks
    {
        private const int IterationCount = 10000;

        // 输入值
        private readonly double[] _doubleValues;
        private readonly float[] _floatValues;
        private readonly RVO.Arithmetic.FP[] _originalFPValues;
        private readonly RVO.Arithmetic.Optimized.FP[] _optimizedFPValues;

        public Fix64Benchmarks()
        {
            // 初始化测试数据 - 包括常规值、大值、小值和边缘情况
            _doubleValues = new double[IterationCount];
            _floatValues = new float[IterationCount];
            _originalFPValues = new RVO.Arithmetic.FP[IterationCount];
            _optimizedFPValues = new RVO.Arithmetic.Optimized.FP[IterationCount];

            var random = new Random(42); // 固定种子以确保可重复性

            for (int i = 0; i < IterationCount; i++)
            {
                // 生成-100到100范围内的随机值
                double value = (random.NextDouble() * 200) - 100;
                _doubleValues[i] = value;
                _floatValues[i] = (float)value;
                _originalFPValues[i] = (RVO.Arithmetic.FP)value;
                _optimizedFPValues[i] = (RVO.Arithmetic.Optimized.FP)value;
            }
        }

        // 加法操作
        [Benchmark(Baseline = true)]
        public double Addition_Double()
        {
            double result = 0;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _doubleValues[i];
            }
            return result;
        }

        [Benchmark]
        public float Addition_Float()
        {
            float result = 0;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _floatValues[i];
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.FP Addition_OriginalFP()
        {
            RVO.Arithmetic.FP result = RVO.Arithmetic.FP.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _originalFPValues[i];
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.FP Addition_OptimizedFP()
        {
            RVO.Arithmetic.Optimized.FP result = RVO.Arithmetic.Optimized.FP.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _optimizedFPValues[i];
            }
            return result;
        }

        // 乘法操作
        [Benchmark]
        public double Multiplication_Double()
        {
            double result = 1;
            for (int i = 0; i < IterationCount; i++)
            {
                result *= (_doubleValues[i] * 0.01 + 1); // 避免数值过大
            }
            return result;
        }

        [Benchmark]
        public float Multiplication_Float()
        {
            float result = 1;
            for (int i = 0; i < IterationCount; i++)
            {
                result *= (_floatValues[i] * 0.01f + 1);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.FP Multiplication_OriginalFP()
        {
            RVO.Arithmetic.FP result = RVO.Arithmetic.FP.One;
            RVO.Arithmetic.FP factor = (RVO.Arithmetic.FP)0.01;
            RVO.Arithmetic.FP one = RVO.Arithmetic.FP.One;

            for (int i = 0; i < IterationCount; i++)
            {
                result *= (_originalFPValues[i] * factor + one);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.FP Multiplication_OptimizedFP()
        {
            RVO.Arithmetic.Optimized.FP result = RVO.Arithmetic.Optimized.FP.One;
            RVO.Arithmetic.Optimized.FP factor = (RVO.Arithmetic.Optimized.FP)0.01;
            RVO.Arithmetic.Optimized.FP one = RVO.Arithmetic.Optimized.FP.One;

            for (int i = 0; i < IterationCount; i++)
            {
                result *= (_optimizedFPValues[i] * factor + one);
            }
            return result;
        }

        // 三角函数测试
        [Benchmark]
        public double Trigonometry_Double()
        {
            double result = 0;
            for (int i = 0; i < IterationCount; i++)
            {
                double value = _doubleValues[i] % (2 * Math.PI);
                result += Math.Sin(value) + Math.Cos(value);
            }
            return result;
        }

        [Benchmark]
        public float Trigonometry_Float()
        {
            float result = 0;
            for (int i = 0; i < IterationCount; i++)
            {
                float value = _floatValues[i] % ((float)(2 * Math.PI));
                result += MathF.Sin(value) + MathF.Cos(value);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.FP Trigonometry_OriginalFP()
        {
            RVO.Arithmetic.FP result = RVO.Arithmetic.FP.Zero;
            RVO.Arithmetic.FP twoPi = RVO.Arithmetic.FP.Pi + RVO.Arithmetic.FP.Pi;

            for (int i = 0; i < IterationCount; i++)
            {
                RVO.Arithmetic.FP value = _originalFPValues[i] % twoPi;
                result += RVO.Arithmetic.FP.Sin(value) + RVO.Arithmetic.FP.Cos(value);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.FP Trigonometry_OptimizedFP()
        {
            RVO.Arithmetic.Optimized.FP result = RVO.Arithmetic.Optimized.FP.Zero;
            RVO.Arithmetic.Optimized.FP twoPi = RVO.Arithmetic.Optimized.FP.Pi + RVO.Arithmetic.Optimized.FP.Pi;

            for (int i = 0; i < IterationCount; i++)
            {
                RVO.Arithmetic.Optimized.FP value = _optimizedFPValues[i] % twoPi;
                result += RVO.Arithmetic.Optimized.FP.Sin(value) + RVO.Arithmetic.Optimized.FP.Cos(value);
            }
            return result;
        }

        // 平方根测试
        [Benchmark]
        public double Sqrt_Double()
        {
            double result = 0;
            for (int i = 0; i < IterationCount; i++)
            {
                double value = Math.Abs(_doubleValues[i]); // 确保是正值
                result += Math.Sqrt(value);
            }
            return result;
        }

        [Benchmark]
        public float Sqrt_Float()
        {
            float result = 0;
            for (int i = 0; i < IterationCount; i++)
            {
                float value = Math.Abs(_floatValues[i]);
                result += MathF.Sqrt(value);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.FP Sqrt_OriginalFP()
        {
            RVO.Arithmetic.FP result = RVO.Arithmetic.FP.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                RVO.Arithmetic.FP value = RVO.Arithmetic.FP.Abs(_originalFPValues[i]);
                result += RVO.Arithmetic.FP.Sqrt(value);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.FP Sqrt_OptimizedFP()
        {
            RVO.Arithmetic.Optimized.FP result = RVO.Arithmetic.Optimized.FP.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                RVO.Arithmetic.Optimized.FP value = RVO.Arithmetic.Optimized.FP.Abs(_optimizedFPValues[i]);
                result += RVO.Arithmetic.Optimized.FP.Sqrt(value);
            }
            return result;
        }
    }
}