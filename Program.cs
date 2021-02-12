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
                throw new ArgumentException("Please enter input file path as an argument");
            }
            if (args.Length > 1)
            {
                throw new ArgumentException("Incorrect number of arguments. \n Expected: one string argument");
            }

            StreamReader inputFile = new StreamReader(args[0]);
            FileManager fileManager = new FileManager(inputFile, Console.Out);
            Analyzer analyzer = new Analyzer(fileManager);
            analyzer.Analyze();
        }
    }
}
