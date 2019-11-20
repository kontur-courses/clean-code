namespace Markdown
{
    internal interface IMarkdownTagInfo
    {
        string HtmlDesignation { get; }
        int Priority { get; }
    }
}
