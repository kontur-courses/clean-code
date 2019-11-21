using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.SeparatorConverters
{
    public class MarkdownToHtmlSeparatorConverter : ISeparatorConverter
    {
        public List<string> GetTokensFormats(string separator, int tokensCount)
        {
            switch (separator)
            {
                case "_":
                    return GetTokenFormatsForUnderscore(tokensCount);
                case "__":
                    return GetTokenFormatsForDoubleUnderscore(tokensCount);
                default:
                    throw new NotImplementedException();
            }
        }

        private List<string> GetTokenFormatsForUnderscore(int tokensCount)
        {
            return GetTokenFormatsForPairedHtmlTag(tokensCount, "em");

        }

        private List<string> GetTokenFormatsForDoubleUnderscore(int tokensCount)
        {
            return GetTokenFormatsForPairedHtmlTag(tokensCount, "strong");
        }

        private List<string> GetTokenFormatsForPairedHtmlTag(int tokensCount, string tagName)
        {
            switch (tokensCount)
            {
                case 0:
                    return new List<string>();
                case 1:
                    return new List<string> { $"<{tagName}>{{0}}</{tagName}>" };
                default:
                    return Enumerable.Repeat("{0}", tokensCount - 2).Prepend($"<{tagName}>{{0}}").Append($"{{0}}</{tagName}>")
                        .ToList();
            }
        }
    }
}