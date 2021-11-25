using System.Text;

namespace Markdown.Tokens
{
    public class TokenBuilder
    {
        private int position;
        private readonly StringBuilder valueBuilder = new StringBuilder();

        public TokenType Type { get; private set; }

        public TokenBuilder SetPosition(int position)
        {
            this.position = position;
            return this;
        }

        public TokenBuilder Append(string value)
        {
            valueBuilder.Append(value);
            return this;
        }

        public TokenBuilder Append(char value)
        {
            valueBuilder.Append(value);
            return this;
        }

        public TokenBuilder SetSettingsByToken(IToken token)
        {
            position = token.Position;
            Type = token.Type;
            valueBuilder.Clear();
            valueBuilder.Append(token.Value);
            return this;
        }

        public IToken Build()
        {
            return new ContentToken(valueBuilder.ToString(), position);
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
