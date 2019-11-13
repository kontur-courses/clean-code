using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        private readonly IEnumerable<Style> knownStylesOrderedByTagLength;

        public Md()
        {
            knownStylesOrderedByTagLength = Enumerable
                .Cast<Style>(Enum.GetValues(typeof(Style)))
                .OrderBy(s => s.MdTag().Length);
        }

        public string Render(string mdText)
        {
            if (string.IsNullOrWhiteSpace(mdText))
                return mdText;

            var html = string.Empty;

            var actualStyles = new Stack<(Style style, int endIndex)>();

            var i = 0;
            while (i < mdText.Length)
            {
                if (actualStyles.Count > 0)
                {
                    var (currentStyle, endIndex) = actualStyles.Peek();
                    if (i == endIndex)
                    {
                        html += currentStyle.CloseHtmlTag();
                        i += currentStyle.MdTag().Length;
                        actualStyles.Pop();
                        continue;
                    }
                }

                if (IsBackslashedSymbol(ref mdText, i, out string backslashedSymbol, out int offset))
                {
                    html += backslashedSymbol;
                    i += offset;
                    continue;
                }

                if (CanBeginNewStyle(ref mdText, i, actualStyles, out Style newStyle, out int newStyleEndIndex))
                {
                    actualStyles.Push((newStyle, newStyleEndIndex));
                    html += newStyle.OpenHtmlTag();
                    i += newStyle.MdTag().Length;
                    continue;
                }

                html += mdText[i];
                i++;
            }

            return html;
        }

        private bool IsBackslashedSymbol(ref string mdText, int i, out string symbol, out int offset)
        {
            if (mdText[i] == '\\')
            {
                if (i + 1 < mdText.Length)
                {
                    var nextSymbol = mdText[i + 1];
                    foreach (var style in knownStylesOrderedByTagLength.Where(s => s.MdTag().Length == 1))
                    {
                        if (style.MdTag()[0] == nextSymbol)
                        {
                            symbol = nextSymbol.ToString();
                            offset = 2;
                            return true;
                        }
                    }
                    symbol = mdText.Substring(i, 2);
                    offset = 2;
                    return true;
                }
                else
                {
                    symbol = "\\";
                    offset = 1;
                    return true;
                }
            }

            symbol = default;
            offset = default;
            return false;
        }

        private bool CanBeginNewStyle(ref string mdText, int i, Stack<(Style style, int endIndex)> outerStyles, out Style newStyle, out int newStyleEndIndex)
        {
            foreach (var knownStyle in knownStylesOrderedByTagLength.Reverse())
            {
                if (knownStyle.IsTag(ref mdText, i) 
                    && knownStyle.CanBegin(ref mdText, i, outerStyles, out int endIndex))
                {
                    newStyle = knownStyle;
                    newStyleEndIndex = endIndex;
                    return true;
                }
            }

            newStyle = default;
            newStyleEndIndex = default;
            return false;
        }

        private void CopyRawText(string sourceString, ref string destString) =>
            destString += sourceString;

        private void CopyRawText(string sourceString, int startIndex, int length, ref string destString) =>
            CopyRawText(sourceString.Substring(startIndex, length), ref destString);
    }
}
