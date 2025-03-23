using System;
using Xunit;
using Xunit.Abstractions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using OriginalFP = RVO.Arithmetic.FP;
using OptimizedFP = RVO.Arithmetic.Optimized.FP;

namespace MathLibraryTests
{
    public class FixedPointTests
    {
        private readonly ITestOutputHelper _output;

        public FixedPointTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CorrectnessTest_Basic_Operations()
        {
            // 基本算术运算测试
            OriginalFP originalA = new OriginalFP(5);
            OriginalFP originalB = new OriginalFP(3);
            OptimizedFP optimizedA = new OptimizedFP(5);
            OptimizedFP optimizedB = new OptimizedFP(3);

            // 加法
            OriginalFP originalAdd = originalA + originalB;
            OptimizedFP optimizedAdd = optimizedA + optimizedB;
            Assert.Equal(originalAdd._serializedValue, optimizedAdd._serializedValue);
            _output.WriteLine($"加法: 原始库 = {originalAdd}, 优化库 = {optimizedAdd}");

            // 减法
            OriginalFP originalSub = originalA - originalB;
            OptimizedFP optimizedSub = optimizedA - optimizedB;
            Assert.Equal(originalSub._serializedValue, optimizedSub._serializedValue);
            _output.WriteLine($"减法: 原始库 = {originalSub}, 优化库 = {optimizedSub}");

            // 乘法
            OriginalFP originalMul = originalA * originalB;
            OptimizedFP optimizedMul = optimizedA * optimizedB;
            Assert.Equal(originalMul._serializedValue, optimizedMul._serializedValue);
            _output.WriteLine($"乘法: 原始库 = {originalMul}, 优化库 = {optimizedMul}");

            // 除法
            OriginalFP originalDiv = originalA / originalB;
            OptimizedFP optimizedDiv = optimizedA / optimizedB;
            Assert.Equal(originalDiv._serializedValue, optimizedDiv._serializedValue);
            _output.WriteLine($"除法: 原始库 = {originalDiv}, 优化库 = {optimizedDiv}");
        }

        [Fact]
        public void CorrectnessTest_AdvancedMath()
        {
            // 高级数学函数测试
            OriginalFP originalA = OriginalFP.Pi / new OriginalFP(4); // π/4
            OptimizedFP optimizedA = OptimizedFP.Pi / new OptimizedFP(4); // π/4

            // 正弦函数
            OriginalFP originalSin = OriginalFP.Sin(originalA);
            OptimizedFP optimizedSin = OptimizedFP.Sin(optimizedA);
            _output.WriteLine($"Sin(π/4): 原始库 = {originalSin}, 优化库 = {optimizedSin}");
            // 允许微小差异
            Assert.True(Math.Abs((double)(originalSin._serializedValue - optimizedSin._serializedValue)) / (double)originalSin._serializedValue < 1e-10);

            // 余弦函数
            OriginalFP originalCos = OriginalFP.Cos(originalA);
            OptimizedFP optimizedCos = OptimizedFP.Cos(optimizedA);
            _output.WriteLine($"Cos(π/4): 原始库 = {originalCos}, 优化库 = {optimizedCos}");
            Assert.True(Math.Abs((double)(originalCos._serializedValue - optimizedCos._serializedValue)) / (double)originalCos._serializedValue < 1e-10);

            // 平方根
            OriginalFP originalSqrt = OriginalFP.Sqrt(new OriginalFP(16));
            OptimizedFP optimizedSqrt = OptimizedFP.Sqrt(new OptimizedFP(16));
            _output.WriteLine($"Sqrt(16): 原始库 = {originalSqrt}, 优化库 = {optimizedSqrt}");
            Assert.Equal(originalSqrt._serializedValue, optimizedSqrt._serializedValue);
        }

        [Fact]
        public void PrecisionTest_LargeNumbers()
        {
            // 测试大数计算精度
            OriginalFP originalLarge = new OriginalFP(1000000);
            OptimizedFP optimizedLarge = new OptimizedFP(1000000);

            OriginalFP originalSquared = originalLarge * originalLarge;
            OptimizedFP optimizedSquared = optimizedLarge * optimizedLarge;

            _output.WriteLine($"1000000²: 原始库 = {originalSquared}, 优化库 = {optimizedSquared}");
            Assert.Equal(originalSquared._serializedValue, optimizedSquared._serializedValue);

            // 大数除以小数 - 使用小的整数替代小数
            OriginalFP originalSmall = new OriginalFP(1) / new OriginalFP(10000);
            OptimizedFP optimizedSmall = new OptimizedFP(1) / new OptimizedFP(10000);

            OriginalFP originalDiv = originalLarge / originalSmall;
            OptimizedFP optimizedDiv = optimizedLarge / optimizedSmall;

            _output.WriteLine($"1000000/0.0001: 原始库 = {originalDiv}, 优化库 = {optimizedDiv}");
            // 由于精度限制，可能会有微小差异，但相对误差应该很小
            double relativeError = Math.Abs((double)(originalDiv._serializedValue - optimizedDiv._serializedValue)) / (double)originalDiv._serializedValue;
            _output.WriteLine($"相对误差: {relativeError}");
            Assert.True(relativeError < 1e-10);
        }

