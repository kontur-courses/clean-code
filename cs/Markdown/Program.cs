using System.IO;

namespace Markdown
{
    public static class Program
    {
        /// <summary>
        /// Implementation of an algorithm for converting md-document into html-document.
        /// </summary>
        /// <param name="inputFile">Path to md-document</param>
        /// <param name="outputFile">Path to result html-document. It will be created if necessary.</param>
        private static void Main(string inputFile = "./MarkdownSpec.md", string outputFile = "./MarkdownSpec.html")
        {
            var source = File.ReadAllText(inputFile);
            var result = MarkdownEngine.Render(source);
            File.WriteAllText(outputFile, result);
        }
    }
}