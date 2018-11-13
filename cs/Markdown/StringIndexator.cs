using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class StringIndexator
    {
        public static ElementInfo GetClosingElementInfo(string markdown, int currentPosition, ISpanElement currentSpanElement, List<ISpanElement> spanElements)
        {
            var position = currentPosition;

            for (; position < markdown.Length; position++)
            {
                if (currentSpanElement != null)
                {
                    var elementInfo = markdown.GetClosingTag(position, currentSpanElement.GetClosingIndicator());
                    if (elementInfo != null)
                    {
                        var positionToCheck = elementInfo.OpeningIndex + elementInfo.Length;
                        if (positionToCheck < markdown.Length && markdown.ElementAt(positionToCheck) !=  ' ')
                        {
                            var potentialClosingTag = String.Concat(currentSpanElement.GetClosingIndicator(),
                                                                                        markdown.ElementAt(positionToCheck));
                            if (!markdown.IsSpanElementClosing(positionToCheck, potentialClosingTag, spanElements))
                                return null;

                        }

                        return elementInfo;
                    }
                }
            }
            return null;
        }

    }
}
