using System.Collections.Generic;
using System.Text;
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
            var tags = mdTagConverter.Parse(tag.Content);

            var checkedTags = new List<ITag>();
            foreach (var t in tags)
                checkedTags.Add(t.Length > tag.Length ? t.ToTextTag() : t);

            if (checkedTags.Count == 0)
                return tag.Content;

            var htmlTagWrapper = new HtmlTagWrapper();
            var htmlText = htmlTagWrapper.ConvertToHtml(checkedTags);
            return htmlText;
        }
    }
}