        [Fact]
        public void PrecisionTest_SmallNumbers()
        {
            // 测试小数计算精度 - 使用分数表示小数，避免类型转换问题
            OriginalFP originalSmall1 = new OriginalFP(1) / new OriginalFP(100000);
            OriginalFP originalSmall2 = new OriginalFP(1) / new OriginalFP(50000);
            OptimizedFP optimizedSmall1 = new OptimizedFP(1) / new OptimizedFP(100000);
            OptimizedFP optimizedSmall2 = new OptimizedFP(1) / new OptimizedFP(50000);

            // 小数相加
            OriginalFP originalAdd = originalSmall1 + originalSmall2;
            OptimizedFP optimizedAdd = optimizedSmall1 + optimizedSmall2;
            _output.WriteLine($"小数相加: 原始库 = {originalAdd}, 优化库 = {optimizedAdd}");
            Assert.Equal(originalAdd._serializedValue, optimizedAdd._serializedValue);

            // 小数相乘
            OriginalFP originalMul = originalSmall1 * originalSmall2;
            OptimizedFP optimizedMul = optimizedSmall1 * optimizedSmall2;
            _output.WriteLine($"小数相乘: 原始库 = {originalMul}, 优化库 = {optimizedMul}");
            Assert.Equal(originalMul._serializedValue, optimizedMul._serializedValue);
        }

        [Fact]
        public void EdgeCaseTest_Limits()
        {
            // 测试极限值和边界条件

            // 最大值测试
            OriginalFP originalMax = OriginalFP.MaxValue;
            OptimizedFP optimizedMax = OptimizedFP.MaxValue;
            _output.WriteLine($"MaxValue: 原始库 = {originalMax}, 优化库 = {optimizedMax}");
            Assert.Equal(originalMax._serializedValue, optimizedMax._serializedValue);

            // 最小值测试
            OriginalFP originalMin = OriginalFP.MinValue;
            OptimizedFP optimizedMin = OptimizedFP.MinValue;
            _output.WriteLine($"MinValue: 原始库 = {originalMin}, 优化库 = {optimizedMin}");
            Assert.Equal(originalMin._serializedValue, optimizedMin._serializedValue);

            // 最小精度测试
            OriginalFP originalEpsilon = OriginalFP.Epsilon;
            OptimizedFP optimizedEpsilon = OptimizedFP.Epsilon;
            _output.WriteLine($"Epsilon: 原始库 = {originalEpsilon}, 优化库 = {optimizedEpsilon}");
            Assert.Equal(originalEpsilon._serializedValue, optimizedEpsilon._serializedValue);

            // 加法边界测试
            OriginalFP originalNearMax = OriginalFP.MaxValue - new OriginalFP(1000);
            OptimizedFP optimizedNearMax = OptimizedFP.MaxValue - new OptimizedFP(1000);

            // 在接近最大值时加法的行为
            OriginalFP originalAdd = originalNearMax + new OriginalFP(500);
            OptimizedFP optimizedAdd = optimizedNearMax + new OptimizedFP(500);
            _output.WriteLine($"接近最大值加法: 原始库 = {originalAdd}, 优化库 = {optimizedAdd}");
            Assert.Equal(originalAdd._serializedValue, optimizedAdd._serializedValue);

            // 在接近最小值时减法的行为
            OriginalFP originalNearMin = OriginalFP.MinValue + new OriginalFP(1000);
            OptimizedFP optimizedNearMin = OptimizedFP.MinValue + new OptimizedFP(1000);

            OriginalFP originalSub = originalNearMin - new OriginalFP(500);
            OptimizedFP optimizedSub = optimizedNearMin - new OptimizedFP(500);
            _output.WriteLine($"接近最小值减法: 原始库 = {originalSub}, 优化库 = {optimizedSub}");
            Assert.Equal(originalSub._serializedValue, optimizedSub._serializedValue);
        }

