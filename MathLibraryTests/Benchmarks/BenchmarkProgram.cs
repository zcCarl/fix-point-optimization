using BenchmarkDotNet.Running;
using System;

namespace MathLibraryTests.Benchmarks
{
    public class BenchmarkProgram
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("开始运行数学库性能测试...");
            Console.WriteLine("请选择要运行的测试：");
            Console.WriteLine("1. Fix64固定点数 性能测试");
            Console.WriteLine("2. TSVector三维向量 性能测试");
            Console.WriteLine("3. TSVector2二维向量 性能测试");
            Console.WriteLine("4. TSVector4四维向量 性能测试");
            Console.WriteLine("5. TSMatrix 3x3矩阵 性能测试");
            Console.WriteLine("6. TSMatrix4x4 4x4矩阵 性能测试");
            Console.WriteLine("7. TSQuaternion四元数 性能测试");
            Console.WriteLine("0. 运行所有测试");
            Console.WriteLine();

            string input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    RunAllBenchmarks();
                    break;
                case "1":
                    BenchmarkRunner.Run<Fix64Benchmarks>();
                    break;
                case "2":
                    BenchmarkRunner.Run<TSVectorBenchmarks>();
                    break;
                case "3":
                    BenchmarkRunner.Run<TSVector2Benchmarks>();
                    break;
                case "4":
                    // 这里可以添加TSVector4性能测试
                    Console.WriteLine("TSVector4性能测试尚未实现");
                    break;
                case "5":
                    BenchmarkRunner.Run<TSMatrixBenchmarks>();
                    break;
                case "6":
                    BenchmarkRunner.Run<TSMatrix4x4Benchmarks>();
                    break;
                case "7":
                    BenchmarkRunner.Run<TSQuaternionBenchmarks>();
                    break;
                default:
                    Console.WriteLine("无效的选择");
                    break;
            }

            Console.WriteLine("性能测试完成！");
            Console.ReadLine();
        }

        private static void RunAllBenchmarks()
        {
            Console.WriteLine("开始运行所有性能测试...");

            Console.WriteLine("=== Fix64固定点数 性能测试 ===");
            BenchmarkRunner.Run<Fix64Benchmarks>();

            Console.WriteLine("=== TSVector三维向量 性能测试 ===");
            BenchmarkRunner.Run<TSVectorBenchmarks>();

            Console.WriteLine("=== TSVector2二维向量 性能测试 ===");
            BenchmarkRunner.Run<TSVector2Benchmarks>();

            Console.WriteLine("=== TSMatrix 3x3矩阵 性能测试 ===");
            BenchmarkRunner.Run<TSMatrixBenchmarks>();

            Console.WriteLine("=== TSMatrix4x4 4x4矩阵 性能测试 ===");
            BenchmarkRunner.Run<TSMatrix4x4Benchmarks>();

            Console.WriteLine("=== TSQuaternion四元数 性能测试 ===");
            BenchmarkRunner.Run<TSQuaternionBenchmarks>();
        }
    }
}