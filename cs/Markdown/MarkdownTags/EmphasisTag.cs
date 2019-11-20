namespace Markdown
{
    internal class EmphasisTag : IMarkdownTagInfo
    {
        public string HtmlDesignation => "em";
        public int Priority => 1;
    }
}
