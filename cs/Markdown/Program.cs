using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = "_a__";
            var converter = new TokenConverter();
            var result = converter.SetMarkupString(source)
                .FindTokens()
                .Build();
            Console.WriteLine(result);
        }
    }
}