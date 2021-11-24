using System;
using System.Linq;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = "_\\_a_";
            var converter = new TokenConverter();
            var tokens = converter.SetMarkupString(source)
                .FindTokens()
                .GetTokens().ToList();
            Console.WriteLine(converter.Build());
        }
    }
}