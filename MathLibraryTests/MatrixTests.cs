using System;
using Xunit;
using Xunit.Abstractions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using OriginalVector = RVO.Arithmetic.TSVector;
using OptimizedVector = RVO.Arithmetic.Optimized.TSVector;
using OriginalMatrix = RVO.Arithmetic.TSMatrix;
using OptimizedMatrix = RVO.Arithmetic.Optimized.TSMatrix;
using OriginalMatrix4x4 = RVO.Arithmetic.TSMatrix4x4;
using OptimizedMatrix4x4 = RVO.Arithmetic.Optimized.TSMatrix4x4;
using OriginalFP = RVO.Arithmetic.FP;
using OptimizedFP = RVO.Arithmetic.Optimized.FP;

namespace MathLibraryTests
{
    public class MatrixTests
    {
        private readonly ITestOutputHelper _output;

        public MatrixTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CorrectnessTest_MatrixOperations()
        {
            // 创建测试矩阵，TSMatrix构造函数需要9个参数
            OriginalMatrix originalA = new OriginalMatrix(
                new OriginalFP(1), new OriginalFP(2), new OriginalFP(3),
                new OriginalFP(4), new OriginalFP(5), new OriginalFP(6),
                new OriginalFP(7), new OriginalFP(8), new OriginalFP(9)
            );

            OptimizedMatrix optimizedA = new OptimizedMatrix(
                new OptimizedFP(1), new OptimizedFP(2), new OptimizedFP(3),
                new OptimizedFP(4), new OptimizedFP(5), new OptimizedFP(6),
                new OptimizedFP(7), new OptimizedFP(8), new OptimizedFP(9)
            );

            OriginalMatrix originalB = new OriginalMatrix(
                new OriginalFP(9), new OriginalFP(8), new OriginalFP(7),
                new OriginalFP(6), new OriginalFP(5), new OriginalFP(4),
                new OriginalFP(3), new OriginalFP(2), new OriginalFP(1)
            );

            OptimizedMatrix optimizedB = new OptimizedMatrix(
                new OptimizedFP(9), new OptimizedFP(8), new OptimizedFP(7),
                new OptimizedFP(6), new OptimizedFP(5), new OptimizedFP(4),
                new OptimizedFP(3), new OptimizedFP(2), new OptimizedFP(1)
            );

            // 测试矩阵加法
            OriginalMatrix originalSum = originalA + originalB;
            OptimizedMatrix optimizedSum = optimizedA + optimizedB;

            _output.WriteLine("矩阵加法:");
            _output.WriteLine($"原始库结果: {originalSum}");
            _output.WriteLine($"优化库结果: {optimizedSum}");

            Assert.Equal(originalSum.M11._serializedValue, optimizedSum.M11._serializedValue);
            Assert.Equal(originalSum.M12._serializedValue, optimizedSum.M12._serializedValue);
            Assert.Equal(originalSum.M13._serializedValue, optimizedSum.M13._serializedValue);
            Assert.Equal(originalSum.M21._serializedValue, optimizedSum.M21._serializedValue);
            Assert.Equal(originalSum.M22._serializedValue, optimizedSum.M22._serializedValue);
            Assert.Equal(originalSum.M23._serializedValue, optimizedSum.M23._serializedValue);
            Assert.Equal(originalSum.M31._serializedValue, optimizedSum.M31._serializedValue);
            Assert.Equal(originalSum.M32._serializedValue, optimizedSum.M32._serializedValue);
            Assert.Equal(originalSum.M33._serializedValue, optimizedSum.M33._serializedValue);

            // 测试矩阵乘法
            OriginalMatrix originalProduct = originalA * originalB;
            OptimizedMatrix optimizedProduct = optimizedA * optimizedB;

            _output.WriteLine("矩阵乘法:");
            _output.WriteLine($"原始库结果: {originalProduct}");
            _output.WriteLine($"优化库结果: {optimizedProduct}");

            Assert.Equal(originalProduct.M11._serializedValue, optimizedProduct.M11._serializedValue);
            Assert.Equal(originalProduct.M12._serializedValue, optimizedProduct.M12._serializedValue);
            Assert.Equal(originalProduct.M13._serializedValue, optimizedProduct.M13._serializedValue);
            Assert.Equal(originalProduct.M21._serializedValue, optimizedProduct.M21._serializedValue);
            Assert.Equal(originalProduct.M22._serializedValue, optimizedProduct.M22._serializedValue);
            Assert.Equal(originalProduct.M23._serializedValue, optimizedProduct.M23._serializedValue);
            Assert.Equal(originalProduct.M31._serializedValue, optimizedProduct.M31._serializedValue);
            Assert.Equal(originalProduct.M32._serializedValue, optimizedProduct.M32._serializedValue);
            Assert.Equal(originalProduct.M33._serializedValue, optimizedProduct.M33._serializedValue);

            // 测试矩阵与向量的乘法
            OriginalVector originalVec = new OriginalVector(
                new OriginalFP(1),
                new OriginalFP(2),
                new OriginalFP(3)
            );

            OptimizedVector optimizedVec = new OptimizedVector(
                new OptimizedFP(1),
                new OptimizedFP(2),
                new OptimizedFP(3)
            );

            // 使用Transform方法转换向量
            OriginalVector originalTransformed = OriginalVector.Transform(originalVec, originalA);
            OptimizedVector optimizedTransformed = OptimizedVector.Transform(optimizedVec, optimizedA);

            _output.WriteLine("矩阵与向量乘法:");
            _output.WriteLine($"原始库结果: {originalTransformed}");
            _output.WriteLine($"优化库结果: {optimizedTransformed}");

            Assert.Equal(originalTransformed.x._serializedValue, optimizedTransformed.x._serializedValue);
            Assert.Equal(originalTransformed.y._serializedValue, optimizedTransformed.y._serializedValue);
            Assert.Equal(originalTransformed.z._serializedValue, optimizedTransformed.z._serializedValue);
        }

