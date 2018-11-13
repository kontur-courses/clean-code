using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class MdToHTMLConverter
    {
        public MdToHTMLConverter()
        {
        }

        public string Convert(string mdInput)
        {
            if (mdInput == string.Empty)
                return string.Empty;
            var parser = new MarkdownParser(mdInput);
            var rootToken = parser.GetTokens();
            return GetStringFromToken(rootToken);
        }

        public string GetStringFromToken(Token token)
        {
            var result = new StringBuilder();
            if (!(token.StartingDelimiter is null))
            {
                if (token.ClosingDelimiter is null)
                {
                    result.Append(token.StartingDelimiter.delimiter);
                }

                else if (token.StartingDelimiter.delimiter == "_")
                    result.Append("<em>");
            }

            if (token.tokens.Count == 0)
            {
                result.Append(token.text);
                return result.ToString();
            }

            foreach (var tkn in token.tokens)
            {
                result.Append(GetStringFromToken(tkn));
            }

            if (!(token.ClosingDelimiter is null))
            {
                if (token.StartingDelimiter.delimiter == "_")
                    result.Append("</em>");
            }

            return result.ToString();
        }
    }
}