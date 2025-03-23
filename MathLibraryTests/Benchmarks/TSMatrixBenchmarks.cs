using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Numerics;

namespace MathLibraryTests.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class TSMatrixBenchmarks
    {
        private const int IterationCount = 1000; // 矩阵操作较重，减少迭代次数

        // 输入值 - 3x3矩阵
        private readonly Matrix4x4[] _systemMatrices; // 使用4x4矩阵，但仅用3x3部分
        private readonly RVO.Arithmetic.TSMatrix[] _originalMatrices;
        private readonly RVO.Arithmetic.Optimized.TSMatrix[] _optimizedMatrices;

        // 向量，用于变换测试
        private readonly Vector3[] _systemVectors;
        private readonly RVO.Arithmetic.TSVector[] _originalVectors;
        private readonly RVO.Arithmetic.Optimized.TSVector[] _optimizedVectors;

        public TSMatrixBenchmarks()
        {
            // 初始化测试数据
            _systemMatrices = new Matrix4x4[IterationCount];
            _originalMatrices = new RVO.Arithmetic.TSMatrix[IterationCount];
            _optimizedMatrices = new RVO.Arithmetic.Optimized.TSMatrix[IterationCount];

            _systemVectors = new Vector3[IterationCount];
            _originalVectors = new RVO.Arithmetic.TSVector[IterationCount];
            _optimizedVectors = new RVO.Arithmetic.Optimized.TSVector[IterationCount];

            var random = new Random(42); // 固定种子以确保可重复性

            for (int i = 0; i < IterationCount; i++)
            {
                // 生成-10到10范围内的随机值作为矩阵元素
                float m11 = (float)((random.NextDouble() * 20) - 10);
                float m12 = (float)((random.NextDouble() * 20) - 10);
                float m13 = (float)((random.NextDouble() * 20) - 10);
                float m21 = (float)((random.NextDouble() * 20) - 10);
                float m22 = (float)((random.NextDouble() * 20) - 10);
                float m23 = (float)((random.NextDouble() * 20) - 10);
                float m31 = (float)((random.NextDouble() * 20) - 10);
                float m32 = (float)((random.NextDouble() * 20) - 10);
                float m33 = (float)((random.NextDouble() * 20) - 10);

                // 创建System.Numerics.Matrix4x4
                _systemMatrices[i] = new Matrix4x4(
                    m11, m12, m13, 0,
                    m21, m22, m23, 0,
                    m31, m32, m33, 0,
                    0, 0, 0, 1
                );

                // 创建TSMatrix
                _originalMatrices[i] = new RVO.Arithmetic.TSMatrix(
                    (RVO.Arithmetic.FP)m11,
                    (RVO.Arithmetic.FP)m12,
                    (RVO.Arithmetic.FP)m13,
                    (RVO.Arithmetic.FP)m21,
                    (RVO.Arithmetic.FP)m22,
                    (RVO.Arithmetic.FP)m23,
                    (RVO.Arithmetic.FP)m31,
                    (RVO.Arithmetic.FP)m32,
                    (RVO.Arithmetic.FP)m33
                );

                // 创建优化版TSMatrix
                _optimizedMatrices[i] = new RVO.Arithmetic.Optimized.TSMatrix(
                    (RVO.Arithmetic.Optimized.FP)m11,
                    (RVO.Arithmetic.Optimized.FP)m12,
                    (RVO.Arithmetic.Optimized.FP)m13,
                    (RVO.Arithmetic.Optimized.FP)m21,
                    (RVO.Arithmetic.Optimized.FP)m22,
                    (RVO.Arithmetic.Optimized.FP)m23,
                    (RVO.Arithmetic.Optimized.FP)m31,
                    (RVO.Arithmetic.Optimized.FP)m32,
                    (RVO.Arithmetic.Optimized.FP)m33
                );

                // 生成-100到100范围内的随机值作为向量元素
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
        public RVO.Arithmetic.TSMatrix MatrixMultiplication_OriginalTSMatrix()
        {
            RVO.Arithmetic.TSMatrix result = RVO.Arithmetic.TSMatrix.Identity;
            for (int i = 0; i < IterationCount - 1; i++)
            {
                result = result * _originalMatrices[i];
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSMatrix MatrixMultiplication_OptimizedTSMatrix()
        {
            RVO.Arithmetic.Optimized.TSMatrix result = RVO.Arithmetic.Optimized.TSMatrix.Identity;
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
        public RVO.Arithmetic.TSMatrix Inverse_OriginalTSMatrix()
        {
            RVO.Arithmetic.TSMatrix result = RVO.Arithmetic.TSMatrix.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * RVO.Arithmetic.TSMatrix.Inverse(_originalMatrices[i]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSMatrix Inverse_OptimizedTSMatrix()
        {
            RVO.Arithmetic.Optimized.TSMatrix result = RVO.Arithmetic.Optimized.TSMatrix.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * RVO.Arithmetic.Optimized.TSMatrix.Inverse(_optimizedMatrices[i]);
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
        public RVO.Arithmetic.TSMatrix Transpose_OriginalTSMatrix()
        {
            RVO.Arithmetic.TSMatrix result = RVO.Arithmetic.TSMatrix.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * RVO.Arithmetic.TSMatrix.Transpose(_originalMatrices[i]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSMatrix Transpose_OptimizedTSMatrix()
        {
            RVO.Arithmetic.Optimized.TSMatrix result = RVO.Arithmetic.Optimized.TSMatrix.Identity;
            for (int i = 0; i < IterationCount; i++)
            {
                result = result * RVO.Arithmetic.Optimized.TSMatrix.Transpose(_optimizedMatrices[i]);
            }
            return result;
        }

        // 矩阵向量变换
        [Benchmark]
        public Vector3 VectorTransform_SystemMatrix()
        {
            Vector3 result = Vector3.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += Vector3.Transform(_systemVectors[i], _systemMatrices[i]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.TSVector VectorTransform_OriginalTSMatrix()
        {
            RVO.Arithmetic.TSVector result = RVO.Arithmetic.TSVector.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += RVO.Arithmetic.TSVector.Transform(_originalVectors[i], _originalMatrices[i]);
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.TSVector VectorTransform_OptimizedTSMatrix()
        {
            RVO.Arithmetic.Optimized.TSVector result = RVO.Arithmetic.Optimized.TSVector.zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += RVO.Arithmetic.Optimized.TSVector.Transform(_optimizedVectors[i], _optimizedMatrices[i]);
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
                result += _originalMatrices[i].Determinant();
            }
            return result;
        }

        [Benchmark]
        public RVO.Arithmetic.Optimized.FP Determinant_OptimizedTSMatrix()
        {
            RVO.Arithmetic.Optimized.FP result = RVO.Arithmetic.Optimized.FP.Zero;
            for (int i = 0; i < IterationCount; i++)
            {
                result += _optimizedMatrices[i].Determinant();
            }
            return result;
        }
    }
}