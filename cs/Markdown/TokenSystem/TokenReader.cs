using System;
using System.Text;

namespace Markdown.TokenSystem
{
    public class TokenReader
    {
        public string Text { get; }
        public int Position { get; private set; }
            
        public TokenReader(string text, int position = 0)
        {
            Text = text;
            Position = position;
        }

        public Token ReadUntil(Func<string, bool> isStopStatement)
        {
            var stringBuilder = new StringBuilder();
            var startPosition = Position;

            for (; Position < Text.Length; Position++)
            {
                var sign = Text[Position].ToString();
                if (isStopStatement(sign)
                    || (Position + 1 != Text.Length && isStopStatement(sign + Text[Position + 1])))
                    break;
                
                stringBuilder.Append(Text[Position]);
            }

            if (startPosition != Position)
                Position--;
            return new Token(startPosition, startPosition - Position, stringBuilder.ToString());
        }

        public Token ReadWhile(Func<string, bool> accept)
        {
            return ReadUntil(str => !accept(str));
        }

        public TokenReader SkipSigns(int count)
        {
            if (Position + count > Text.Length)
                throw new ArgumentException(
                    "Sum of signs count and current position must be less than text length or equal");
            
            Position += count;
            return this;
        }

        public TokenReader SetPosition(int position)
        {
            if (position < 0 || position >= Text.Length)
                throw new ArgumentException(
                    "Position must be non negative number and be more than lenght of text");
            
            Position = position;
            return this;
        }
    }
}