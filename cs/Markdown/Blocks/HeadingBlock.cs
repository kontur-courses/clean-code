namespace Markdown.Blocks
{
    public class HeadingBlock : Block
    {
        public string Text { get; }

        public HeadingBlock(string text)
        {
            Text = text;
        }
    }
}
