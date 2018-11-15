using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

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
            if (token.text != null)
                return token.text;
            var result = new StringBuilder();
            foreach (var tkn in token.tokens)
            {
                result.Append(GetStringFromToken(tkn));
            }

            string startString = "";
            string closingString = "";
            if (token.tokenType == TokenType.text)
            {
                if (token.StartingDelimiter != null)
                {
                    startString = token.StartingDelimiter.delimiter;
                }

                if (token.ClosingDelimiter != null)
                {
                    closingString = token.ClosingDelimiter.delimiter;
                }
            }
            else if (token.tokenType == TokenType.escaped)
            {
                closingString = token.ClosingDelimiter.delimiter;
            }

            else if (token.tokenType != TokenType.escaped)
            {
                startString = "<" + Specification.TokenTypeToHTML[token.tokenType] + ">";
                closingString = "</" + Specification.TokenTypeToHTML[token.tokenType] + ">";
            }

            return startString + result.ToString() + closingString;
        }
    }
}