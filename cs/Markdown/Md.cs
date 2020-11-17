using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tags;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            return string.Join("\n", text.Split('\n')
                .Select(RenderOneString)
                .Select(DeShield));
        }

        private string DeShield(string paragraph)
        {
            var resultParagraph = new StringBuilder();
            for (var i = 0; i < paragraph.Length; i++)
            {
                if (paragraph[i] != '\\' || i == paragraph.Length - 1 || (paragraph[i + 1] != '_' && paragraph[i + 1] != '#'))
                {
                    resultParagraph.Append(paragraph[i]);
                }
            }

            return resultParagraph.ToString();
        }

        private string CollectOneString(Dictionary<int, Tag> tags, string paragraph)
        {
            var result = new StringBuilder();
            var i = -1;

            while (i <= paragraph.Length)
            {
                if (tags.ContainsKey(i))
                {
                    result.Append(tags[i].Value);
                    var length = tags[i].Length;
                    tags.Remove(i);
                    i += length;
                }
                else
                {
                    if (i >= 0 && i < paragraph.Length)
                    {
                        result.Append(paragraph[i]);
                    }
                    i++;
                }
            }
            return result.ToString();
        }

        private string RenderOneString(string paragraph)
        {
            var allTags = MarkdownParser.ParseAllTags(paragraph);
            var filteredTagsDictionary = MarkdownFilter.FilterTags(allTags, paragraph.Length);
            return CollectOneString(filteredTagsDictionary, paragraph);
        }
    }
}