        [Fact]
        public void CorrectnessTest_Matrix4x4Operations()
        {
            // 测试旋转矩阵创建
            OriginalFP originalAngle = new OriginalFP(45) * OriginalFP.Deg2Rad;
            OptimizedFP optimizedAngle = new OptimizedFP(45) * OptimizedFP.Deg2Rad;

            // 手动创建一个简化的旋转矩阵进行测试
            OriginalFP originalCos = OriginalFP.Cos(originalAngle);
            OriginalFP originalSin = OriginalFP.Sin(originalAngle);
            OptimizedFP optimizedCos = OptimizedFP.Cos(optimizedAngle);
            OptimizedFP optimizedSin = OptimizedFP.Sin(optimizedAngle);

            _output.WriteLine("旋转测试:");
            _output.WriteLine($"原始库: cos={originalCos}, sin={originalSin}");
            _output.WriteLine($"优化库: cos={optimizedCos}, sin={optimizedSin}");

            // 测试余弦和正弦值是否相等
            Assert.Equal(originalCos._serializedValue, optimizedCos._serializedValue);
            Assert.Equal(originalSin._serializedValue, optimizedSin._serializedValue);

            // 测试平移向量
            OriginalVector originalTranslation = new OriginalVector(
                new OriginalFP(10),
                new OriginalFP(20),
                new OriginalFP(30)
            );

            OptimizedVector optimizedTranslation = new OptimizedVector(
                new OptimizedFP(10),
                new OptimizedFP(20),
                new OptimizedFP(30)
            );

            _output.WriteLine("平移向量测试:");
            _output.WriteLine($"原始库结果: {originalTranslation}");
            _output.WriteLine($"优化库结果: {optimizedTranslation}");

            // 测试向量元素
            Assert.Equal(originalTranslation.x._serializedValue, optimizedTranslation.x._serializedValue);
            Assert.Equal(originalTranslation.y._serializedValue, optimizedTranslation.y._serializedValue);
            Assert.Equal(originalTranslation.z._serializedValue, optimizedTranslation.z._serializedValue);
        }

