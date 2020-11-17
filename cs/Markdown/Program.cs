using System;
using System.IO;

namespace Markdown
{
    public static class Program
    {
        private static void Main(string inputFile = "MarkdownSpec.md", string outputFile = "MarkdownSpec.html")
        {
            var source = File.ReadAllText(inputFile);
            var result = MarkdownEngine.Render(source);
            File.WriteAllText(outputFile, result);
        }
    }
}