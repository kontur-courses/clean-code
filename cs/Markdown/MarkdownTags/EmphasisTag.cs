namespace Markdown.MarkdownTags
{
    internal class EmphasisTag : MarkdownTag
    {
        public override string TagDesignation => "_";
        public override string HtmlDesignation => "em";
        public override int Priority => 1;
    }
}
