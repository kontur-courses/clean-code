using System;

namespace Markdown
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var translator = new Translator('\\',new MarkHtml("__", "<strong>"), new MarkHtml("_", "<em>"));
            Console.WriteLine(translator.Translate("__a _b_ c__"));
        }
    }
}
