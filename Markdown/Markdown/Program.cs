using System;
using System.Collections.Generic;
using Markdown.Tag;

namespace Markdown
{
    internal class Program
    {
        private static void Main()
        {
            var dictionaryTags = new Dictionary<string, ITag>
            {
                {"_", new SingleUnderLineTag()},
                {"__", new DoubleUnderLineTag()},
                {"#", new SharpTag()}
            };

            var md = new Md(dictionaryTags);
            Console.WriteLine(md.Render("__a _b_ c__"));
        }
    }
}