        [Fact]
        public void EdgeCaseTest_Overflow()
        {
            // 测试溢出行为

            // 乘法溢出测试
            OriginalFP originalLarge1 = new OriginalFP(Int16.MaxValue);
            OriginalFP originalLarge2 = new OriginalFP(Int16.MaxValue);
            OptimizedFP optimizedLarge1 = new OptimizedFP(Int16.MaxValue);
            OptimizedFP optimizedLarge2 = new OptimizedFP(Int16.MaxValue);

            OriginalFP originalMulOverflow = originalLarge1 * originalLarge2;
            OptimizedFP optimizedMulOverflow = optimizedLarge1 * optimizedLarge2;
            _output.WriteLine($"乘法溢出行为: 原始库 = {originalMulOverflow}, 优化库 = {optimizedMulOverflow}");
            Assert.Equal(originalMulOverflow._serializedValue, optimizedMulOverflow._serializedValue);

            // 除零测试
            OriginalFP originalNonZero = new OriginalFP(10);
            OriginalFP originalZero = new OriginalFP(0);
            OptimizedFP optimizedNonZero = new OptimizedFP(10);
            OptimizedFP optimizedZero = new OptimizedFP(0);

            // 记录异常行为
            bool originalThrows = false;
            bool optimizedThrows = false;
            OriginalFP originalDivByZero = OriginalFP.Zero;
            OptimizedFP optimizedDivByZero = OptimizedFP.Zero;

            try
            {
                originalDivByZero = originalNonZero / originalZero;
            }
            catch (Exception ex)
            {
                originalThrows = true;
                _output.WriteLine($"原始库除零异常: {ex.GetType().Name}: {ex.Message}");
            }

            try
            {
                optimizedDivByZero = optimizedNonZero / optimizedZero;
            }
            catch (Exception ex)
            {
                optimizedThrows = true;
                _output.WriteLine($"优化库除零异常: {ex.GetType().Name}: {ex.Message}");
            }

            Assert.Equal(originalThrows, optimizedThrows);

            if (!originalThrows && !optimizedThrows)
            {
                _output.WriteLine($"除零结果: 原始库 = {originalDivByZero}, 优化库 = {optimizedDivByZero}");
                Assert.Equal(originalDivByZero._serializedValue, optimizedDivByZero._serializedValue);
            }
        }

        [Fact]
        public void EdgeCaseTest_SpecialValues()
        {
            // 测试特殊值

            // Pi值测试
            OriginalFP originalPi = OriginalFP.Pi;
            OptimizedFP optimizedPi = OptimizedFP.Pi;
            _output.WriteLine($"Pi值: 原始库 = {originalPi}, 优化库 = {optimizedPi}");
            Assert.Equal(originalPi._serializedValue, optimizedPi._serializedValue);

            // 零值测试
            OriginalFP originalZero = OriginalFP.Zero;
            OptimizedFP optimizedZero = OptimizedFP.Zero;
            _output.WriteLine($"零值: 原始库 = {originalZero}, 优化库 = {optimizedZero}");
            Assert.Equal(originalZero._serializedValue, optimizedZero._serializedValue);

            // One值测试
            OriginalFP originalOne = OriginalFP.One;
            OptimizedFP optimizedOne = OptimizedFP.One;
            _output.WriteLine($"One值: 原始库 = {originalOne}, 优化库 = {optimizedOne}");
            Assert.Equal(originalOne._serializedValue, optimizedOne._serializedValue);

            // 负数负数相乘应该得到正数
            OriginalFP originalNeg1 = new OriginalFP(-5);
            OriginalFP originalNeg2 = new OriginalFP(-7);
            OptimizedFP optimizedNeg1 = new OptimizedFP(-5);
            OptimizedFP optimizedNeg2 = new OptimizedFP(-7);

            OriginalFP originalNegMul = originalNeg1 * originalNeg2;
            OptimizedFP optimizedNegMul = optimizedNeg1 * optimizedNeg2;
            _output.WriteLine($"负数相乘: 原始库 = {originalNegMul}, 优化库 = {optimizedNegMul}");
            Assert.Equal(originalNegMul._serializedValue, optimizedNegMul._serializedValue);

            // 各种精度角度的三角函数
            OriginalFP[] originalAngles = new OriginalFP[]
            {
                OriginalFP.Zero,
                OriginalFP.Pi / new OriginalFP(6),   // 30度
                OriginalFP.Pi / new OriginalFP(4),   // 45度
                OriginalFP.Pi / new OriginalFP(3),   // 60度
                OriginalFP.Pi / new OriginalFP(2),   // 90度
                OriginalFP.Pi,                      // 180度
                OriginalFP.Pi * new OriginalFP(3) / new OriginalFP(2), // 270度
                OriginalFP.Pi * new OriginalFP(2)   // 360度
            };

            OptimizedFP[] optimizedAngles = new OptimizedFP[]
            {
                OptimizedFP.Zero,
                OptimizedFP.Pi / new OptimizedFP(6),   // 30度
                OptimizedFP.Pi / new OptimizedFP(4),   // 45度
                OptimizedFP.Pi / new OptimizedFP(3),   // 60度
                OptimizedFP.Pi / new OptimizedFP(2),   // 90度
                OptimizedFP.Pi,                      // 180度
                OptimizedFP.Pi * new OptimizedFP(3) / new OptimizedFP(2), // 270度
                OptimizedFP.Pi * new OptimizedFP(2)   // 360度
            };

            for (int i = 0; i < originalAngles.Length; i++)
            {
                OriginalFP originalSin = OriginalFP.Sin(originalAngles[i]);
                OptimizedFP optimizedSin = OptimizedFP.Sin(optimizedAngles[i]);

                string angleName;
                switch (i)
                {
                    case 0: angleName = "0°"; break;
                    case 1: angleName = "30°"; break;
                    case 2: angleName = "45°"; break;
                    case 3: angleName = "60°"; break;
                    case 4: angleName = "90°"; break;
                    case 5: angleName = "180°"; break;
                    case 6: angleName = "270°"; break;
                    case 7: angleName = "360°"; break;
                    default: angleName = $"角度{i}"; break;
                }

                _output.WriteLine($"Sin({angleName}): 原始库 = {originalSin}, 优化库 = {optimizedSin}");

                // 允许微小差异
                double relativeError = Math.Abs((double)(originalSin._serializedValue - optimizedSin._serializedValue)) /
                                      (double)Math.Max(1, Math.Abs((double)originalSin._serializedValue));
                Assert.True(relativeError < 1e-10, $"Sin({angleName}) 的相对误差 {relativeError} 超过了容许值");
            }
        }
    }

