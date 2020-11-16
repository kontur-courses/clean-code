using System;
using System.Text;

namespace Markdown.TokenSystem
{
    public class TokenReader
    {
        public string Text { get; }
        public int Position { get; private set; }
            
        public TokenReader(string text, int position)
        {
            Text = text;
            Position = position;
        }

        public Token ReadUntil(Func<string, bool> isStopStatement)
        {
            var stringBuilder = new StringBuilder();
            var startPosition = Position;

            for (var i = startPosition; i < Text.Length; i++)
            {
                var sign = Text[i].ToString();
                if (isStopStatement(sign) || (i + 1 != Text.Length && isStopStatement(sign + Text[i + 1])))
                    return new Token(startPosition, stringBuilder.Length, stringBuilder.ToString());

                stringBuilder.Append(Text[i]);
                Position++;
            }

            return new Token(startPosition, Text.Length, stringBuilder.ToString());
        }

        public Token ReadWhile(Func<string, bool> accept)
        {
            return ReadUntil(str => !accept(str));
        }

        public Token TakeSigns(int count)
        {
            if (Position + count >= Text.Length)
                throw new ArgumentException(
                    "Sum of signs count and current position must be less than text length");
            
            var stringBuilder = new StringBuilder(count);
            var startPosition = Position;
            for (int i = startPosition; i < count; i++)
            {
                stringBuilder.Append(Text[i]);
                Position++;
            }
            
            return new Token(startPosition, count, stringBuilder.ToString());
        }

        public void StartReadingOver()
        {
            Position = 0;
        }

        public TokenReader SkipSigns(int count)
        {
            if (Position + count >= Text.Length)
                throw new ArgumentException(
                    "Sum of signs count and current position must be less than text length");
            
            Position += count;
            return this;
        }
    }
}