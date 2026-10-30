namespace Markdown
{
    internal class Blocks
    {
        // Внутри должен вызываться InlineParser для text
        public interface IBlock
        {
            string ToHtml();
        }

        public class HeadingBlock : IBlock
        {
            private readonly string text;

            public HeadingBlock(string text)
            {
                this.text = text;
            }

            public string ToHtml()
            {
                throw new NotImplementedException();
            }
        }

        public class ParagraphBlock : IBlock
        {
            private readonly string text;

            public ParagraphBlock(string text)
            {
                this.text = text;
            }

            public string ToHtml()
            {
                throw new NotImplementedException();
            }
        }
    }
}