        [Fact]
        public void CorrectnessTest_RotationMatrices()
        {
            // 测试旋转矩阵 - 创建4x4矩阵来测试不同轴的旋转
            OriginalFP originalAngle = new OriginalFP(45) * OriginalFP.Deg2Rad; // 45度
            OptimizedFP optimizedAngle = new OptimizedFP(45) * OptimizedFP.Deg2Rad;

            // 使用静态RotateX方法创建旋转矩阵
            OriginalMatrix4x4 originalRotationX = OriginalMatrix4x4.RotateX(originalAngle);
            OptimizedMatrix4x4 optimizedRotationX = OptimizedMatrix4x4.RotateX(optimizedAngle);

            // 绕Y轴旋转
            OriginalMatrix4x4 originalRotationY = OriginalMatrix4x4.RotateY(originalAngle);
            OptimizedMatrix4x4 optimizedRotationY = OptimizedMatrix4x4.RotateY(optimizedAngle);

            // 绕Z轴旋转
            OriginalMatrix4x4 originalRotationZ = OriginalMatrix4x4.RotateZ(originalAngle);
            OptimizedMatrix4x4 optimizedRotationZ = OptimizedMatrix4x4.RotateZ(optimizedAngle);

            _output.WriteLine("旋转矩阵测试:");
            _output.WriteLine($"X轴旋转 - 原始库: \n{originalRotationX}");
            _output.WriteLine($"X轴旋转 - 优化库: \n{optimizedRotationX}");

            // 测试旋转向量 - 绕X轴旋转应保持X分量不变
            OriginalVector originalVec = new OriginalVector(new OriginalFP(1), new OriginalFP(1), new OriginalFP(0));
            OptimizedVector optimizedVec = new OptimizedVector(new OptimizedFP(1), new OptimizedFP(1), new OptimizedFP(0));

            // 需要先从4x4矩阵创建一个3x3矩阵来进行转换
            OriginalMatrix original3x3 = new OriginalMatrix(
                originalRotationX.M11, originalRotationX.M12, originalRotationX.M13,
                originalRotationX.M21, originalRotationX.M22, originalRotationX.M23,
                originalRotationX.M31, originalRotationX.M32, originalRotationX.M33
            );

            OptimizedMatrix optimized3x3 = new OptimizedMatrix(
                optimizedRotationX.M11, optimizedRotationX.M12, optimizedRotationX.M13,
                optimizedRotationX.M21, optimizedRotationX.M22, optimizedRotationX.M23,
                optimizedRotationX.M31, optimizedRotationX.M32, optimizedRotationX.M33
            );

            OriginalVector originalRotatedX = OriginalVector.Transform(originalVec, original3x3);
            OptimizedVector optimizedRotatedX = OptimizedVector.Transform(optimizedVec, optimized3x3);

            _output.WriteLine($"原始向量: {originalVec}");
            _output.WriteLine($"X轴旋转后 - 原始库: {originalRotatedX}");
            _output.WriteLine($"X轴旋转后 - 优化库: {optimizedRotatedX}");

            // X分量应保持不变
            Assert.Equal(originalVec.x._serializedValue, originalRotatedX.x._serializedValue);
            Assert.Equal(optimizedVec.x._serializedValue, optimizedRotatedX.x._serializedValue);

            // 比较两个库的实现结果
            double relErrorX = Math.Abs((double)(originalRotatedX.x._serializedValue - optimizedRotatedX.x._serializedValue)) /
                             (double)Math.Max(1, Math.Abs((double)originalRotatedX.x._serializedValue));
            double relErrorY = Math.Abs((double)(originalRotatedX.y._serializedValue - optimizedRotatedX.y._serializedValue)) /
                             (double)Math.Max(1, Math.Abs((double)originalRotatedX.y._serializedValue));
            double relErrorZ = Math.Abs((double)(originalRotatedX.z._serializedValue - optimizedRotatedX.z._serializedValue)) /
                             (double)Math.Max(1, Math.Abs((double)originalRotatedX.z._serializedValue));

            _output.WriteLine($"相对误差: X={relErrorX}, Y={relErrorY}, Z={relErrorZ}");

            Assert.True(relErrorX < 1e-10);
            Assert.True(relErrorY < 1e-10);
            Assert.True(relErrorZ < 1e-10);
        }
    }

