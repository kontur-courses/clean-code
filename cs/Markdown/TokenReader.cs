using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Markups;

namespace Markdown
{
    class TokenReader
    {
        private readonly string text;
        private readonly List<Markup> markups;
        private int currentPosition;

        public TokenReader(string text, List<Markup> markups)
        {
            this.text = text;
            this.markups = markups;
        }

        public bool HasMoreTokens()
        {
            return currentPosition < text.Length;
        }

        public Token NextToken()
        {
            var token = ReadTokenWithMarkup() ?? ReadPlainTextToken();
            return token;
        }

        private Token ReadTokenWithMarkup()
        {
            RawToken rawToken = null;
            var openingLength = 0;
            foreach (var markup in markups)
            {
                var tmpToken = markup.GetRawToken(text, currentPosition);
                if (tmpToken?.Markup.Opening.Length > openingLength)
                    rawToken = tmpToken;
            }

            if (rawToken == null || rawToken.Empty())
                return null;

            var tokenText = GetRawTokenText(rawToken);
            var token = new Token(tokenText, rawToken.Markup);
            currentPosition = rawToken.ClosingPosition + rawToken.Markup.Closing.Length;
            return token;
        }

        private string GetRawTokenText(RawToken rawToken)
        {
            var startText = rawToken.OpeningPosition + rawToken.Markup.Opening.Length;
            var content = text.Substring(startText, rawToken.ClosingPosition - startText);
            return content;
        }

        private Token ReadPlainTextToken()
        {
            var markupChars = markups.Select(m => m.Opening.First()).ToArray();
            var rawText = ReadPlainText(markupChars);
            return new Token(rawText, null);
        }

        private string ReadPlainText(char[] markupChars)
        {
            var tokenText = new StringBuilder();

            while (currentPosition < text.Length)
            {
                tokenText.Append(text[currentPosition++]);
                if (currentPosition != text.Length && markupChars.Contains(text[currentPosition]))
                    break;
            }
            return tokenText.ToString();
        }
    }
}
