using System;

namespace Markdown
{
    public class MarkdownParser : IParser
    {
        private class TokenParseResult
        {
            public readonly bool IsToken;
            public readonly string Text;

            public TokenParseResult(bool isToken = false, string text = null)
            {
                IsToken = isToken;
                Text = text;
            }
        }

        private readonly MarkdownTagLanguage language;
        private readonly ITagTranslator translator;

        public MarkdownParser(MarkdownTagLanguage language, ITagTranslator translator)
        {
            this.language = language;
            this.translator = translator;
        }

        public string Parse(string text)
        {
            throw new NotImplementedException();
        }

        private TokenParseResult ParseToken(string text, int startingIndex, string tagSymbol)
        {
            throw new NotImplementedException();
        }

        private bool TryGetTagSymbol(string text, int startingIndex, out string tagSymbol)
        {
            throw new NotImplementedException();
        }
    }
}