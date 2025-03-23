using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Numerics;

namespace MathLibraryTests.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class TSMatrix4x4Benchmarks
    {
        private const int IterationCount = 1000; // 矩阵操作较重，减少迭代次数

        // 输入值 - 4x4矩阵
        private readonly Matrix4x4[] _systemMatrices;
        private readonly RVO.Arithmetic.TSMatrix4x4[] _originalMatrices;
        private readonly RVO.Arithmetic.Optimized.TSMatrix4x4[] _optimizedMatrices;

        // 向量，用于变换测试
        private readonly Vector4[] _systemVectors;
        private readonly RVO.Arithmetic.TSVector4[] _originalVectors;
        private readonly RVO.Arithmetic.Optimized.TSVector4[] _optimizedVectors;

        public TSMatrix4x4Benchmarks()
        {
            // 初始化测试数据
            _systemMatrices = new Matrix4x4[IterationCount];
            _originalMatrices = new RVO.Arithmetic.TSMatrix4x4[IterationCount];
            _optimizedMatrices = new RVO.Arithmetic.Optimized.TSMatrix4x4[IterationCount];

            _systemVectors = new Vector4[IterationCount];
            _originalVectors = new RVO.Arithmetic.TSVector4[IterationCount];
            _optimizedVectors = new RVO.Arithmetic.Optimized.TSVector4[IterationCount];

            var random = new Random(42); // 固定种子以确保可重复性

            for (int i = 0; i < IterationCount; i++)
            {
                // 生成-10到10范围内的随机值作为矩阵元素
                float m11 = (float)((random.NextDouble() * 20) - 10);
                float m12 = (float)((random.NextDouble() * 20) - 10);
                float m13 = (float)((random.NextDouble() * 20) - 10);
                float m14 = (float)((random.NextDouble() * 20) - 10);
                float m21 = (float)((random.NextDouble() * 20) - 10);
                float m22 = (float)((random.NextDouble() * 20) - 10);
                float m23 = (float)((random.NextDouble() * 20) - 10);
                float m24 = (float)((random.NextDouble() * 20) - 10);
                float m31 = (float)((random.NextDouble() * 20) - 10);
                float m32 = (float)((random.NextDouble() * 20) - 10);
                float m33 = (float)((random.NextDouble() * 20) - 10);
                float m34 = (float)((random.NextDouble() * 20) - 10);
                float m41 = (float)((random.NextDouble() * 20) - 10);
                float m42 = (float)((random.NextDouble() * 20) - 10);
                float m43 = (float)((random.NextDouble() * 20) - 10);
                float m44 = (float)((random.NextDouble() * 20) - 10);

                // 创建System.Numerics.Matrix4x4
                _systemMatrices[i] = new Matrix4x4(
                    m11, m12, m13, m14,
                    m21, m22, m23, m24,
                    m31, m32, m33, m34,
                    m41, m42, m43, m44
                );

                // 创建TSMatrix4x4
                _originalMatrices[i] = new RVO.Arithmetic.TSMatrix4x4(
                    (RVO.Arithmetic.FP)m11,
                    (RVO.Arithmetic.FP)m12,
                    (RVO.Arithmetic.FP)m13,
                    (RVO.Arithmetic.FP)m14,
                    (RVO.Arithmetic.FP)m21,
                    (RVO.Arithmetic.FP)m22,
                    (RVO.Arithmetic.FP)m23,
                    (RVO.Arithmetic.FP)m24,
                    (RVO.Arithmetic.FP)m31,
                    (RVO.Arithmetic.FP)m32,
                    (RVO.Arithmetic.FP)m33,
                    (RVO.Arithmetic.FP)m34,
                    (RVO.Arithmetic.FP)m41,
                    (RVO.Arithmetic.FP)m42,
                    (RVO.Arithmetic.FP)m43,
                    (RVO.Arithmetic.FP)m44
                );

                // 创建优化版TSMatrix4x4
                _optimizedMatrices[i] = new RVO.Arithmetic.Optimized.TSMatrix4x4(
                    (RVO.Arithmetic.Optimized.FP)m11,
                    (RVO.Arithmetic.Optimized.FP)m12,
                    (RVO.Arithmetic.Optimized.FP)m13,
                    (RVO.Arithmetic.Optimized.FP)m14,
                    (RVO.Arithmetic.Optimized.FP)m21,
                    (RVO.Arithmetic.Optimized.FP)m22,
                    (RVO.Arithmetic.Optimized.FP)m23,
                    (RVO.Arithmetic.Optimized.FP)m24,
                    (RVO.Arithmetic.Optimized.FP)m31,
                    (RVO.Arithmetic.Optimized.FP)m32,
                    (RVO.Arithmetic.Optimized.FP)m33,
                    (RVO.Arithmetic.Optimized.FP)m34,
                    (RVO.Arithmetic.Optimized.FP)m41,
                    (RVO.Arithmetic.Optimized.FP)m42,
                    (RVO.Arithmetic.Optimized.FP)m43,
                    (RVO.Arithmetic.Optimized.FP)m44
                );

                // 生成-100到100范围内的随机值作为向量元素
                float x = (float)((random.NextDouble() * 200) - 100);
                float y = (float)((random.NextDouble() * 200) - 100);
                float z = (float)((random.NextDouble() * 200) - 100);
                float w = (float)((random.NextDouble() * 200) - 100);

                _systemVectors[i] = new Vector4(x, y, z, w);
                _originalVectors[i] = new RVO.Arithmetic.TSVector4(
                    (RVO.Arithmetic.FP)x,
                    (RVO.Arithmetic.FP)y,
                    (RVO.Arithmetic.FP)z,
                    (RVO.Arithmetic.FP)w
                );
                _optimizedVectors[i] = new RVO.Arithmetic.Optimized.TSVector4(
                    (RVO.Arithmetic.Optimized.FP)x,
                    (RVO.Arithmetic.Optimized.FP)y,
                    (RVO.Arithmetic.Optimized.FP)z,
                    (RVO.Arithmetic.Optimized.FP)w
                );
            }
        }

        // 矩阵乘法（矩阵 * 矩阵）
        [Benchmark(Baseline = true)]
        public Matrix4x4 MatrixMultiplication_SystemMatrix()
        {
            Matrix4x4 result = Matrix4x4.Identity;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result = Matrix4x4.Multiply(result, _systemMatrices[i]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSMatrix4x4 MatrixMultiplication_OriginalTSMatrix()
        {
            RVO.Arithmetic.TSMatrix4x4 result = RVO.Arithmetic.TSMatrix4x4.Identity;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result = result * _originalMatrices[i];
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSMatrix4x4 MatrixMultiplication_OptimizedTSMatrix()
        {
            RVO.Arithmetic.Optimized.TSMatrix4x4 result = RVO.Arithmetic.Optimized.TSMatrix4x4.Identity;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result = result * _optimizedMatrices[i];
            }
            return result;
        }

        // 矩阵求逆
        [Benchmark]
        public Matrix4x4 Inverse_SystemMatrix()
        {
            Matrix4x4 result = Matrix4x4.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                Matrix4x4 inverse;
                if (Matrix4x4.Invert(_systemMatrices[i], out inverse))
                {
                    result = Matrix4x4.Multiply(result, inverse);
                }
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSMatrix4x4 Inverse_OriginalTSMatrix()
        {
            RVO.Arithmetic.TSMatrix4x4 result = RVO.Arithmetic.TSMatrix4x4.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * RVO.Arithmetic.TSMatrix4x4.Inverse(_originalMatrices[i]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSMatrix4x4 Inverse_OptimizedTSMatrix()
        {
            RVO.Arithmetic.Optimized.TSMatrix4x4 result = RVO.Arithmetic.Optimized.TSMatrix4x4.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * RVO.Arithmetic.Optimized.TSMatrix4x4.Inverse(_optimizedMatrices[i]);
            }
            return result;
        }

        // 矩阵转置
        [Benchmark]
        public Matrix4x4 Transpose_SystemMatrix()
        {
            Matrix4x4 result = Matrix4x4.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = Matrix4x4.Multiply(result, Matrix4x4.Transpose(_systemMatrices[i]));
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSMatrix4x4 Transpose_OriginalTSMatrix()
        {
            RVO.Arithmetic.TSMatrix4x4 result = RVO.Arithmetic.TSMatrix4x4.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * RVO.Arithmetic.TSMatrix4x4.Transpose(_originalMatrices[i]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSMatrix4x4 Transpose_OptimizedTSMatrix()
        {
            RVO.Arithmetic.Optimized.TSMatrix4x4 result = RVO.Arithmetic.Optimized.TSMatrix4x4.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * RVO.Arithmetic.Optimized.TSMatrix4x4.Transpose(_optimizedMatrices[i]);
            }
            return result;
        }

        // 矩阵向量变换
        [Benchmark]
        public Vector4 VectorTransform_SystemMatrix()
        {
            Vector4 result = Vector4.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += Vector4.Transform(_systemVectors[i], _systemMatrices[i]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSVector4 VectorTransform_OriginalTSMatrix()
        {
            RVO.Arithmetic.TSVector4 result = RVO.Arithmetic.TSVector4.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += RVO.Arithmetic.TSVector4.Transform(_originalVectors[i], _originalMatrices[i]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSVector4 VectorTransform_OptimizedTSMatrix()
        {
            RVO.Arithmetic.Optimized.TSVector4 result = RVO.Arithmetic.Optimized.TSVector4.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += RVO.Arithmetic.Optimized.TSVector4.Transform(_optimizedVectors[i], _optimizedMatrices[i]);
            }
            return result;
        }

        // 行列式计算
        [Benchmark]
        public float Determinant_SystemMatrix()
        {
            float result = 0;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _systemMatrices[i].GetDeterminant();
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.FP Determinant_OriginalTSMatrix()
        {
            RVO.Arithmetic.FP result = RVO.Arithmetic.FP.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _originalMatrices[i].determinant;
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.FP Determinant_OptimizedTSMatrix()
        {
            RVO.Arithmetic.Optimized.FP result = RVO.Arithmetic.Optimized.FP.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _optimizedMatrices[i].determinant;
            }
            return result;
        }
    }
}