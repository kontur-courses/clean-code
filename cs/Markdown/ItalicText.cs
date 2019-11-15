using System.Collections.Generic;

namespace Markdown
{
    public class ItalicText : Text
    {
        private readonly RawText previousText;
        private bool closed;

        public ItalicText(RawText previousText)
        {
            this.previousText = previousText;
        }

        public override IEnumerable<Token> GetContentTokens()
        {
            if (closed)
            {
                if (previousText.HasContent)
                    yield return previousText.GetToken();
                yield return new Token(Content.ToString(), TokenType.Italic);
                yield break;
            }
            Content.Insert(0, '_');
            previousText.AddContent(Content);
            yield return previousText.GetToken();
        }

        protected override IReadingState ProcessUnderscore()
        {
            if (char.IsWhiteSpace(PreviousSymbol))
                return ProcessDefault('_');
            closed = true;
            return new RawText();
        }

        protected override IReadingState ProcessDefault(char symbol)
        {
            Content.Append(symbol);
            PreviousSymbol = symbol;
            return this;
        }
    }
}
