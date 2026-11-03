namespace Markdown.Blocks
{
    public class ParagraphBlock : Block
    {
        public string Text { get; }

        public ParagraphBlock(string text)
        {
            Text = text;
        }
    }
}