using System;
using System.IO;

namespace Markdown
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Enter full path to file to convert:");
            var filePath = Console.ReadLine();
            var pathToSave = Path.Combine(
                Path.GetDirectoryName(filePath),
                Path.GetFileNameWithoutExtension(filePath) + ".html");

            using (var sr = new StreamReader(filePath))
            using (var sw = new StreamWriter(pathToSave))
            {
                var fileContents = sr.ReadToEnd();
                sw.Write(Markdown.Render(fileContents));
            }

            Console.WriteLine("Conversion Complete");
            Console.ReadKey();
        }
    }
}
