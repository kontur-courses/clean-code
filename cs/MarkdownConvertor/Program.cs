using System;

namespace MarkdownConvertor
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var markdownConverter = new MarkdownConverter();
            Console.WriteLine(markdownConverter.ConvertMarkdownToHtml("__a\nb__"));
            Console.WriteLine(markdownConverter.ConvertMarkdownToHtml("abc\n- a\n- b\n- c\nabc _jn_ __oi__"));
        }
    }
}