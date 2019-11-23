namespace Markdown.MarkdownTags
{
    internal class EmphasisTagInfo : MarkdownTagInfo
    {
        public override string MarkdownTagOpenDesignation => "_";
        public override string MarkdownTagCloseDesignation => "_";
        public override string HtmlTagDesignation => "em";
        public override int Priority => 1;
    }
}
