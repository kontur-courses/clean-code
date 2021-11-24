using System.Text;

namespace Markdown.Tokens
{
    public class TokenBuilder
    {
        private int position;
        private bool isOpening;
        private bool shouldBeSkipped;
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

        public TokenBuilder SetSkip(bool shouldBeSkipped)
        {
            this.shouldBeSkipped = shouldBeSkipped;
            return this;
        }

        public TokenBuilder SetSettingsByToken(IToken token)
        {
            position = token.Position;
            isOpening = token.IsOpening;
            shouldBeSkipped = token.ShouldBeSkipped;
            Type = token.Type;
            valueBuilder.Clear();
            valueBuilder.Append(token.Value);
            return this;
        }

        public IToken Build()
        {
            return Type switch
            {
                TokenType.Content => new ContentToken(valueBuilder.ToString(), position),
                TokenType.Heading => new HeadingToken(position, isOpening),
                TokenType.Bold => new BoldToken(position, isOpening, shouldBeSkipped),
                TokenType.Italics => new ItalicToken(position, isOpening, shouldBeSkipped),
                _ => new EscapeToken(),
            };
        }

        public TokenBuilder Clear()
        {
            position = 0;
            Type = default;
            isOpening = false;
            shouldBeSkipped = false;
            valueBuilder.Clear();
            return this;
        }
    }
}
