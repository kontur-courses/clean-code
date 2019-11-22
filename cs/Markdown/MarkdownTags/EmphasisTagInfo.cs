namespace Markdown.MarkdownTags
{
    internal class EmphasisTagInfo : MarkdownTagInfo
    {
        public override string MarkdownTagDesignation => "_";
        public override string HtmlTagDesignation => "em";
        public override int Priority => 1;
    }
}
