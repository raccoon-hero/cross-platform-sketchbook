namespace KPCore4
{
    public class KPCore4
    {
        public string inputPath { get; set; }
        public string outputPath { get; set; }

        public KPCore4(string inputFileName = "../../../input.txt", string outputFileName = "../../../output.txt")
        {           
            this.inputPath = inputFileName;
            this.outputPath = outputFileName;
        }

        #region Lab1Instructions
        public void Lab1StartProcess()
        {
            try
            {
                // Read and parse the input data
                string[] inputLines = File.ReadAllLines(inputPath);
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
                File.WriteAllText(outputPath, maxReward.ToString());

                // End Message of Success ☺
                Console.WriteLine($"Max Reward would be: {maxReward}\n∟ Result added to output.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
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
        #endregion

        #region Lab2Instructions
        public void Lab2StartProcess()
        {

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
                Console.WriteLine($"Min Operations required: {minOperations}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public static int CalculateMinOperations(int[][] blocks, int n)
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
        #endregion

        #region Lab3Instructions
        public void Lab3StartProcess()
        {

            try
            {
                // Reading & processing the matrix
                int[,] matrix = ReadMatrixFromFile(inputPath);
                int[,] updatedMatrix = ReplaceZeros(matrix);

                // Writing the updated matrix to output file
                WriteMatrixToFile(updatedMatrix, outputPath);

                // End Message of Success ☺
                PrintMatrix(updatedMatrix);
                Console.WriteLine($"\n∟ Result added to output.txt");
            }
            catch (Exception ex)
            {
                // Error handling in case of any exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static int[,] ReadMatrixFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            int size = int.Parse(lines[0]);
            if (size > 200) throw new ArgumentException("Matrix size exceeds 200.");

            int[,] matrix = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                var row = lines[i + 1].Split(' ').Select(int.Parse).ToArray();
                if (row.Length != size) throw new ArgumentException("Matrix row size mismatch.");

                for (int j = 0; j < size; j++)
                {
                    if (row[j] < 0 || row[j] > 106) throw new ArgumentException("Matrix element out of range.");
                    matrix[i, j] = row[j];
                }
            }
            return matrix;
        }

        public static int[,] ReplaceZeros(int[,] matrix)
        {
            int size = matrix.GetLength(0);
            int[,] result = (int[,])matrix.Clone();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    // Check and replace zero elements
                    if (matrix[i, j] == 0)
                    {
                        int nearestValue = FindNearestNonZero(matrix, i, j);
                        if (nearestValue != -1) result[i, j] = nearestValue;
                    }
                }
            }

            return result;
        }

        private static int FindNearestNonZero(int[,] matrix, int x, int y)
        {
            int size = matrix.GetLength(0);
            int nearestValue = -1;
            int minDistance = int.MaxValue;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j] != 0)
                    {
                        // Calculating the distance and updating nearest value
                        int distance = Math.Abs(i - x) + Math.Abs(j - y);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestValue = matrix[i, j];
                        }
                        else if (distance == minDistance && nearestValue != matrix[i, j])
                        {
                            return -1; // Multiple nearest values found
                        }
                    }
                }
            }
            return nearestValue;
        }

        public static void WriteMatrixToFile(int[,] matrix, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        writer.Write(matrix[i, j] + (j == matrix.GetLength(1) - 1 ? "" : " "));
                    }
                    writer.WriteLine();
                }
            }
        }

        public static void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}