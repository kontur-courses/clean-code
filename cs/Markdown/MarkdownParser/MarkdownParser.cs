using System.Collections.Generic;
using System.Linq;

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
            var closingIndex = GetSpanElementClosingIndex(markdown, Position, spanElement, spanElements);
            if (!IsRestrictedSpanElement(markdown, spanElement, closingIndex))
            {
                Position = startPosition;
                openingIndex = startPosition;
                spanElement = null;
                closingIndex = GetSpanElementClosingIndex(markdown, Position, spanElement, spanElements);
            }
            var spanElementClosingLength = spanElement?.GetClosingIndicator().Length ?? 0;
            Position = closingIndex + 1 + spanElementClosingLength;
            return new HtmlTag(markdown.Substring(openingIndex, closingIndex - openingIndex + 1), spanElement);

        }

        private bool IsRestrictedSpanElement(string markdown, ISpanElement spanElement, int closingIndex)
        {
            return spanElement == null || IsSubstring(markdown, closingIndex + 1, spanElement.GetClosingIndicator());
        }

        public ISpanElement DetermineSpanElement(string markdown, int startPosition, List<ISpanElement> spanElements)
        {
            var currentPosition = startPosition;
            Position = startPosition;
            if (IsPreviousCharEscape(markdown, startPosition))
                return null;

            var markdownSubstring = "";
            ISpanElement currentSpanElement = null;
            while (currentPosition < markdown.Length)
            {
                var currentChar = markdown.ElementAt(currentPosition);
                markdownSubstring += currentChar;
                var recognizedSpanElements = TryGetSpanElement(markdownSubstring, spanElements);
                if (recognizedSpanElements.Any())
                {
                    currentSpanElement = recognizedSpanElements.First();
                    currentPosition++;
                }
                else break;
            }

            if (IsWrongBoundary(markdown, currentPosition))
                return null;

            if (currentSpanElement != null)
            {
                Position = currentPosition;
            }
            return currentSpanElement;
        }

        public int GetSpanElementClosingIndex(string markdown, int currentPosition, ISpanElement currentSpanElement, List<ISpanElement> spanElements)
        {
            for (currentPosition++; currentPosition < markdown.Length; currentPosition++)
            {
                if (currentSpanElement == null)
                {
                    if (IsSpanElementOpening(markdown, currentPosition, spanElements))
                        break;
                }                  
                else
                {
                    if (!IsSubstring(markdown, currentPosition, currentSpanElement.GetOpeningIndicator()))
                        continue;
                    if (!IsWrongBoundary(markdown, currentPosition - 1))
                        break;
                }
            }

            return currentPosition - 1;
        }

        private bool IsSubstring(string markdown, int currentPosition, string substring)
        {
            return string.Join(string.Empty, markdown.Skip(currentPosition)).StartsWith(substring);
        }

        private bool IsSpanElementOpening(string markdown, int currentPosition, List<ISpanElement> spanElements)
        {
            return spanElements.Any(e => IsSubstring(markdown, currentPosition, e.GetOpeningIndicator()));
        }

        private static bool IsPreviousCharEscape(string markdown, int currentPosition)
        {
            return currentPosition - 1 >= 0 && currentPosition < markdown.Length && markdown.ElementAt(currentPosition - 1) == '\\';
        }


        private static bool IsWrongBoundary(string markdown, int currentPosition)
        {
            if (currentPosition >= markdown.Length)
                return true;
            if (markdown.ElementAt(currentPosition) == ' ')
                return true;
            return false;
        }

        private IEnumerable<ISpanElement> TryGetSpanElement(string substring, List<ISpanElement> spanElements)
        {
            return spanElements
                            .Where(e => e.GetOpeningIndicator()
                                .StartsWith(substring));
        }

    }
}
