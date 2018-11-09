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
            for (currentPosition++; currentPosition < markdown.Length; currentPosition++)
            {
                if (currentSpanElement == null)
                {
                    if (markdown.IsSpanElementOpening(currentPosition, spanElements))
                        break;
                }
                else
                {
                    if (markdown.ElementAt(currentPosition - 1) == '\\' || markdown.ElementAt(currentPosition - 1) == ' ') continue;
                    if (!markdown.IsSubstring(currentPosition, currentSpanElement.GetOpeningIndicator()))
                        continue;
                    if (!markdown.IsWrongBoundary(currentPosition - 1))
                        break;
                }
            }

            return currentPosition - 1;
        }

    }
}
