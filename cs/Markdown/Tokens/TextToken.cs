using System.Net.Mime;
using System.Text;
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

        public void Translate(ITranslator translator, StringBuilder stringBuilder)
        {
            translator.VisitText(this, stringBuilder);
        }
    }
}
