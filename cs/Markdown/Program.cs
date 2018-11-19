using System;
using CommandLine;

namespace Markdown
{
    class Program
    {
        public class Options
        {
            [Value(0, Default = "", MetaName = "text", HelpText = "Text to be converted to HTML")]
            public string Text { get; set; }
        }

        static void Main(string[] args)
        {
            var md = new Md();

            var result = Parser.Default.ParseArguments<Options>(args);
            result.WithParsed(options =>
            {
                var renderedText = md.Render(options.Text);
                Console.WriteLine(renderedText);
            });
        }
    }
}
