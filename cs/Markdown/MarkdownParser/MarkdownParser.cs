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
            {
                yield return ParseNextTag();
            }
        }

        public HtmlTag ParseNextTag()
        {
            var startPosition = currentPosition;
            var spanElement = DetermineSpanElement();
            var openingIndex = currentPosition;
            var closingIndex = StringIndexator.GetClosingIndex(markdown, currentPosition, spanElement, spanElements);
            if (!markdown.IsExistingSpanElement(spanElement, closingIndex))
            {
                currentPosition = startPosition;
                openingIndex = startPosition;
                spanElement = null;
                closingIndex = StringIndexator.GetClosingIndex(markdown, currentPosition, null, spanElements);
            }
            var spanElementClosingLength = spanElement?.GetClosingIndicator().Length ?? 0;
            currentPosition = closingIndex + 1 + spanElementClosingLength;
            return new HtmlTag(markdown.Substring(openingIndex, closingIndex - openingIndex + 1), spanElement);

        }


        public ISpanElement DetermineSpanElement()
        {
            var position = currentPosition;
            if (markdown.IsPreviousCharEscape(position))
                return null;

            var markdownSubstring = new StringBuilder();
            ISpanElement currentSpanElement = null;
            while (position < markdown.Length)
            {
                var currentChar = markdown.ElementAt(position);
                markdownSubstring.Append(currentChar);
                var recognizedSpanElements = TryGetSpanElement(markdownSubstring.ToString());
                if (recognizedSpanElements.Any())
                {
                    currentSpanElement = recognizedSpanElements.First();
                    position++;
                }
                else break;
            }

            if (markdown.IsWrongBoundary(position))
                return null;

            if (currentSpanElement != null)
            {
                currentPosition = position;
            }
            return currentSpanElement;
        }



        private IEnumerable<ISpanElement> TryGetSpanElement(string substring)
        {
            return spanElements
                            .Where(e => e.GetOpeningIndicator()
                                .StartsWith(substring));
        }

    }
}
