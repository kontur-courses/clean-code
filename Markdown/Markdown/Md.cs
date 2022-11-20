using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        public static string Render(string text)
        {
            string[] line = text.Split('\n');
            List<string> convertedLines = new List<string>();
            Md md = new Md();
            for (int i = 0; i < line.Length; i++)
            {
                convertedLines.Add(md.GetConvertedLine(line[i]));
            }

            return String.Join('\n', convertedLines);
        }

        private string GetConvertedLine(string line)
        {
            this.Line = line;
            return HTMLMarkdownConverter.ConvertMarkdownToHTML(CreateMarkDown(), line);
        }

        private string Line;

        private List<MarkdownAction> CreateMarkDown()
        {
            throw new NotImplementedException();
        }
    }
}