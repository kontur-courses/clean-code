using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class StringIndexator
    {
        public static int GetClosingIndex(string markdown, int currentPosition, ISpanElement currentSpanElement, List<ISpanElement> spanElements)
        {
            var closingIndex = currentPosition;
            for (closingIndex++; closingIndex < markdown.Length; closingIndex++)
            {
                if (currentSpanElement == null)
                {
                    if (markdown.IsSpanElementOpening(closingIndex, spanElements))
                        break;
                }
                else
                {
                    if (!markdown.IsSubstring(closingIndex, currentSpanElement.GetOpeningIndicator()))
                        continue;
                    if (!markdown.IsWrongBoundary(closingIndex - 1))
                        break;
                }
            }

            return closingIndex - 1;
        }

    }
}
