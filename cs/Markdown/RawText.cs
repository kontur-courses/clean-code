using System.Collections.Generic;

namespace Markdown
{
    public class RawText : Text
    {
        public bool HasContent => Content.Length != 0;

        private bool contentIsClosed;

        public override IEnumerable<Token> GetContentTokens()
        {
            if (contentIsClosed)
                yield return GetToken();
        }

        public Token GetToken()
        {
            return new Token(Content.ToString(), TokenType.Raw);
        }

        protected override IReadingState ProcessUnderscore()
        {
            if (HasContent && !char.IsWhiteSpace(PreviousSymbol))
                return ProcessDefault('_');
            contentIsClosed = false;
            return new ItalicText(this);
        }

        protected override IReadingState ProcessDefault(char symbol)
        {
            contentIsClosed = true;
            Content.Append(symbol);
            PreviousSymbol = symbol;
            return this;
        }
    }
}
