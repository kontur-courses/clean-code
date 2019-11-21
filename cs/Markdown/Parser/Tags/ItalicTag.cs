namespace Markdown.Parser.Tags
{
    public class ItalicTag : MarkdownTag
    {
        public override string String => "_";

        public override TokenType TokenTypeStart => TokenType.ItalicStart;
        public override TokenType TokenTypeEnd => TokenType.ItalicEnd;
    }
}