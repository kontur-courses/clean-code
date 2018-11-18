namespace Markdown.Tags
{
    public interface IToken
    {
        string Text { get; }
        int Position { get; }
        string ToHtml();
    }
}
