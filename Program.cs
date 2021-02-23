using System;
using System.IO;

namespace LexicalAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("Please enter input file path and output file path as arguments.\n" +
                    "Output file is optional and by default it will be console output.\n");
            }
            if (args.Length > 2)
            {
                throw new ArgumentException("Incorrect number of arguments.\n" +
                    "Expected: one or two string arguments.\n" +
                    "Usage: LexicalAnalyzer.exe <input file> <output file>\n");
            }
            TextReader inputStream = new StreamReader(
                File.Exists(args[0])
                ? args[0]
                : throw new FileNotFoundException($"Input file {args[0]} was not found"));
            TextWriter outputStream = 
                args.Length == 2
                ? File.Exists(args[1])
                    ? new StreamWriter(args[1])
                    : throw new FileNotFoundException($"Output file {args[1]} was not found")
                : Console.Out;

            FileManager fileManager = new FileManager(inputStream, outputStream);
            NewAnalyzer analyzer = new NewAnalyzer(fileManager);
            analyzer.Analyze();
        }
    }
}
