using System;
using System.IO;
using Markdown;

namespace MarkdownUsageExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent;
            var mdFile = new StreamReader($"{currentDirectory.FullName}\\MarkdownSpec.md");
            
            var mdToHtml = new Md();
            using (var htmlWriter = new StreamWriter($"{currentDirectory.FullName}\\MarkdownSpec.html"))
            {
                htmlWriter.Write(mdToHtml.Render(mdFile.ReadToEnd()));
            }
        }
    }
}