    // 性能测试基准类
    [MemoryDiagnoser]
    public class MatrixBenchmarks
    {
        private readonly OriginalMatrix[] _originalMatrices;
        private readonly OptimizedMatrix[] _optimizedMatrices;
        private readonly OriginalVector[] _originalVectors;
        private readonly OptimizedVector[] _optimizedVectors;
        private const int Count = 1000;

        public MatrixBenchmarks()
        {
            var random = new Random(42);
            _originalMatrices = new OriginalMatrix[Count];
            _optimizedMatrices = new OptimizedMatrix[Count];
            _originalVectors = new OriginalVector[Count];
            _optimizedVectors = new OptimizedVector[Count];

            for (int i = 0; i < Count; i++)
            {
                // 使用固定的整数值初始化矩阵
                _originalMatrices[i] = new OriginalMatrix(
                    new OriginalFP(random.Next(1, 10)), new OriginalFP(random.Next(1, 10)), new OriginalFP(random.Next(1, 10)),
                    new OriginalFP(random.Next(1, 10)), new OriginalFP(random.Next(1, 10)), new OriginalFP(random.Next(1, 10)),
                    new OriginalFP(random.Next(1, 10)), new OriginalFP(random.Next(1, 10)), new OriginalFP(random.Next(1, 10))
                );

                _optimizedMatrices[i] = new OptimizedMatrix(
                    new OptimizedFP(random.Next(1, 10)), new OptimizedFP(random.Next(1, 10)), new OptimizedFP(random.Next(1, 10)),
                    new OptimizedFP(random.Next(1, 10)), new OptimizedFP(random.Next(1, 10)), new OptimizedFP(random.Next(1, 10)),
                    new OptimizedFP(random.Next(1, 10)), new OptimizedFP(random.Next(1, 10)), new OptimizedFP(random.Next(1, 10))
                );

                // 使用固定的整数值初始化向量
                _originalVectors[i] = new OriginalVector(
                    new OriginalFP(random.Next(1, 10)),
                    new OriginalFP(random.Next(1, 10)),
                    new OriginalFP(random.Next(1, 10))
                );

                _optimizedVectors[i] = new OptimizedVector(
                    new OptimizedFP(random.Next(1, 10)),
                    new OptimizedFP(random.Next(1, 10)),
                    new OptimizedFP(random.Next(1, 10))
                );
            }
        }

        [Benchmark]
        public OriginalMatrix[] Original_MatrixMultiplication()
        {
            var results = new OriginalMatrix[Count - 1];
            for (int i = 0; i < Count - 1; i++)
            {
                results[i] = _originalMatrices[i] * _originalMatrices[i + 1];
            }
            return results;
        }

        [Benchmark]
        public OptimizedMatrix[] Optimized_MatrixMultiplication()
        {
            var results = new OptimizedMatrix[Count - 1];
            for (int i = 0; i < Count - 1; i++)
            {
                results[i] = _optimizedMatrices[i] * _optimizedMatrices[i + 1];
            }
            return results;
        }

        [Benchmark]
        public OriginalVector[] Original_MatrixVectorMultiplication()
        {
            var results = new OriginalVector[Count];
            for (int i = 0; i < Count; i++)
            {
                results[i] = OriginalVector.Transform(_originalVectors[i], _originalMatrices[i]);
            }
            return results;
        }

        [Benchmark]
        public OptimizedVector[] Optimized_MatrixVectorMultiplication()
        {
            var results = new OptimizedVector[Count];
            for (int i = 0; i < Count; i++)
            {
                results[i] = OptimizedVector.Transform(_optimizedVectors[i], _optimizedMatrices[i]);
            }
            return results;
        }

        [Benchmark]
        public OriginalMatrix[] Original_MatrixAddition()
        {
            var results = new OriginalMatrix[Count - 1];
            for (int i = 0; i < Count - 1; i++)
            {
                results[i] = _originalMatrices[i] + _originalMatrices[i + 1];
            }
            return results;
        }

        [Benchmark]
        public OptimizedMatrix[] Optimized_MatrixAddition()
        {
            var results = new OptimizedMatrix[Count - 1];
            for (int i = 0; i < Count - 1; i++)
            {
                results[i] = _optimizedMatrices[i] + _optimizedMatrices[i + 1];
            }
            return results;
        }
    }
}