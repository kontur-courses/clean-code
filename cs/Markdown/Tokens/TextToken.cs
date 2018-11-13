using System.Net.Mime;
using Markdown.Tags;

namespace Markdown.Tokens
{
    class TextToken : IToken
    {
        public string Text { get; }
        public int Position { get; }

        public TextToken(string text, int position)
        {
            Text = text;
            Position = position;
        }

        public string ToHtml()
        {
            return Text;
        }
    }
}
