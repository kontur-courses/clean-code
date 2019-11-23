using System;

namespace MarkdownProcessor
{
    internal static class Program
    {
        public static void Main()
        {
            // ReSharper disable once StringLiteralTypo
            const string markdownText = @"Текст _окруженный с двух сторон_  одинарными символами подчерка 
должен помещаться в HTML-тег em вот так:";

            var result = Markdown.RenderHtml(markdownText);

            Console.WriteLine(result);

            /*
             Output:
             
             Текст <em>окруженный с двух сторон</em>  одинарными символами подчерка
             должен помещаться в HTML-тег em вот так:
             
             */
        }
    }
}