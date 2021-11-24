using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tokens
{
    public class ItalicToken : IToken
    {
        public string Value => "_";
        public TokenType Type => TokenType.Italics;
        public int Position { get; }
        public bool IsOpening { get; set; }
        public bool ShouldBeSkipped { get; set; }
        public string OpeningTag => "<em>";
        public string ClosingTag => "</em>";

        public ItalicToken(int position, bool isOpening, bool shouldBeSkipped)
        {
            Position = position;
            IsOpening = isOpening;
            ShouldBeSkipped = shouldBeSkipped;
        }

        public ItalicToken(int position)
        {
            Position = position;
        }
    }
}
