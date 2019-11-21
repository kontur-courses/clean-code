using System;
using System.IO;

namespace Markdown
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var md = new Md();
            using (var reader = new StreamReader(Path.Combine("..", "..", "MarkDownSpec.md" )))
            {
                var content = reader.ReadToEnd();
                var paragraphs = content.Split(new []{"\r\n\r\n"}, StringSplitOptions.None);
                using (var writer = new StreamWriter(Path.Combine("..", "..", "SpecResult.html")))
                {
                    foreach (var paragraph in paragraphs)
                    { 
                        writer.WriteLine(md.Render(paragraph));
                    }
                }
            }
        }
    }
}
