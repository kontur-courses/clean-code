using System.Text;

namespace Markdown.Tokens
{
    public class TokenBuilder
    {
        private int position;
        private bool isOpening;
        public TokenType Type { get; private set; }
        private readonly StringBuilder valueBuilder = new StringBuilder();

        public TokenBuilder SetType(TokenType type)
        {
            Type = type;
            return this;
        }

        public TokenBuilder Append(string value)
        {
            valueBuilder.Append(value);
            return this;
        }

        public TokenBuilder SetPosition(int position)
        {
            this.position = position;
            return this;
        }

        public TokenBuilder Append(char value)
        {
            valueBuilder.Append(value);
            return this;
        }

        public TokenBuilder SetOpening(bool isOpening)
        {
            this.isOpening = isOpening;
            return this;
        }

        public Token Build()
        {
            return new Token(valueBuilder.ToString(), Type, position, isOpening);
        }

        public TokenBuilder Clear()
        {
            position = 0;
            Type = default;
            valueBuilder.Clear();
            return this;
        }
    }
}
