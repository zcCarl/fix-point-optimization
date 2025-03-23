using System;
using Xunit;
using Xunit.Abstractions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using OriginalVector = RVO.Arithmetic.TSVector;
using OptimizedVector = RVO.Arithmetic.Optimized.TSVector;
using OriginalFP = RVO.Arithmetic.FP;
using OptimizedFP = RVO.Arithmetic.Optimized.FP;

namespace MathLibraryTests
{
    public class VectorTests
    {
        private readonly ITestOutputHelper _output;

        public VectorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CorrectnessTest_BasicOperations()
        {
            // 创建向量
            OriginalVector originalA = new OriginalVector(1, 2, 3);
            OriginalVector originalB = new OriginalVector(4, 5, 6);
            OptimizedVector optimizedA = new OptimizedVector(1, 2, 3);
            OptimizedVector optimizedB = new OptimizedVector(4, 5, 6);

            // 向量加法
            OriginalVector originalAdd = originalA + originalB;
            OptimizedVector optimizedAdd = optimizedA + optimizedB;
            _output.WriteLine($"向量加法: 原始库 = ({originalAdd.x}, {originalAdd.y}, {originalAdd.z}), 优化库 = ({optimizedAdd.x}, {optimizedAdd.y}, {optimizedAdd.z})");

            // 精确比较每个分量
            Assert.Equal(originalAdd.x._serializedValue, optimizedAdd.x._serializedValue);
            Assert.Equal(originalAdd.y._serializedValue, optimizedAdd.y._serializedValue);
            Assert.Equal(originalAdd.z._serializedValue, optimizedAdd.z._serializedValue);

            // 向量减法
            OriginalVector originalSub = originalA - originalB;
            OptimizedVector optimizedSub = optimizedA - optimizedB;
            _output.WriteLine($"向量减法: 原始库 = ({originalSub.x}, {originalSub.y}, {originalSub.z}), 优化库 = ({optimizedSub.x}, {optimizedSub.y}, {optimizedSub.z})");

            Assert.Equal(originalSub.x._serializedValue, optimizedSub.x._serializedValue);
            Assert.Equal(originalSub.y._serializedValue, optimizedSub.y._serializedValue);
            Assert.Equal(originalSub.z._serializedValue, optimizedSub.z._serializedValue);

            // 标量乘法
            OriginalVector originalMulScalar = originalA * new OriginalFP(2);
            OptimizedVector optimizedMulScalar = optimizedA * new OptimizedFP(2);
            _output.WriteLine($"标量乘法: 原始库 = ({originalMulScalar.x}, {originalMulScalar.y}, {originalMulScalar.z}), 优化库 = ({optimizedMulScalar.x}, {optimizedMulScalar.y}, {optimizedMulScalar.z})");

            Assert.Equal(originalMulScalar.x._serializedValue, optimizedMulScalar.x._serializedValue);
            Assert.Equal(originalMulScalar.y._serializedValue, optimizedMulScalar.y._serializedValue);
            Assert.Equal(originalMulScalar.z._serializedValue, optimizedMulScalar.z._serializedValue);

            // 点积
            OriginalFP originalDot = OriginalVector.Dot(originalA, originalB);
            OptimizedFP optimizedDot = OptimizedVector.Dot(optimizedA, optimizedB);
            _output.WriteLine($"点积: 原始库 = {originalDot}, 优化库 = {optimizedDot}");

            Assert.Equal(originalDot._serializedValue, optimizedDot._serializedValue);

            // 叉积
            OriginalVector originalCross = OriginalVector.Cross(originalA, originalB);
            OptimizedVector optimizedCross = OptimizedVector.Cross(optimizedA, optimizedB);
            _output.WriteLine($"叉积: 原始库 = ({originalCross.x}, {originalCross.y}, {originalCross.z}), 优化库 = ({optimizedCross.x}, {optimizedCross.y}, {optimizedCross.z})");

            Assert.Equal(originalCross.x._serializedValue, optimizedCross.x._serializedValue);
            Assert.Equal(originalCross.y._serializedValue, optimizedCross.y._serializedValue);
            Assert.Equal(originalCross.z._serializedValue, optimizedCross.z._serializedValue);
        }

