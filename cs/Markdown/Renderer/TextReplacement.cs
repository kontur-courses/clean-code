namespace Markdown.Renderer
{
    internal class TextReplacement
    {
        public readonly int Shift;
        public readonly string Tag;

        public TextReplacement(string tag, int shift)
        {
            Tag = tag;
            Shift = shift;
        }
    }
}