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
                default:
                    throw new NotImplementedException();
            }
        }

        private List<string> GetTokenFormatsForUnderscore(int tokensCount)
        {
            if (tokensCount <= 1)
            {
                return new List<string> {"<em>{0}</em>"};
            }

            return Enumerable.Repeat("{0}", tokensCount - 2).Prepend("<em>{0}").Append("{0}</em>").ToList();

        }
    }
}