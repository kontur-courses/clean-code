namespace Markdown.Parser.Tags
{
    public class BoldTag : MarkdownTag
    {
        public override string String => "__";
        public override TokenType TokenTypeStart => TokenType.BoldStart;
        public override TokenType TokenTypeEnd => TokenType.BoldEnd;
    }
}