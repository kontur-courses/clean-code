using System;
using Markdown.ParserClasses;
using Markdown.TokenizerClasses;
using Markdown.HTMLGeneratorClasses;
using Fclp;

namespace Markdown
{
    public class Md
    {
        public static void Main(string[] args)
        {
            var markdownText = "";

            var parser = new FluentCommandLineParser();
            parser.Setup<string>('t', "markdownText")
                .Callback(arg => markdownText = arg)
                .Required();
            parser.Parse(args);

            var htmlText = Render(markdownText);

            Console.WriteLine(htmlText);
            Console.ReadKey();
        }

        public static string Render(string markdownText)
        {
            var tokenizer = new Tokenizer();
            var parser = new Parser();
            var generator = new HTMLGenerator();

            var tokens = tokenizer.Tokenize(markdownText);
            var abstractSyntaxTree = parser.Parse(tokens);
            var htmlText = generator.Generate(abstractSyntaxTree);

            return htmlText;
        }
    }
}
