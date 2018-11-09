using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MarkdownParser
    {
        public int Position { get; private set; }

        public IEnumerable<HtmlTag> ParseMarkdownOnHtmlTags(string markdown, List<ISpanElement> spanElements)
        {
            Position = 0;
            while (Position < markdown.Length)
            {
                yield return ParseNextTag(markdown, Position, spanElements);
            }
        }

        public HtmlTag ParseNextTag(string markdown, int startPosition, List<ISpanElement> spanElements)
        {
            Position = startPosition;
            var spanElement = DetermineSpanElement(markdown, startPosition, spanElements);
            var openingIndex = Position;
            var closingIndex = StringIndexator.GetClosingIndex(markdown, Position, spanElement, spanElements);
            if (!markdown.IsExistingSpanElement(spanElement, closingIndex))
            {
                Position = startPosition;
                openingIndex = startPosition;
                spanElement = null;
                closingIndex = StringIndexator.GetClosingIndex(markdown, Position, null, spanElements);
            }
            var spanElementClosingLength = spanElement?.GetClosingIndicator().Length ?? 0;
            Position = closingIndex + 1 + spanElementClosingLength;
            return new HtmlTag(markdown.Substring(openingIndex, closingIndex - openingIndex + 1), spanElement);

        }


        public ISpanElement DetermineSpanElement(string markdown, int startPosition, List<ISpanElement> spanElements)
        {
            var currentPosition = startPosition;
            Position = startPosition;
            if (markdown.IsPreviousCharEscape(startPosition))
                return null;

            var markdownSubstring = new StringBuilder();
            ISpanElement currentSpanElement = null;
            while (currentPosition < markdown.Length)
            {
                var currentChar = markdown.ElementAt(currentPosition);
                markdownSubstring.Append(currentChar);
                var recognizedSpanElements = TryGetSpanElement(markdownSubstring.ToString(), spanElements);
                if (recognizedSpanElements.Any())
                {
                    currentSpanElement = recognizedSpanElements.First();
                    currentPosition++;
                }
                else break;
            }

            if (markdown.IsWrongBoundary(currentPosition))
                return null;

            if (currentSpanElement != null)
            {
                Position = currentPosition;
            }
            return currentSpanElement;
        }



        private IEnumerable<ISpanElement> TryGetSpanElement(string substring, List<ISpanElement> spanElements)
        {
            return spanElements
                            .Where(e => e.GetOpeningIndicator()
                                .StartsWith(substring));
        }

    }
}
