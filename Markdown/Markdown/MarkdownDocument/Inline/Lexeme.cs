using System;
using System.Linq;

namespace Markdown.MarkdownDocument.Inline
{
    public class Lexeme : IInline
    {
        private static readonly char[] PunctuationCharacters = new char[]
        {
            '!', ',', '"', '#', '$', '%', '&', '\'',
            '(', ')', '*', '+', ',', '-', '.', '/',
            ':', ';', '<', '=', '>', '?', '@', '[',
            '\\', ']', '^', '_', '`', '{', '|', '}', '~'
        };

        private static readonly char[] WhitespaceCharacters = new char[]
        {
            '\u0020', // Space
            '\u0009', // Tab
            '\u000a', // Newline
            '\u000b', // Line tabulation
            '\u000c', // Form feed
            '\u000d', // Carriage return
        };

        private static readonly char[] NumericCharacters = new char[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        public static bool IsWhitespace(char s) => WhitespaceCharacters.Any(c => c == s);

        public static bool IsPunctuation(char s) => PunctuationCharacters.Any(c => c == s);

        public static bool IsDigit(char s) => NumericCharacters.Any(c => c == s);
        
        public static Lexeme CreateFromChar(char c, bool escaped = false)
        {
            if (IsWhitespace(c))
            {
                return new Lexeme(LexemeType.Whitespace, c, escaped);
            }

            if (IsPunctuation(c))
            {
                return new Lexeme(LexemeType.Punctuation, c, escaped);
            }

            if (IsDigit(c))
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