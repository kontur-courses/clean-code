namespace Markdown.TokenizerClasses.Scanners
{
    public class PlainTextScanner
    {
        private const string TokenType = "TEXT";

        public Token Scan(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            var tokenLength = 0;
            var scanner = new TagScanner();
            foreach (var t in text)
            {
                if (scanner.Scan(t.ToString()) != null)
                    break;

                tokenLength++;
            }

            var token = text.Substring(0, tokenLength);

            return new Token(TokenType, token);
        }
    }
}
