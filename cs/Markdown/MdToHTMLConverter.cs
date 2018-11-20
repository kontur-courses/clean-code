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
                if (token.Delimiter != null)
                {
                    startString = token.Delimiter.Value;
                }

                if (token.Closed)
                {
                    closingString = token.Delimiter.Value;
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