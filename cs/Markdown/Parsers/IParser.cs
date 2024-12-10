using Markdown.Tokens.HtmlTokens;

namespace Markdown.Parsers
{
    internal interface IParser
    {
        IList<IRenderable> Parse(string text);
    }
}
