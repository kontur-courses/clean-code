using System;
using System.Collections.Generic;
using Markdown.Element;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var md = new Md(new HtmlElement("_", "em"), new HtmlElement("__", "strong"));
            var value = "fd _ffsdfd_ fsdfs";
            Console.WriteLine($"value :{value}");
            Console.WriteLine(md.Render(value));
        }
    }
}
