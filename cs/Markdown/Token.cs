using Markdown.Markups;

namespace Markdown
{
    class Token
    {
        public readonly string Text;
        public readonly Markup Markup;

        public Token(string text, Markup markup)
        {
            Text = text;
            Markup = markup;
        }

        public bool HasMarkup()
        {
            return Markup != null;
        }

        public string ConvertToHtml(string text)
        {
            return Markup.GetTaggedText(text);
        }
    }
}
