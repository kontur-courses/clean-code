namespace Markdown
{
    internal class CodeTag : MarkdownTag
    {
        public override string TagDesignation => "`";
        public override string HtmlDesignation => "code";
        public override int Priority => 3;
    }
}
