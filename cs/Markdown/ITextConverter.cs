using Markdown.Styles;

namespace Markdown
{
    internal interface ITextConverter
    {
        string Convert(Style styledTokens);
    }

    internal class TextConverter
    {
        public static ITextConverter HTMLConverter() => new HTMLConverter();
    }
}