        [Fact]
        public void CorrectnessTest_AdvancedOperations()
        {
            // 创建向量
            OriginalVector originalA = new OriginalVector(3, 4, 5);
            OptimizedVector optimizedA = new OptimizedVector(3, 4, 5);

            // 向量长度
            OriginalFP originalMagnitude = originalA.magnitude;
            OptimizedFP optimizedMagnitude = optimizedA.magnitude;
            _output.WriteLine($"向量长度: 原始库 = {originalMagnitude}, 优化库 = {optimizedMagnitude}");

            // 由于可能存在舍入误差，使用相对误差比较
            double relativeErrorMag = Math.Abs((double)(originalMagnitude._serializedValue - optimizedMagnitude._serializedValue)) / (double)originalMagnitude._serializedValue;
            _output.WriteLine($"长度相对误差: {relativeErrorMag}");
            Assert.True(relativeErrorMag < 1e-10);

            // 向量平方长度
            OriginalFP originalSqrMagnitude = originalA.sqrMagnitude;
            OptimizedFP optimizedSqrMagnitude = optimizedA.sqrMagnitude;
            _output.WriteLine($"向量平方长度: 原始库 = {originalSqrMagnitude}, 优化库 = {optimizedSqrMagnitude}");

            Assert.Equal(originalSqrMagnitude._serializedValue, optimizedSqrMagnitude._serializedValue);

            // 归一化向量
            OriginalVector originalNormalized = originalA.normalized;
            OptimizedVector optimizedNormalized = optimizedA.normalized;
            _output.WriteLine($"归一化向量: 原始库 = ({originalNormalized.x}, {originalNormalized.y}, {originalNormalized.z}), 优化库 = ({optimizedNormalized.x}, {optimizedNormalized.y}, {optimizedNormalized.z})");

            // 归一化后的长度应接近1
            Assert.True(Math.Abs((double)(originalNormalized.magnitude._serializedValue - OriginalFP.ONE)) < OriginalFP.EN3._serializedValue);
            Assert.True(Math.Abs((double)(optimizedNormalized.magnitude._serializedValue - OptimizedFP.ONE)) < OptimizedFP.EN3._serializedValue);

            // 比较归一化分量，允许微小差异
            double relativeErrorX = Math.Abs((double)(originalNormalized.x._serializedValue - optimizedNormalized.x._serializedValue)) / (double)originalNormalized.x._serializedValue;
            double relativeErrorY = Math.Abs((double)(originalNormalized.y._serializedValue - optimizedNormalized.y._serializedValue)) / (double)originalNormalized.y._serializedValue;
            double relativeErrorZ = Math.Abs((double)(originalNormalized.z._serializedValue - optimizedNormalized.z._serializedValue)) / (double)originalNormalized.z._serializedValue;

            _output.WriteLine($"归一化分量相对误差: X={relativeErrorX}, Y={relativeErrorY}, Z={relativeErrorZ}");

            Assert.True(relativeErrorX < 1e-10);
            Assert.True(relativeErrorY < 1e-10);
            Assert.True(relativeErrorZ < 1e-10);
        }

