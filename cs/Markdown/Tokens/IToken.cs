namespace Markdown.Tags
{
    interface IToken
    {
        string Text { get; }
        int Position { get; }
        string ToHtml();
    }
}
