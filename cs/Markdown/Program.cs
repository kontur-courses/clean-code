using System;
using System.IO;
using Fclp;

namespace Markdown
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parser = SetupParser();

            var parseResult = parser.Parse(args);
            if (!parseResult.HasErrors)
            {
                var processor = MarkdownProcessorFactory.Create();
                var markdownText = File.ReadAllText(parser.Object.InputFilePath);
                var html = processor.RenderToHtml(markdownText);
                File.WriteAllText(parser.Object.OutputFilePath, html);
            }
            else
            {
                Console.WriteLine(parseResult.ErrorText);
            }
        }

        private static FluentCommandLineParser<ApplicationArguments> SetupParser()
        {
            var parser = new FluentCommandLineParser<ApplicationArguments>();

            parser.Setup(arg => arg.InputFilePath).As('i', "input").Required();
            parser.Setup(arg => arg.OutputFilePath).As('o', "output").Required();
            return parser;
        }
    }

    public class ApplicationArguments
    {
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }
    }
}