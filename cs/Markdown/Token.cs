using System;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public int Position { get; }
        private readonly StringBuilder value = new StringBuilder();
        public string Value => value.ToString();
        public Token Parent { get; private set; }
        public ITagInfo Tag { get; }

        public Token(int position, ITagInfo tag)
		{
            Position = position;
            Tag = tag;
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
        }

        public void Close()
        {
            var openingTag = $"<{Tag.HtmlTagText}>";
            var closingTag = $"</{Tag.HtmlTagText}>";
            var finalValue = openingTag + value + closingTag;

            Parent?.value.Append(finalValue);
        }

        public void Abort(Token closingToken)
        {
            if (this == closingToken)
            {
                Close();
                return;
            }

            Parent.value.Append(Tag.MarkdownTagText + value);
            Parent?.Abort(closingToken);
        }
    }
}
