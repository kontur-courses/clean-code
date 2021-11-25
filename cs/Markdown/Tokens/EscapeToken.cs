using System.Collections.Generic;
using Markdown.Extensions;

namespace Markdown.Tokens
{
    public class EscapeToken : IToken
    {
        public const string MdTag = "\\";

        public string Value => "\\";
        public TokenType Type => TokenType.Escape;
        public int Position { get; }
        public bool IsOpening { get; private set; }
        public bool ShouldShowValue { get; private set; }
        public string OpeningTag => Value;
        public string ClosingTag => Value;
        public bool ShouldBeIgnored { get; private set; }
        public int SkipLength => 1;
        public bool ShouldBeClosed => false;

        public EscapeToken(int position)
        {
            Position = position;
        }

        public void SetIsOpening(TokenBuilder tokenBuilder, string markdown,
            HashSet<TokenType> tokens)
        {
            IsOpening = false;
        }

        public void Validate(string markdown, IEnumerable<IToken> tokens)
        {
            ShouldShowValue = true;
            var isNotLast = Position + Value.Length < markdown.Length;
            ShouldBeIgnored = isNotLast && markdown[Position + Value.Length].IsTagSymbol();
        }
    }
}
