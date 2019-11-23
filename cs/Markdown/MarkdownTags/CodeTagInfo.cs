namespace Markdown.MarkdownTags
{
    internal class CodeTagInfo : MarkdownTagInfo
    {
        public override string MarkdownTagOpenDesignation => "`";
        public override string MarkdownTagCloseDesignation => "`";
        public override string HtmlTagDesignation => "code";
        public override int Priority => 3;
    }
}
