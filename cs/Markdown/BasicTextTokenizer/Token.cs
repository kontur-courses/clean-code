using System;

namespace Markdown.BasicTextTokenizer
{
    public class Token
    {
        public int Position { get; }
        public int Length { get; }
        public TokenType Type { get; }
        public Token PairedToken { get; set; }
        public ITagClassifier Classifier { get; }

        public Token(int position, int length, TokenType type = TokenType.Text, ITagClassifier classifier = null, Token pairedToken = null)
        {
            Position = position;
            Length = length;
            Classifier = classifier;
            Type = type;
            PairedToken = pairedToken;
        }

        public Token Add(Token other)
        {
            if (!IsSimpleToken(this) || !IsSimpleToken(other))
                throw new ArgumentException("Unable to sum complex tokens");
            if (Position + Length != other.Position)
                throw new ArgumentException("Other token should follow this token");
            return new Token(Position, Length + other.Length);
        }

        private bool IsSimpleToken(Token token)
        {
            return token.Type == TokenType.Text && token.Classifier == null && token.PairedToken == null;
        }
    }

}