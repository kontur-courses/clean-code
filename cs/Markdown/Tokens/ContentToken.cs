using System.Collections.Generic;

namespace Markdown.Tokens
{
    public class ContentToken : IToken
    {
        public string Value { get; }
        public TokenType Type => TokenType.Content;
        public int Position { get; }
        public bool IsOpening => false;
        public bool ShouldShowValue => true;
        public bool ShouldBeIgnored => false;
        public string OpeningTag => Value;
        public string ClosingTag => Value;
        public int SkipLength => 0;
        public bool ShouldBeClosed => false;

        public ContentToken(string value, int position)
        {
            Value = value;
            Position = position;
        }

        public void Validate(string markdown, IEnumerable<IToken> tokens)
        {
        }

        public void SetIsOpening(TokenBuilder tokenBuilder, string markdown,
            HashSet<TokenType> tokens)
        {
        }
    }
}
