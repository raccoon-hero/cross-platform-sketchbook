namespace KP
{
    public class MatrixProcessingCRT3lib
    {
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
    }
}