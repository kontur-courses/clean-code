using System.Text;

namespace Markdown
{
    public class Token
    {
        public int Position { get; }
        public int Length { get; private set; }
        public StringBuilder Value { get; } = new StringBuilder();
        public Token Parent { get; set; }
        public Token Child { get; set; }
        public TagInfo Tag { get; }

        public Token(int position, TagInfo tag)
		{
            Position = position;
            Tag = tag;
		}

        public void AddChild(Token token)
        {
            Child = token;
        }

        public void AddCharacter(char c)
        {
            Value.Append(c);
            Length++;
        }

        public void Close()
        {
            var openingIndex = Position - Parent.Position;
            var openingTag = $"<{Tag.TagText}>";
            var closingTag = $"</{Tag.TagText}>";
            var tagValue = new StringBuilder(Value.ToString());
            tagValue = tagValue
                .Remove(0, Tag.TagLength)
                .Remove(tagValue.Length - Tag.TagLength, Tag.TagLength);
            var finalValue = openingTag + tagValue + closingTag;

            Parent.Value.Remove(openingIndex, Length);
            Parent.Value.Insert(openingIndex, finalValue);

            Parent.Child = null;
        }
    }
}
