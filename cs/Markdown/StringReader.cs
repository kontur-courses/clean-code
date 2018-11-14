using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class StringReader
    {
        public int CurrentPosition;
        private readonly string markdown;
        private readonly List<ISpanElement> spanElements;

        public StringReader(string markdown, List<ISpanElement> spanElements)
        {
            CurrentPosition = 0;
            this.markdown = markdown;
            this.spanElements = spanElements;

        }

        public string ReadUntilTag()
        {
            var position = CurrentPosition;
            var tagContent = new StringBuilder();
            for (; position < markdown.Length; position++)
            {
                tagContent.Append(markdown.ElementAt(position));
                if (position != markdown.Length && spanElements.Any(e => GetOpeningTag(position + 1, e.GetOpeningIndicator()) != null))
                {
                    position++;
                    break;
                }
            }
            CurrentPosition = position;
            return tagContent.ToString();
        }

        public HtmlTag WrapTag(ElementInfo elementInfo, ISpanElement spanElement)
        {
            var tagContent = markdown.Substring(CurrentPosition, elementInfo.OpeningIndex - CurrentPosition);
            CurrentPosition = elementInfo.ClosingIndex + 1;
            if (tagContent == String.Empty)
            {
                tagContent = String.Concat(spanElement.GetOpeningIndicator(), spanElement.GetClosingIndicator());
                spanElement = null;
            }
            return new HtmlTag(tagContent, spanElement);
        }

        public ISpanElement DetermineSpanElement()
        {
            var position = CurrentPosition;
            ISpanElement currentSpanElement = null;
            foreach (var spanElement in spanElements)
            {
                var elementInfo = GetOpeningTag(position, spanElement.GetOpeningIndicator());
                if (elementInfo != null)
                {
                    if (elementInfo.Length > 0)
                    {
                        currentSpanElement = spanElement;
                        CurrentPosition = elementInfo.ClosingIndex + 1;
                    }
                }
            }

            return currentSpanElement;
        }

        public ElementInfo GetClosingElementInfo(ISpanElement currentSpanElement)
        {
            var position = CurrentPosition;

            for (; position < markdown.Length; position++)
            {
                if (currentSpanElement != null)
                {
                    var elementInfo = GetClosingTag(position, currentSpanElement.GetClosingIndicator());
                    if (elementInfo != null)
                    {
                        var positionToCheck = elementInfo.OpeningIndex + elementInfo.Length;
                        if (positionToCheck < markdown.Length && markdown.ElementAt(positionToCheck) != ' ')
                        {
                            var potentialClosingTag = String.Concat(currentSpanElement.GetClosingIndicator(),
                                markdown.ElementAt(positionToCheck));
                            if (!IsSpanElementClosing(potentialClosingTag, positionToCheck))
                                return null;

                        }

                        return elementInfo;
                    }
                }
            }
            return null;
        }

        private ElementInfo GetOpeningTag(int openingIndex, string indicator)
        {
            var tag = MatchTag(indicator, openingIndex);
            if (!markdown.IsWhiteSpace(openingIndex + indicator.Length)
                && tag != null)
            {
                return tag;
            }

            return null;
        }

        private ElementInfo GetClosingTag(int closingIndex, string indicator)
        {
            var tag = MatchTag(indicator, closingIndex);
            if (!markdown.IsWhiteSpace(closingIndex - 1) && tag != null)
            {
                return tag;
            }

            return null;
        }

        private ElementInfo MatchTag(string substring, int openingIndex)
        {
            var elementInfo = TryMatchSubstring(substring, openingIndex);
            if (!markdown.IsEscapeChar(openingIndex - 1) && elementInfo != null)
            {
                return elementInfo;
            }

            return null;
        }

        private ElementInfo TryMatchSubstring(string substring, int openingIndex)
        {
            ElementInfo info = null;
            if (IsSubstring(openingIndex, substring))
            {
                info = new ElementInfo(openingIndex, openingIndex + substring.Length - 1);
            }

            return info;
        }

        private bool IsSubstring(int currentPosition, string substring)
        {
            if (currentPosition + substring.Length > markdown.Length)
                return false;
            for (var i = 0; i < substring.Length; i++)
                if (markdown.ElementAt(currentPosition + i) != substring.ElementAt(i))
                    return false;
            return true;
        }

        private bool IsSpanElementClosing(string substring, int position)
        {
            return spanElements.Any(e => IsSubstring(position, substring));
        }
    }
}
