using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var stringAccumulator = new StringBuilder();
            foreach (var line in text.Split('\n'))
            {
                stringAccumulator.Append(DeShield(RenderOneString(line)));
            }

            return stringAccumulator.ToString();
        }

        private string DeShield(string line)
        {
            var resultLine = new StringBuilder();
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] != '\\' || i == line.Length - 1 || (line[i + 1] != '_' && line[i + 1] != '#'))
                {
                    resultLine.Append(line[i]);
                }
            }

            return resultLine.ToString();
        }

        private string CollectOneString(Dictionary<int, Tag> tags, string line)
        {
            var result = new StringBuilder();
            var i = 0;

            while (i <= line.Length)
            {
                if (tags.ContainsKey(i))
                {
                    result.Append(tags[i].Value);
                    i += tags[i].Length;
                }
                else
                {
                    if (i < line.Length)
                    {
                        result.Append(line[i]);
                    }
                    i++;
                }
            }
            return result.ToString();
        }

        private string RenderOneString(string line)
        {
            var dict = 
                MarkdownParser.FilterTags(MarkdownParser.ReadAllTags(line), line.Length);
            return CollectOneString(dict, line);
        }
    }
}