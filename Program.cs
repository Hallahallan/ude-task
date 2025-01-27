namespace TwoSumProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Solution solution = new Solution();
            
            // Test case
            int[] numbers = new int[] { 2, 7, 11, 15 };
            int target = 9;
            
            int[] result = solution.TwoSum(numbers, target);
            
            Console.WriteLine($"Input Array: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Target Sum: {target}");
            Console.WriteLine($"Indices: [{string.Join(", ", result)}]");

            //Test case 2
            int[] fibo = new int[] { 1, 2, 3, 5, 8, 13, 21 };
            int target_fibo = 34;

            int[] result_fibo = solution.TwoSum(fibo, target_fibo);
            
            Console.WriteLine($"Input Array: [{string.Join(", ", fibo)}]");
            Console.WriteLine($"Target Sum: {target_fibo}");
            Console.WriteLine($"Indices: [{string.Join(", ", result_fibo)}]");
        }
    }
}