namespace Markdown
{
    internal class StrongTag : IMarkdownTagInfo
    {
        public string HtmlDesignation => "strong";
        public int Priority => 2;
    }
}
