namespace Markdown.MarkdownTags
{
    internal class CodeTagInfo : MarkdownTagInfo
    {
        public override string MarkdownTagDesignation => "`";
        public override string HtmlTagDesignation => "code";
        public override int Priority => 3;
    }
}
