using System.Net.Mime;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public class TextToken : IToken
    {
        public string Text { get; }
        public int Position { get; }

        public TextToken(string text, int position)
        {
            Text = text;
            Position = position;
        }

        public string Translate(ITranslator translator)
        {
            return translator.VisitText(this);
        }
    }
}
