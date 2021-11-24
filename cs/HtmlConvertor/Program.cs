using System;

namespace HtmlConvertor
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var htmlConvertor = new HtmlConvertor();
            Console.WriteLine(htmlConvertor.ConvertMarkdownToHtml("__abc__"));
        }
    }
}