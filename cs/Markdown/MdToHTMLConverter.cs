using System.Text;

namespace Markdown
{
    public class MdToHTMLConverter
    {
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
            if (token.Text != null)
                return token.Text;
            var result = new StringBuilder();
            foreach (var tkn in token.Tokens)
            {
                result.Append(GetStringFromToken(tkn));
            }

            string startString = "";
            string closingString = "";
            if (token.TokenType == TokenType.Text)
            {
                if (token.StartingDelimiter != null)
                {
                    startString = token.StartingDelimiter.Value;
                }

                if (token.ClosingDelimiter != null)
                {
                    closingString = token.ClosingDelimiter.Value;
                }
            }

            else
            {
                startString = $"<{Specification.TokenTypeToHTML[token.TokenType]}>";
                closingString = $"</{Specification.TokenTypeToHTML[token.TokenType]}>";
            }

            return startString + result + closingString;
        }
    }
}