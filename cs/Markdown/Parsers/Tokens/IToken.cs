namespace Markdown.Parsers.Tokens
{
    public interface IToken
    {
        IToken ToText();
        IToken ToHtml();
        IToken ToMarkdown();
    }
}
