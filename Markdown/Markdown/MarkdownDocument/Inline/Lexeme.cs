using System;
using System.Linq;

namespace Markdown.MarkdownDocument.Inline
{
    public class Lexeme : IInline
    {
        public static Lexeme CreateFromChar(char c, bool escaped = false)
        {
            if (char.IsWhiteSpace(c))
            {
                return new Lexeme(LexemeType.Whitespace, c, escaped);
            }

            if (char.IsPunctuation(c))
            {
                return new Lexeme(LexemeType.Punctuation, c, escaped);
            }

            if (char.IsDigit(c))
            {
                return new Lexeme(LexemeType.Digit, c, escaped);
            }

            return new Lexeme(LexemeType.Text, c, escaped);
        }

        public enum LexemeType
        {
            Whitespace,
            Punctuation,
            Digit,
            Text
        };

        public LexemeType Type;
        public readonly string Value;
        public bool Escaped = false;

        private Lexeme(LexemeType type, char c, bool escaped = false)
        {
            Type = type;
            Value = c.ToString();
            Escaped = escaped;
        }

        public bool IsWhitespace() => Type == LexemeType.Whitespace;
        public bool IsPunctuation() => Type == LexemeType.Punctuation;
        public bool IsDigit() => Type == LexemeType.Digit;
        public bool IsText() => Type == LexemeType.Text;
        public bool IsEscaped() => Escaped;


        public bool Equals(Lexeme other)
        {
            return other.Type == Type && other.Value == Value && other.Escaped == Escaped;
        }

        public override string ToString()
        {
            return (IsEscaped() ? "\\" : "") + Value;
        }
    }
}