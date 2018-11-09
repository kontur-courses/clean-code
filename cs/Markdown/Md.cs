using System;
using System.Collections.Generic;
using System.Linq;

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
            var result = "";
            var markdownParser = new MarkdownParser();
            var tags = markdownParser.ParseMarkdownOnHtmlTags(markdown, spanElements);
            foreach (var tag in tags)
            {
                if (tag.HasHtmlWrap())
                {
                    var possibleInnerElements = spanElements.Where(e => tag.SpanElement.Contains(e)).ToList();
                    var tagContent = ConvertMarkdownToHtml(tag.Content, possibleInnerElements);
                    result += tag.SpanElement.ToHtml(tagContent);

                }
                else
                {
                    result += tag.ToHtml();
                }
            }
            return result;
        }

    }
}
