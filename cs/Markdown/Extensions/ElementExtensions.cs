using Markdown.Element;

namespace Markdown.Extensions
{
    public static class ElementExtensions
    {
        public static int GetOpenIndex(this IElement element, string markdown, int startIndex)
        {
            while (startIndex < markdown.Length - element.Indicator.Length * 2)
            {
                if (markdown.Substring(startIndex, element.Indicator.Length) == element.Indicator
                    && !char.IsWhiteSpace(markdown[startIndex + element.Indicator.Length]) 
                    && !element.IsInsideTwoDigits(markdown, startIndex)
                    && !element.EqualsNextElement(markdown, startIndex)
                    && !IsEscaped(markdown, startIndex))

                    return startIndex;

                startIndex++;
            }

            return -1;
        }

        public static int GetCloseIndex(this IElement element, string markdown, int openIndex)
        {
            var closeIndex = openIndex + element.Indicator.Length;

            while (closeIndex <= markdown.Length - element.Indicator.Length)
            {
                if (markdown.Substring(closeIndex, element.Indicator.Length) == element.Indicator
                    && closeIndex > 0
                    && !char.IsWhiteSpace(markdown[closeIndex - 1])
                    && !element.IsInsideTwoDigits(markdown, closeIndex) 
                    && !element.EqualsPreviousElement(markdown, closeIndex) 
                    && !IsEscaped(markdown, closeIndex)
                    )
                    return closeIndex;
                closeIndex++;
            }

            return -1;
        }

        private static bool IsInsideTwoDigits(this IElement element, string markdown, int index)
        {
            var previousIsDigit = false;
            var nextIsDigit = false;

            while (index > 0)
            {
                if (char.IsDigit(markdown[index - 1]))
                {
                    previousIsDigit = true;
                    break;
                }

                if (index - element.Indicator.Length > -1 &&
                    markdown.Substring(index - element.Indicator.Length, element.Indicator.Length) == element.Indicator)
                {
                    index -= element.Indicator.Length;
                }
                else
                {
                    break;
                }
            }

            while (index + element.Indicator.Length < markdown.Length)
            {
                if (char.IsDigit(markdown[index + element.Indicator.Length]))
                {
                    nextIsDigit = true;
                    break;
                }

                if (index + 2 * element.Indicator.Length < markdown.Length &&
                    markdown.Substring(index + element.Indicator.Length, element.Indicator.Length) == element.Indicator)
                {
                    index += element.Indicator.Length;
                }
                else
                {
                    break;
                }
            }

            return previousIsDigit && nextIsDigit;
        }

        private static bool EqualsNextElement(this IElement element, string markdown, int index)
        {
            return index + element.Indicator.Length < markdown.Length 
                   && markdown.Substring(index + 1, element.Indicator.Length) == element.Indicator;
        }

        private static bool EqualsPreviousElement(this IElement htmlElement, string markdown, int index)
        {
            return index - htmlElement.Indicator.Length > -1 
                   && markdown.Substring(index - htmlElement.Indicator.Length, htmlElement.Indicator.Length) == htmlElement.Indicator
                   && !IsEscaped(markdown, index - htmlElement.Indicator.Length);
        }

        private static bool IsEscaped(string markdown, int index)
        {
            var slashCount = 0;

            while (index - slashCount > 0 && markdown[index - slashCount - 1] == '\\')
                slashCount++;

            return slashCount % 2 == 1;
        }
    }
}
