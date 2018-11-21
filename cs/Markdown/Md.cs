using System;
using Markdown.ParserClasses;
using Markdown.TokenizerClasses;
using Markdown.HTMLGeneratorClasses;

namespace Markdown
{
    public class Md
    {
        public static void Main(string[] args)
        {
            var markdownText = "__Foo__ _bar_ text";
            var htmlText = Render(markdownText);

            Console.WriteLine(htmlText);
            Console.ReadKey();
        }

        private static string Render(string markdownText)
        {
            var tokenizer = new Tokenizer();
//            var parser = new Parser();
//            var generator = new HTMLGenerator();

//            var tokens = tokenizer.Tokenize(markdownText);
//            var abstractSyntaxTree = parser.Parse(tokens);
//            var htmlText = generator.Generate(abstractSyntaxTree);
//
//            return htmlText;

            throw new NotImplementedException();
        }
    }
}
