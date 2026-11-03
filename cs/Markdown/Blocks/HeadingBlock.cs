namespace Markdown.Blocks
{
    public class HeadingBlock : IBlock
    {
        private readonly string text;

        public HeadingBlock(string text)
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
