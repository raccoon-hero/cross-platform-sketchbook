using System;
using System.IO;
using System.Linq;

class AntonOrders
{
    static void Main(string[] args)
    {
        // Get the base directory of the project & Define the Input/Output Paths
        // ::: Pick the way, most suitable for your environment 🦝

        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        string inputFilePath = Path.Combine(projectDirectory, "input.txt");
        string outputFilePath = Path.Combine(projectDirectory, "output.txt");

        try
        {
            // Read and parse the input data
            string[] inputLines = File.ReadAllLines(inputFilePath);
            int N = int.Parse(inputLines[0]);

            if (N < 1 || N > 1000)
                throw new Exception("The number of orders N must be between 1 and 1000.");

            int[] deadlines = new int[N];
            int[] rewards = new int[N];

            for (int i = 0; i < N; i++)
            {
                string[] orderData = inputLines[i + 1].Split(' ');
                int T = int.Parse(orderData[0]);
                int C = int.Parse(orderData[1]);

                if (T <= 0 || C <= 0 || T > 105 || C > 105)
                    throw new Exception("Order data T and C must be natural numbers & not exceeding 105.");

                deadlines[i] = T;
                rewards[i] = C;
            }

            // Process the orders to maximize the reward
            int maxReward = CalculateMaxReward(N, deadlines, rewards);

            // Write the output data
            File.WriteAllText(outputFilePath, maxReward.ToString());

            // End Message of Success ☺
            Console.WriteLine($"Max Reward would be: {maxReward}\n∟ Result added to output.txt\n∟ {Path.Combine(projectDirectory, "output.txt")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static int CalculateMaxReward(int N, int[] deadlines, int[] rewards)
    {
        // Pair deadlines with rewards and sort by deadline then by reward descending
        var orders = deadlines.Zip(rewards, (t, c) => new { Deadline = t, Reward = c })
                              .OrderByDescending(order => order.Reward)
                              .ThenBy(order => order.Deadline).ToList();

        bool[] daysTaken = new bool[N];
        int totalReward = 0;

        // Greedily select the orders to maximize reward
        foreach (var order in orders)
        {
            int daysToComplete = Math.Min(order.Deadline, N) - 1;
            while (daysToComplete >= 0)
            {
                if (!daysTaken[daysToComplete])
                {
                    daysTaken[daysToComplete] = true;
                    totalReward += order.Reward;
                    break;
                }
                daysToComplete--;
            }
        }

        return totalReward;
    }
}