        [Fact]
        public void PrecisionTest_ComplexCalculations()
        {
            // 测试复杂计算的精度 - 使用整数值替代小数，避免类型转换问题
            OriginalVector originalA = new OriginalVector(1, 4, 7);
            OriginalVector originalB = new OriginalVector(9, 6, 3);
            OptimizedVector optimizedA = new OptimizedVector(1, 4, 7);
            OptimizedVector optimizedB = new OptimizedVector(9, 6, 3);

            // 复杂计算：投影
            OriginalFP originalDot = OriginalVector.Dot(originalA, originalB);
            OriginalFP originalBSqrMag = originalB.sqrMagnitude;
            OriginalVector originalProj = originalB * (originalDot / originalBSqrMag);

            OptimizedFP optimizedDot = OptimizedVector.Dot(optimizedA, optimizedB);
            OptimizedFP optimizedBSqrMag = optimizedB.sqrMagnitude;
            OptimizedVector optimizedProj = optimizedB * (optimizedDot / optimizedBSqrMag);

            _output.WriteLine($"向量投影: 原始库 = ({originalProj.x}, {originalProj.y}, {originalProj.z}), 优化库 = ({optimizedProj.x}, {optimizedProj.y}, {optimizedProj.z})");

            // 比较投影结果，允许微小误差
            double relErrX = Math.Abs((double)(originalProj.x._serializedValue - optimizedProj.x._serializedValue)) / (double)Math.Max(1, Math.Abs((double)originalProj.x._serializedValue));
            double relErrY = Math.Abs((double)(originalProj.y._serializedValue - optimizedProj.y._serializedValue)) / (double)Math.Max(1, Math.Abs((double)originalProj.y._serializedValue));
            double relErrZ = Math.Abs((double)(originalProj.z._serializedValue - optimizedProj.z._serializedValue)) / (double)Math.Max(1, Math.Abs((double)originalProj.z._serializedValue));

            _output.WriteLine($"投影相对误差: X={relErrX}, Y={relErrY}, Z={relErrZ}");

            Assert.True(relErrX < 1e-10);
            Assert.True(relErrY < 1e-10);
            Assert.True(relErrZ < 1e-10);

            // 复杂计算：反射
            OriginalVector originalNormal = new OriginalVector(0, 1, 0).normalized;
            OptimizedVector optimizedNormal = new OptimizedVector(0, 1, 0).normalized;

            // 使用2代替Two
            OriginalVector originalReflect = originalA - originalNormal * (new OriginalFP(2)) * OriginalVector.Dot(originalA, originalNormal);
            OptimizedVector optimizedReflect = optimizedA - optimizedNormal * (new OptimizedFP(2)) * OptimizedVector.Dot(optimizedA, optimizedNormal);

            _output.WriteLine($"向量反射: 原始库 = ({originalReflect.x}, {originalReflect.y}, {originalReflect.z}), 优化库 = ({optimizedReflect.x}, {optimizedReflect.y}, {optimizedReflect.z})");

            // 比较反射结果，允许微小误差
            relErrX = Math.Abs((double)(originalReflect.x._serializedValue - optimizedReflect.x._serializedValue)) / (double)Math.Max(1, Math.Abs((double)originalReflect.x._serializedValue));
            relErrY = Math.Abs((double)(originalReflect.y._serializedValue - optimizedReflect.y._serializedValue)) / (double)Math.Max(1, Math.Abs((double)originalReflect.y._serializedValue));
            relErrZ = Math.Abs((double)(originalReflect.z._serializedValue - optimizedReflect.z._serializedValue)) / (double)Math.Max(1, Math.Abs((double)originalReflect.z._serializedValue));

            _output.WriteLine($"反射相对误差: X={relErrX}, Y={relErrY}, Z={relErrZ}");

            Assert.True(relErrX < 1e-10);
            Assert.True(relErrY < 1e-10);
            Assert.True(relErrZ < 1e-10);
        }
    }

    // 向量性能测试基准类
    [MemoryDiagnoser]
    public class VectorBenchmarks
    {
        private readonly OriginalVector[] _originalVectors1;
        private readonly OriginalVector[] _originalVectors2;
        private readonly OptimizedVector[] _optimizedVectors1;
        private readonly OptimizedVector[] _optimizedVectors2;
        private const int Count = 10000;

