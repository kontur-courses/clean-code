using System.Collections.Generic;
using System.Text;
using Markdown.Interfaces;

namespace Markdown
{
    public class HtmlRender : ITranslatorFromMarkdown
    {
        private readonly Dictionary<Tags, (string Open, string Close)> tagsDictionary = new()
        {
            [Tags.Italic] = ("<em>", "</em>"),
            [Tags.Bold] = ("<strong>", "</strong>"),
            [Tags.Header] = ("<h1>", "</h1>")
        };

        public string Translate(List<ITag> tags)
        {
            var result = new StringBuilder();

            foreach (var tag in tags)
            {
                if (!tagsDictionary.ContainsKey(tag.Tag))
                    result.Append(tag.View);
                else
                {
                    result.Append(tag.TagType == TagType.Open
                        ? tagsDictionary[tag.Tag].Open
                        : (tagsDictionary[tag.Tag]).Close);
                }
            }

            return result.ToString();
        }
    }
}