    // 性能测试基准类
    [MemoryDiagnoser]
    public class FixedPointBenchmarks
    {
        private readonly OriginalFP[] _originalValues;
        private readonly OptimizedFP[] _optimizedValues;
        private const int Count = 10000;

        public FixedPointBenchmarks()
        {
            var random = new Random(42);
            _originalValues = new OriginalFP[Count];
            _optimizedValues = new OptimizedFP[Count];

            for (int i = 0; i < Count; i++)
            {
                // 使用int值代替decimal
                int value = random.Next(1, 1000);
                _originalValues[i] = new OriginalFP(value);
                _optimizedValues[i] = new OptimizedFP(value);
            }
        }

        [Benchmark]
        public OriginalFP[] Original_Addition()
        {
            var results = new OriginalFP[Count - 1];
            for (int i = 0; i < Count - 1; i++)
            {
                results[i] = _originalValues[i] + _originalValues[i + 1];
            }
            return results;
        }

        [Benchmark]
        public OptimizedFP[] Optimized_Addition()
        {
            var results = new OptimizedFP[Count - 1];
            for (int i = 0; i < Count - 1; i++)
            {
                results[i] = _optimizedValues[i] + _optimizedValues[i + 1];
            }
            return results;
        }

        [Benchmark]
        public OriginalFP[] Original_Multiplication()
        {
            var results = new OriginalFP[Count - 1];
            for (int i = 0; i < Count - 1; i++)
            {
                results[i] = _originalValues[i] * _originalValues[i + 1];
            }
            return results;
        }

        [Benchmark]
        public OptimizedFP[] Optimized_Multiplication()
        {
            var results = new OptimizedFP[Count - 1];
            for (int i = 0; i < Count - 1; i++)
            {
                results[i] = _optimizedValues[i] * _optimizedValues[i + 1];
            }
            return results;
        }

        [Benchmark]
        public OriginalFP[] Original_Division()
        {
            var results = new OriginalFP[Count - 1];
            for (int i = 0; i < Count - 1; i++)
            {
                if (_originalValues[i + 1]._serializedValue != 0)
                {
                    results[i] = _originalValues[i] / _originalValues[i + 1];
                }
                else
                {
                    results[i] = OriginalFP.MaxValue;
                }
            }
            return results;
        }

        [Benchmark]
        public OptimizedFP[] Optimized_Division()
        {
            var results = new OptimizedFP[Count - 1];
            for (int i = 0; i < Count - 1; i++)
            {
                if (_optimizedValues[i + 1]._serializedValue != 0)
                {
                    results[i] = _optimizedValues[i] / _optimizedValues[i + 1];
                }
                else
                {
                    results[i] = OptimizedFP.MaxValue;
                }
            }
            return results;
        }

        [Benchmark]
        public OriginalFP[] Original_Sqrt()
        {
            var results = new OriginalFP[Count];
            for (int i = 0; i < Count; i++)
            {
                if (_originalValues[i]._serializedValue >= 0)
                {
                    results[i] = OriginalFP.Sqrt(_originalValues[i]);
                }
            }
            return results;
        }

        [Benchmark]
        public OptimizedFP[] Optimized_Sqrt()
        {
            var results = new OptimizedFP[Count];
            for (int i = 0; i < Count; i++)
            {
                if (_optimizedValues[i]._serializedValue >= 0)
                {
                    results[i] = OptimizedFP.Sqrt(_optimizedValues[i]);
                }
            }
            return results;
        }
    }
}