using System;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public int Position { get; }
        public int Length { get; private set; }
        public StringBuilder Value { get; } = new StringBuilder();
        public Token Parent { get; private set; }
        public Token Child { get; private set; }
        public TagInfo Tag { get; }

        public Token(int position, TagInfo tag)
		{
            Position = position;
            Tag = tag;
		}

        public void SetChild(Token token)
        {
            if (Child != null)
                throw new InvalidOperationException("This token already has a child");
            Child = token;
        }

        public void SetParent(Token token)
        {
            if (Parent != null)
                throw new InvalidOperationException("This token already has a parent");
            Parent = token;
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
            var tagValue = Value
                .Remove(0, Tag.TagLength)
                .Remove(Value.Length - Tag.TagLength, Tag.TagLength);
            var finalValue = openingTag + tagValue + closingTag;

            Parent.Value.Remove(openingIndex, Length);
            Parent.Value.Insert(openingIndex, finalValue);

            Parent.Child = null;
        }
    }
}
