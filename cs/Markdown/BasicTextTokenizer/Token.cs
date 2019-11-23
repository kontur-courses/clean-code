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

        private Token(
            int position, 
            int length, 
            TokenType type = TokenType.Text, 
            ITagClassifier classifier = null, 
            Token pairedToken = null)
        {
            Position = position;
            Length = length;
            Classifier = classifier;
            Type = type;
            PairedToken = pairedToken;
        }

        public static Token CreateTextToken(int position, int length)
        {
            return new Token(position, length);
        }

        public static Token CreateControllingToken(
            int position, 
            int length, 
            TokenType type, 
            ITagClassifier classifier, 
            Token pairedToken)
        {
            return new Token(position, length, type, classifier, pairedToken);
        }

        public Token Concat(Token other)
        {
            if (!IsTextToken(this) || !IsTextToken(other))
                throw new ArgumentException("Can only concatenate text tokens");
            if (Position + Length != other.Position)
                throw new ArgumentException("Other token should follow this token");
            return new Token(Position, Length + other.Length);
        }

        private static bool IsTextToken(Token token)
        {
            return token.Type == TokenType.Text && token.Classifier == null && token.PairedToken == null;
        }
    }

}