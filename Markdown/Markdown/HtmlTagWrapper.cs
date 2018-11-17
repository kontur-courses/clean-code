using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Markdown.Tag;

namespace Markdown
{
    public class HtmlTagWrapper
    {
        private readonly Dictionary<string, ITag> dictionaryTags = new Dictionary<string, ITag>
        {
            {"_", new SingleUnderLineTag()},
            {"__", new DoubleUnderLineTag()},
            {"#", new SharpTag()}
        };

        public string ConvertToHtml(List<ITag> pairedTags)
        {
            var htmlBuilder = new StringBuilder();

            foreach (var tag in pairedTags)
            {
                if (tag is TextTag)
                    htmlBuilder.Append(tag.Content.RemoveEscapedSymbols(dictionaryTags));
                else
                {
                    htmlBuilder.Append(tag.HtmlOpen);
                    htmlBuilder.Append(GetInnerFormattedText(tag));
                    htmlBuilder.Append(tag.HtmlClose);
                }
            }

            return htmlBuilder.ToString();
        }

        private string GetInnerFormattedText(ITag tag)
        {
            var mdTagConverter = new MdTagConverter(dictionaryTags);
            var innerTags = mdTagConverter.Parse(tag.Content);

            var result = new List<ITag>();
            foreach (var t in innerTags)
                result.Add(t.Length > tag.Length ? t.ToTextTag() : t);

            if (result.Count == 0)
                return tag.Content;

            var htmlTagWrapper = new HtmlTagWrapper();
            var htmlText = htmlTagWrapper.ConvertToHtml(result);
            return htmlText;
        }
    }
}