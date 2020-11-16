using System.Collections.Generic;
using System.Text;
using Markdown.Tags;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var stringAccumulator = new StringBuilder();
            var paragraphes = text.Split('\n');
            for (var i = 0; i < paragraphes.Length; i++)
            {
                stringAccumulator.Append(DeShield(RenderOneString(paragraphes[i])));
                if (i != paragraphes.Length - 1)
                {
                    stringAccumulator.Append("\n");
                }
            }
            
            return stringAccumulator.ToString();
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
                    i += tags[i].Length;
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