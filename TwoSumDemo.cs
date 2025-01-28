using System;
using System.Collections.Generic;

namespace YourNamespace
{
    public class TwoSumDemo
    {
        private readonly Solution solution;

        public TwoSumDemo()
        {
            solution = new Solution();
        }

        public void RunAllTests()
        {
            RunBasicTest();
            RunFibonacciTest();
        }

        public void RunBasicTest()
        {
            int[] numbers = new int[] { 2, 7, 11, 15 };
            int target = 9;
            
            int[] result = solution.TwoSum(numbers, target);
            
            Console.WriteLine("=== Basic Test ===");
            Console.WriteLine($"Input Array: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Target Sum: {target}");
            Console.WriteLine($"Indices: [{string.Join(", ", result)}]");
            Console.WriteLine();
        }

        public void RunFibonacciTest()
        {
            int[] fibo = new int[] { 1, 2, 3, 5, 8, 13, 21 };
            int target_fibo = 34;

            int[] result_fibo = solution.TwoSum(fibo, target_fibo);
            
            Console.WriteLine("=== Fibonacci Test ===");
            Console.WriteLine($"Input Array: [{string.Join(", ", fibo)}]");
            Console.WriteLine($"Target Sum: {target_fibo}");
            Console.WriteLine($"Indices: [{string.Join(", ", result_fibo)}]");
            Console.WriteLine();
        }
    }
}