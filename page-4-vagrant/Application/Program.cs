using System;
using KPCore4;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("CROSS_Task4 Started. Type 'exit' to quit.\n");
        bool exitRequested = false;

        while (!exitRequested)
        {
            Console.Write("> [Enter command]: ");
            string input = Console.ReadLine();
            string[] commandArgs = input?.Split(' ');

            if (commandArgs.Length > 0 && commandArgs[0].ToLower() == "exit")
            {
                exitRequested = true;
            }
            else
            {
                ProcessCommand(commandArgs);
            }
        }

        Console.WriteLine("CROSS_Task4 Exited.");
    }

    static void ProcessCommand(string[] args)
    {
        if (args.Length == 0 || args[0].ToLower() == "help")
        {
            ShowUsage();
            return;
        }

        switch (args[0].ToLower())
        {
            case "version":
                ShowVersion();
                break;

            case "run":
                if (args.Length < 2)
                {
                    Console.WriteLine("Error: 'run' command requires a lab number (lab1, lab2, lab3).");
                }
                else
                {
                    RunLabTask(args[1], args);
                }
                break;

            case "set-path":
                SetLabPath(args);
                break;

            default:
                Console.WriteLine("Invalid command. Type 'help' for usage.");
                break;
        }
    }

    static void ShowVersion()
    {
        Console.WriteLine("CROSS_Task4:\n  Author: K. Petrachyk\n  Version: 0.6.4\n  Packages Required: KPCore4.2+");
    }

    static void RunLabTask(string lab, string[] args)
    {
        string inputFile = null, outputFile = null;

        for (int i = 2; i < args.Length; i++)
        {
            if (args[i] == "-i" || args[i] == "--input")
            {
                inputFile = args[++i];
            }
            else if (args[i] == "-o" || args[i] == "--output")
            {
                outputFile = args[++i];
            }
        }

        // Check LAB_PATH if input or output is not set
        string labPath = Environment.GetEnvironmentVariable("LAB_PATH");
        if (string.IsNullOrEmpty(inputFile) && !string.IsNullOrEmpty(labPath))
        {
            inputFile = Path.Combine(labPath, "input.txt");
        }

        if (string.IsNullOrEmpty(outputFile) && !string.IsNullOrEmpty(labPath))
        {
            outputFile = Path.Combine(labPath, "output.txt");
        }

        // Use user's home directory if neither is set
        if (string.IsNullOrEmpty(inputFile))
        {
            inputFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "input.txt");
        }

        if (string.IsNullOrEmpty(outputFile))
        {
            outputFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "output.txt");
        }

        KPCore4.KPCore4 kPCore4 = new KPCore4.KPCore4(inputFile, outputFile);

        switch (lab.ToLower())
        {
            case "lab1":
                kPCore4.Lab1StartProcess();
                break;
            case "lab2":
                kPCore4.Lab2StartProcess();
                break;
            case "lab3":
                Console.WriteLine("Result:\n");
                kPCore4.Lab3StartProcess();
                break;
            default:
                Console.WriteLine("Invalid lab number. Choose 'lab1', 'lab2', or 'lab3'.");
                break;
        }
    }

    static void SetLabPath(string[] args)
    {
        // Check if there are enough arguments
        if (args.Length < 2)
        {
            Console.WriteLine("Error: 'set-path' command requires a path.");
            return;
        }

        // Checking for the 'set-path' command and the flags '-p' or '--path'
        if (args[0].ToLower() == "set-path" && (args[1] == "-p" || args[1] == "--path"))
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Error: No path provided.");
                return;
            }

            Environment.SetEnvironmentVariable("LAB_PATH", args[2]);
            Console.WriteLine($"LAB_PATH set to {args[2]}");
        }
        else
        {
            Console.WriteLine("Error: Invalid command format.");
        }
    }


    static void ShowUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  version: Show version information.");
        Console.WriteLine("  run [lab1|lab2|lab3] [-i|--input] <inputFilePath> [-o|--output] <outputFilePath>: Run a specific lab task.");
        Console.WriteLine("  set-path [-p|--path] <path>: Set the LAB_PATH environment variable.");
        Console.WriteLine("  exit: Exit the application.");
    }
}
