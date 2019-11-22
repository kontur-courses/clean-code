namespace Markdown.MarkdownTags
{
    internal class StrongTagInfo : MarkdownTagInfo
    {
        public override string MarkdownTagDesignation => "__";
        public override string HtmlTagDesignation => "strong";
        public override int Priority => 2;
    }
}
