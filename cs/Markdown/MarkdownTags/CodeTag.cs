namespace Markdown
{
    class CodeTag : IMarkdownTagInfo
    {
        public string HtmlDesignation => "code";

        public int Priority => 2;
    }
}
