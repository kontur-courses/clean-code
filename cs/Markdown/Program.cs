using System;
using System.Collections.Generic;
using System.IO;
using Markdown.Core;
using Markdown.Core.Infrastructure;
using Markdown.Core.Normalizer;
using NDesk.Options;

namespace Markdown
{
    public class Program
    {
        static void Main(string[] args)
        {
            var showHelp = false;
            string inputFilename = null;
            string outputFilename = null;
            var optionSet = new OptionSet()
            {
                {"i|input=", "Path to input file", path => inputFilename = path},
                {"o|output=", "Path to output file", path => outputFilename = path},
                {"h|help", "Show help message", arg => showHelp = arg != null},
            };

            optionSet.Parse(args);
            if (showHelp)
            {
                ShowHelp(optionSet);
                return;
            }

            if (inputFilename == null || outputFilename == null)
            {
                Console.WriteLine("Try `Markdown.exe --help' for more information.");
                return;
            }

            if (!File.Exists(inputFilename))
            {
                Console.WriteLine("Input file does not exist");
                return;
            }

            var markdownText = File.ReadAllText(inputFilename);
            File.WriteAllText(outputFilename,
                new MdRenderer().Render(markdownText, StandardIgnoreRules.IgnoreInsideRules));
        }

        private static void ShowHelp(OptionSet optionSet)
        {
            Console.WriteLine("Usage: Markdown.exe [OPTIONS]");
            Console.WriteLine("Translates markdown paragraph to HTML file.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            optionSet.WriteOptionDescriptions(Console.Out);
        }
    }
}