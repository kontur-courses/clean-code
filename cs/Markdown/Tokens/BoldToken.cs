using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tokens
{
    public class BoldToken : IToken
    {
        public string Value => "__";
        public TokenType Type => TokenType.Bold;
        public int Position { get; }
        public bool IsOpening { get; set; }
        public bool ShouldBeSkipped { get; set; }
        public string OpeningTag => "<strong>";
        public string ClosingTag => "</strong>";

        public BoldToken(int position, bool isOpening, bool shouldBeSkipped)
        {
            Position = position;
            IsOpening = isOpening;
            ShouldBeSkipped = shouldBeSkipped;
        }

        public BoldToken(BoldToken token, bool shouldBeSkipped)
        {
            Position = token.Position;
            IsOpening = token.IsOpening;
            ShouldBeSkipped = shouldBeSkipped;
        }

        public BoldToken(int position)
        {
            Position = position;
        }
    }
}
