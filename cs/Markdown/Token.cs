using System;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public int Position { get; }
        public int Length => value.Length;
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
            if (Child == null)
            {
                value.Append(c);
                return;
            }
            Child.AddCharacter(c);
        }

        public void Close()
        {
            Child?.Abort();
            var openingTag = $"<{Tag.HtmlTagText}>";
            var closingTag = $"</{Tag.HtmlTagText}>";
            var finalValue = openingTag + value + closingTag;

            Parent.value.Append(finalValue);

            Parent.Child = null;
        }

        public void Abort()
        {
            Child?.Abort();
            Parent.value.Append(Tag.MarkdownTagText + value);
        }
    }
}
