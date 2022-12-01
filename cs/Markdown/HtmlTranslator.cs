using System.Collections.Generic;
using System.Text;
using Markdown.Interfaces;

namespace Markdown
{
    public class HtmlTranslator : ITranslator
    {
        private readonly Dictionary<Tag, (string Open, string Close)> htmlTagsDictionary = new()
        {
            [Tag.Italic] = ("<em>", "</em>"),
            [Tag.Bold] = ("<strong>", "</strong>"),
            [Tag.Header] = ("<h1>", "</h1>")
        };

        private readonly Dictionary<Tag, string> singleTagFormat = new()
        {
            [Tag.Image] = "<img href='{0}' alt='{1}'>{2}",
            [Tag.Link] = "<a href='{0}' alt='{1}'>{2}</a>"
        };

        public string Translate(IEnumerable<ITag> tags)
        {
            var result = new StringBuilder();

            foreach (var tag in tags)
                if (tag.TagType == TagType.Single)
                {
                    var l = (ILinkTag)tag;
                    result.Append(string.Format(singleTagFormat[tag.Tag], l.Path, l.Alternative, l.Title));
                }
                else if (!htmlTagsDictionary.ContainsKey(tag.Tag))
                {
                    result.Append(tag.ViewTag);
                }
                else
                {
                    result.Append(tag.TagType == TagType.Open
                        ? htmlTagsDictionary[tag.Tag].Open
                        : htmlTagsDictionary[tag.Tag].Close);
                }

            return result.ToString();
        }
    }
}