using System;
using System.IO;
using System.Linq;
using KP;

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
            // Reading & processing the matrix
            int[,] matrix = MatrixProcessingCRT3lib.ReadMatrixFromFile(inputPath);
            int[,] updatedMatrix = MatrixProcessingCRT3lib.ReplaceZeros(matrix);

            // Writing the updated matrix to output file
            MatrixProcessingCRT3lib.WriteMatrixToFile(updatedMatrix, outputPath);

            // End Message of Success ☺
            MatrixProcessingCRT3lib.PrintMatrix(updatedMatrix);
            Console.WriteLine($"\n∟ Result added to output.txt\n∟ {Path.Combine(projectDirectory, "output.txt")}");
        }
        catch (Exception ex)
        {
            // Error handling in case of any exceptions
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
