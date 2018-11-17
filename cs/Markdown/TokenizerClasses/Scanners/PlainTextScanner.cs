using System.Text;

namespace Markdown.TokenizerClasses.Scanners
{
    public class PlainTextScanner : IScanner
    {
        public bool TryScan(string text, out Token token)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var value = GetTokenValue(text);
                if (!string.IsNullOrEmpty(value))
                {
                    token = new Token(TokenType.Text, value);

                    return true;
                }
            }

            token = Token.Null;

            return false;
        }

        private string GetTokenValue(string text)
        {
            var tokenValue = new StringBuilder();
            var tagScanner = new TagScanner();;
            foreach (var c in text)
            {
                if (tagScanner.TryScan(c.ToString(), out _))
                    break;

                tokenValue.Append(c);
            }

            return tokenValue.ToString();
        }
    }
}
