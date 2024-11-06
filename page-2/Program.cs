using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Get the base directory of the project & Define the Input/Output Paths
        // ::: Pick the way, most suitable for your environment 🦝

        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        string inputPath = Path.Combine(projectDirectory, "input.txt");
        string outputPath = Path.Combine(projectDirectory, "output.txt");

        try
        {
            // Read input file, parse it, and prepare data for processing
            var lines = File.ReadAllLines(inputPath);
            int n = int.Parse(lines[0]);
            var blocks = lines.Skip(1).Select(line => line.Split(' ').Select(int.Parse).ToArray()).ToArray();

            // Calculate minimum operations needed and write the result to output file
            int minOperations = CalculateMinOperations(blocks, n);
            File.WriteAllText(outputPath, minOperations.ToString());

            // End Message of Success ☺
            Console.WriteLine($"Min Operations required: {minOperations}\n∟ Result added to output.txt\n∟ {Path.Combine(projectDirectory, "output.txt")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    static int CalculateMinOperations(int[][] blocks, int n)
    {
        // Initialize arrays to store specific values from blocks for easier access
        int[] m = new int[n];
        int[] k = new int[n];
        for (int i = 0; i < n; i++)
        {
            m[i] = blocks[i][0];
            k[i] = blocks[i][1];
        }

        // Initialize the dp array to hold minimum operation costs for pairs of blocks
        int[,] dp = new int[n, n];

        // Pre-fill dp array with multiplication costs for adjacent block pairs
        for (int i = 0; i < n - 1; i++)
        {
            dp[i, i + 1] = m[i] * k[i + 1];
        }

        // Fill dp table using bottom-up approach
        for (int len = 3; len <= n; len++)
        {
            for (int i = 0; i <= n - len; i++)
            {
                int j = i + len - 1;
                dp[i, j] = int.MaxValue;
                for (int l = i; l < j; l++)
                {
                    int cost = dp[i, l] + dp[l + 1, j] + m[i] * k[j];
                    if (cost < dp[i, j])
                    {
                        dp[i, j] = cost;
                    }
                }
            }
        }

        return dp[0, n - 1];
    }
}