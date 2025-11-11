using System.Text;

namespace Markdown.Dto
{
    public class Tag
    {
        public Token Token { get; }
        public StringBuilder Content { get; }
        public bool IsOpen { get;  }

        public bool ContainsSpace { get; private set; }
        public bool HasOnlyDigits { get; private set; } = true;

        public Tag(Token token, StringBuilder content, bool isOpen)
        {
            Token = token;
            Content = content;
            IsOpen = isOpen;
            UpdateFlags(content.ToString());
        }

        public void Append(string text)
        {
            Content.Append(text);
            UpdateFlags(text);
        }

        private void UpdateFlags(string text)
        {
            if (!ContainsSpace && text.Contains(' '))
            {
                ContainsSpace = true;
            }

            if (!HasOnlyDigits)
            {
                return;
            }
            foreach (var ch in text.Skip(Token.Value.Length).Where(ch => !char.IsDigit(ch)))
            {
                HasOnlyDigits = false;
                break;
            }
        }
    }
}