namespace Markdown.Blocks
{
    public class ParagraphBlock : IBlock
    {
        private readonly string text;

        public ParagraphBlock(string text)
        {
            this.text = text;
        }

        public string ToHtml()
        {
            // Внутри должен вызываться InlineParser для text
            throw new NotImplementedException();
        }
    }
}