using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Markdown
{
    class Program
    {
        public static void Main(string[] args)
        {
            var command = string.Join(" ", args);
            var (inputFilename, outputFilename) = ParseArgs(command);
            var markdown = File.ReadAllText(inputFilename);
            // TODO: Добавить преобразование markdown в html
            var result = markdown;

            using (var sw = new StreamWriter(outputFilename))
                sw.WriteLine(result);
        }

        private static (string pathFrom, string pathTo) ParseArgs(string command)
        {
            var pattern = new Regex("^-i (.*?) -o (.*?)$");
            var matches = pattern.Matches(command);
            if (matches.Count != 1)
                throw new ArgumentException(
                    "Invalid Command. Command should be '-i [path to input file] -o [path to output file]'");
            Console.WriteLine(matches[0].Groups[1].Value);
            Console.WriteLine(matches[0].Groups[2].Value);
            return (matches[0].Groups[1].Value, matches[0].Groups[2].Value);
        }
    }
}