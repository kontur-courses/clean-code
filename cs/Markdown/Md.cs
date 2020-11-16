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

        private string DeShield(string paragraphs)
        {
            var resultParagraph = new StringBuilder();
            for (var i = 0; i < paragraphs.Length; i++)
            {
                if (paragraphs[i] != '\\' || i == paragraphs.Length - 1 || (paragraphs[i + 1] != '_' && paragraphs[i + 1] != '#'))
                {
                    resultParagraph.Append(paragraphs[i]);
                }
            }

            return resultParagraph.ToString();
        }

        private string CollectOneString(Dictionary<int, Tag> tags, string paragraph)
        {
            var result = new StringBuilder();
            var i = 0;

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
                    if (i < paragraph.Length)
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