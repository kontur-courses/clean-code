namespace Markdown.MarkdownTags
{
    internal class StrongTag : MarkdownTag
    {
        public override string TagDesignation => "__";
        public override string HtmlDesignation => "strong";
        public override int Priority => 2;
    }
}
