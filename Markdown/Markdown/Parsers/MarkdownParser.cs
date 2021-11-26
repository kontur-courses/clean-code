using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.Parsers
{
    public class MarkdownParser : IMarkdownParser
    {
        public IEnumerable<string> Parse(string markdown)
        {
            var rawTokens = new List<string>();

            var token = new StringBuilder();

            foreach (var symbol in markdown)
            {
                if (char.IsLetter(symbol))
                {
                    token.Append(symbol);
                }
                else
                {
                    if (token.Length > 0)
                    {
                        rawTokens.Add(token.ToString());
                    }

                    rawTokens.Add(symbol.ToString());
                    token = new StringBuilder();
                }
            }

            if (token.Length > 0)
            {
                rawTokens.Add(token.ToString());
            }

            return rawTokens;
        }
    }
}