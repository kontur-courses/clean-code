using Markdown.Tokens;

namespace Markdown
{
    internal interface ITextConverter
    {
        string Convert(Token rootToken, ref string text);
    }

    internal class TextConverter
    {
        public static ITextConverter HTMLConverter() => new HTMLConverter();
    }
}
