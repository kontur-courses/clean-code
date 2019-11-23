namespace Markdown.MarkdownTags
{
    internal class StrongTagInfo : MarkdownTagInfo
    {
        public override string MarkdownTagOpenDesignation => "__";
        public override string MarkdownTagCloseDesignation => "__";
        public override string HtmlTagDesignation => "strong";
        public override int Priority => 2;
    }
}
