using System;
using System.IO;
using System.Text;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = new StringBuilder();
            var md = new Md();
            var data = "";
            try
            {
                data = File.ReadAllText(@"examples.md");
            }
            catch (IOException e)
            {
                Console.Error.WriteLine("Something went wrong! Check the correctness of file path.", e);
            }
            var mdParagraph = new StringBuilder();

            foreach (var mdLine in data.Split('\n'))
            {
                if (mdLine.Length > 0 && mdLine != "\r")
                    mdParagraph.Append(mdLine);
                else
                {
                    result.Append(md.Render(mdParagraph.ToString()) + "\n\n");
                    mdParagraph.Clear();
                }
            }

            result.Remove(result.Length - 1, 1);

            using (StreamWriter writer = new StreamWriter(@"examples.html"))
            {
                writer.Write(result);
            }
        }
    }
}
