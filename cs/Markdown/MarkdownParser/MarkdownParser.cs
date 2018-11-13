using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MarkdownParser
    {
        private int currentPosition;
        private string markdown;
        private List<ISpanElement> spanElements;

        public MarkdownParser(string markdown, List<ISpanElement> spanElements)
        {
            this.markdown = markdown;
            this.spanElements = spanElements;
        }

        public IEnumerable<HtmlTag> ParseMarkdownOnHtmlTags()
        {
            currentPosition = 0;
            while (currentPosition < markdown.Length)
                yield return ParseNextTag();
        }

        public HtmlTag ParseNextTag()
        {
            var tagContent = String.Empty;
            var startPosition = currentPosition;
            var spanElement = DetermineSpanElement();

            if (spanElement == null)
            {
                tagContent = GetTagContent();
                return new HtmlTag(tagContent);
            }

            var elementInfo = StringIndexator.GetClosingElementInfo(markdown, currentPosition, spanElement, spanElements);

            if (elementInfo != null)
            {
                tagContent = markdown.Substring(currentPosition, elementInfo.OpeningIndex - currentPosition);
                currentPosition = elementInfo.ClosingIndex + 1;
                if (tagContent == String.Empty)
                {
                    tagContent = String.Concat(spanElement.GetOpeningIndicator(), spanElement.GetClosingIndicator());
                    spanElement = null;
                }
                return new HtmlTag(tagContent, spanElement);
            }

            currentPosition = startPosition + 1;
            tagContent = GetTagContent();

            return new HtmlTag(String.Concat(markdown.ElementAt(startPosition), tagContent));
        }

        private string GetTagContent()
        {
            var position = currentPosition;
            var tagContent = new StringBuilder();
            for (; position < markdown.Length; position++)
            {
                tagContent.Append(markdown.ElementAt(position));
                if (position != markdown.Length && spanElements.Any(e => markdown
                                                                             .GetOpeningTag(position + 1, e.GetOpeningIndicator()) != null))
                {
                    position++;
                    break;
                }
            }
            currentPosition = position;
            return tagContent.ToString();
        }

        public ISpanElement DetermineSpanElement()
        {
            var position = currentPosition;
            ISpanElement currentSpanElement = null;
            foreach (var spanElement in spanElements)
            {
                var elementInfo = markdown.GetOpeningTag(position, spanElement.GetOpeningIndicator());
                if (elementInfo != null)
                {
                    if (elementInfo.Length > 0)
                    {
                        currentSpanElement = spanElement;
                        currentPosition = elementInfo.ClosingIndex + 1;
                    }
                }
            }

            return currentSpanElement;
        }

    }
}
