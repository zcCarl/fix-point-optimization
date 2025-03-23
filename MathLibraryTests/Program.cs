using System;
using BenchmarkDotNet.Running;

namespace MathLibraryTests
{
    public class Program
    {
        // 使用xUnit的Main方法
        /*
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "benchmark")
            {
                Console.WriteLine("正在运行性能基准测试...");
                
                Console.WriteLine("\n运行定点数基准测试:");
                var fpSummary = BenchmarkRunner.Run<FixedPointBenchmarks>();
                
                Console.WriteLine("\n运行向量基准测试:");
                var vectorSummary = BenchmarkRunner.Run<VectorBenchmarks>();
                
                Console.WriteLine("\n运行矩阵基准测试:");
                var matrixSummary = BenchmarkRunner.Run<MatrixBenchmarks>();
            }
            else
            {
                Console.WriteLine("请运行单元测试或者使用 'dotnet run benchmark' 参数来运行性能基准测试");
            }
        }
        */

        // 提供单独的方法用于运行基准测试
        public static void RunBenchmarks()
        {
            Console.WriteLine("正在运行性能基准测试...");

            Console.WriteLine("\n运行定点数基准测试:");
            var fpSummary = BenchmarkRunner.Run<FixedPointBenchmarks>();

            Console.WriteLine("\n运行向量基准测试:");
            var vectorSummary = BenchmarkRunner.Run<VectorBenchmarks>();

            Console.WriteLine("\n运行矩阵基准测试:");
            var matrixSummary = BenchmarkRunner.Run<MatrixBenchmarks>();
        }
    }
}