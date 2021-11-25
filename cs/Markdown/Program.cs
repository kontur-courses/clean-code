using System;
using System.Linq;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = "# Заголовок __с _разными_ символами__";
            var converter = new TokenConverter();
            var tokens = converter.Initialize(source)
                .FindTokens()
                .GetTokens().ToList();
            Console.WriteLine(converter.Build());
        }
    }
}