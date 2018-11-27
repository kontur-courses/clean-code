using System;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public int Position { get; }
        public int Length { get; private set; }
        private readonly StringBuilder value = new StringBuilder();
        public string Value => value.ToString();
        public Token Parent { get; private set; }
        public Token Child { get; private set; }
        public ITagInfo Tag { get; }

        public Token(int position, ITagInfo tag)
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
            value.Append(c);
            Length++;
        }

        public void Close()
        {
            var openingIndex = Position - Parent.Position;
            var openingTag = $"<{Tag.HtmlTagText}>";
            var closingTag = $"</{Tag.HtmlTagText}>";
            var tagValue = value
                .Remove(0, Tag.TagLength)
                .Remove(value.Length - Tag.TagLength, Tag.TagLength);
            var finalValue = openingTag + tagValue + closingTag;

            Parent.value.Remove(openingIndex, Length);
            Parent.value.Insert(openingIndex, finalValue);

            Parent.Child = null;
        }
    }
}
