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
            using (StreamReader reader = new StreamReader(@"examples.md"))
            {
                while (!reader.EndOfStream)
                {
                    var mdParagraph = new StringBuilder();
                    var currentString = reader.ReadLine();

                    while (currentString.Length != 0)
                    {
                        mdParagraph.Append(currentString);
                        if (!reader.EndOfStream)
                            currentString = reader.ReadLine();
                        else
                            break;
                    }
                    result.Append(md.Render(mdParagraph.ToString()));
                    if (reader.EndOfStream)
                        result.Append("\n");
                    else
                        result.Append("\n\n");
                    mdParagraph.Clear();
                }
            }

            using (StreamWriter writer = new StreamWriter(@"examples.html"))
            {
                writer.Write(result);
            }
        }
    }
}
