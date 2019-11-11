using System;
using System.Collections.Generic;
using System.Text;
using Markdown.MarkdownDocument;
using Markdown.MarkdownDocument.Inline;

namespace Markdown
{
    public static class HtmlRenderer
    {
        public static string RenderParagraph(Line line)
        {
            var lineElements = line.Elements;
            return "<p>" + RenderInlineElements(lineElements) + "</p>";
        }

        private static string RenderInlineElements(IEnumerable<IInline> lineElements)
        {
            var stringBuilder = new StringBuilder();

            foreach (var inlineElement in lineElements)
            {
                switch (inlineElement)
                {
                    case Lexeme lexeme:
                        stringBuilder.Append(lexeme.Value);
                        break;
                    case Emphasis emphasis:
                        stringBuilder.Append("<em>");
                        stringBuilder.Append(RenderInlineElements(emphasis.Content));
                        stringBuilder.Append("</em>");
                        break;
                    case StrongEmphasis strongEmphasis:
                        stringBuilder.Append("<strong>");
                        stringBuilder.Append(RenderInlineElements(strongEmphasis.Content));
                        stringBuilder.Append("</strong>");
                        break;
                    case Link link:
                        stringBuilder.Append("<a href=\"");
                        stringBuilder.Append(RenderInlineElements(link.Address));
                        stringBuilder.Append("\">");
                        stringBuilder.Append(RenderInlineElements(link.Content));
                        stringBuilder.Append("</a>");
                        break;
                }
            }

            return stringBuilder.ToString();
        }
    }
}