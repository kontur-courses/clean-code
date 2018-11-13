using System;
using System.Collections.Generic;
using Markdown.Readers;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "_a _";

            SetupReaders.Setup();

            var readers = new List<IReader> {new SlashReader(), new StrongReader(), new EmReader(),  new CharReader()};
            var md = new Md(readers);
            Console.WriteLine(md.Render(input));
            Console.Read();
        }
    }
}
