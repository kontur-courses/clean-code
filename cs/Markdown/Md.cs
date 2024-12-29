using Markdown.Tags;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public static readonly IReadOnlyDictionary<char, Dictionary<string, Func<string, int, Tag>>> MdTags =
           new Dictionary<char, Dictionary<string, Func<string, int, Tag>>>()
           {
                {
                    '_', new Dictionary<string, Func<string, int, Tag>>
                    {
                        { "_", (markdown, tagStart) => new Italic(markdown, tagStart)},
                        { "__", (markdown, tagStart) => new Bold(markdown, tagStart)}
                    }
                },
                {
                    '#', new Dictionary<string, Func<string, int, Tag>>
                    {
                        { "#", (markdown, tagStart) => new Header(markdown, tagStart) }
                    }
                },
                {
                    '\\',
                    new Dictionary<string, Func<string, int, Tag>>
                    {
                        { "\\", (markdown, tagStart) => new Escape(markdown, tagStart) }
                    }
                }
           };

        public string Render(string markdownText)
        {
            var parser = new MdParser(markdownText, MdTags);
            var tags = parser.GetTags();
            var result = new StringBuilder();
            var tagsStart = tags.ToDictionary(t => t.TagStart);
            for (var i = 0; i < markdownText.Length;)
            {
                if (tagsStart.ContainsKey(i))
                {
                    result.Append(tagsStart[i].RenderToHtml());
                    i = tagsStart[i].TagEnd++;
                }
                else
                {
                    result.Append(markdownText[i]);
                    i++;
                }
            }
            return result.ToString();
        }
    }
}