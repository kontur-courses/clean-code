using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private List<ISpanElement> markdownSpanElements = new List<ISpanElement>()
        {
            new SingleUnderscore(),
            new DoubleUnderscore()
        };

        public String Render(string markdown)
        {
            return ConvertMarkdownToHtml(markdown, markdownSpanElements);
        }

        public string ConvertMarkdownToHtml(string markdown, List<ISpanElement> spanElements)
        {
            var convertedTag = String.Empty;
            var result = new StringBuilder();
            var markdownParser = new MarkdownParser(markdown, spanElements);
            var tags = markdownParser.ParseMarkdownOnHtmlTags();
            foreach (var tag in tags)
            {
                if (tag.HasHtmlWrap())
                {
                    var possibleInnerElements = spanElements.Where(e => tag.SpanElement.Contains(e)).ToList();
                    var tagContent = ConvertMarkdownToHtml(tag.Content, possibleInnerElements).RemoveEscapes();
                    convertedTag = tag.SpanElement.ToHtml(tagContent);
                }
                else
                    convertedTag = tag.ToHtml().RemoveEscapes();

                result.Append(convertedTag);
            }
            return result.ToString();
        }

    }
}