        public VectorBenchmarks()
        {
            var random = new Random(42);
            _originalVectors1 = new OriginalVector[Count];
            _originalVectors2 = new OriginalVector[Count];
            _optimizedVectors1 = new OptimizedVector[Count];
            _optimizedVectors2 = new OptimizedVector[Count];

            for (int i = 0; i < Count; i++)
            {
                // 使用int值代替decimal
                int x1 = random.Next(-50, 50);
                int y1 = random.Next(-50, 50);
                int z1 = random.Next(-50, 50);

                int x2 = random.Next(-50, 50);
                int y2 = random.Next(-50, 50);
                int z2 = random.Next(-50, 50);

                _originalVectors1[i] = new OriginalVector(x1, y1, z1);
                _originalVectors2[i] = new OriginalVector(x2, y2, z2);
                _optimizedVectors1[i] = new OptimizedVector(x1, y1, z1);
                _optimizedVectors2[i] = new OptimizedVector(x2, y2, z2);
            }
        }

        [Benchmark]
        public OriginalVector[] Original_Vector_Addition()
        {
            var results = new OriginalVector[Count];
            for (int i = 0; i < Count; i++)
            {
                results[i] = _originalVectors1[i] + _originalVectors2[i];
            }
            return results;
        }

        [Benchmark]
        public OptimizedVector[] Optimized_Vector_Addition()
        {
            var results = new OptimizedVector[Count];
            for (int i = 0; i < Count; i++)
            {
                results[i] = _optimizedVectors1[i] + _optimizedVectors2[i];
            }
            return results;
        }

        [Benchmark]
        public OriginalVector[] Original_Vector_Cross()
        {
            var results = new OriginalVector[Count];
            for (int i = 0; i < Count; i++)
            {
                results[i] = OriginalVector.Cross(_originalVectors1[i], _originalVectors2[i]);
            }
            return results;
        }

        [Benchmark]
        public OptimizedVector[] Optimized_Vector_Cross()
        {
            var results = new OptimizedVector[Count];
            for (int i = 0; i < Count; i++)
            {
                results[i] = OptimizedVector.Cross(_optimizedVectors1[i], _optimizedVectors2[i]);
            }
            return results;
        }

        [Benchmark]
        public OriginalFP[] Original_Vector_Dot()
        {
            var results = new OriginalFP[Count];
            for (int i = 0; i < Count; i++)
            {
                results[i] = OriginalVector.Dot(_originalVectors1[i], _originalVectors2[i]);
            }
            return results;
        }

        [Benchmark]
        public OptimizedFP[] Optimized_Vector_Dot()
        {
            var results = new OptimizedFP[Count];
            for (int i = 0; i < Count; i++)
            {
                results[i] = OptimizedVector.Dot(_optimizedVectors1[i], _optimizedVectors2[i]);
            }
            return results;
        }

        [Benchmark]
        public OriginalVector[] Original_Vector_Normalize()
        {
            var results = new OriginalVector[Count];
            for (int i = 0; i < Count; i++)
            {
                results[i] = _originalVectors1[i].normalized;
            }
            return results;
        }

        [Benchmark]
        public OptimizedVector[] Optimized_Vector_Normalize()
        {
            var results = new OptimizedVector[Count];
            for (int i = 0; i < Count; i++)
            {
                results[i] = _optimizedVectors1[i].normalized;
            }
            return results;
        }

        [Benchmark]
        public OriginalFP[] Original_Vector_Magnitude()
        {
            var results = new OriginalFP[Count];
            for (int i = 0; i < Count; i++)
            {
                results[i] = _originalVectors1[i].magnitude;
            }
            return results;
        }

        [Benchmark]
        public OptimizedFP[] Optimized_Vector_Magnitude()
        {
            var results = new OptimizedFP[Count];
            for (int i = 0; i < Count; i++)
            {
                results[i] = _optimizedVectors1[i].magnitude;
            }
            return results;
        }
    }
}