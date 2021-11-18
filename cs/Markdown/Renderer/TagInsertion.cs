namespace Markdown.Renderer
{
    internal class TagInsertion
    {
        public readonly int Shift;
        public readonly string Tag;

        public TagInsertion(string tag, int shift)
        {
            Tag = tag;
            Shift = shift;
        }
    }
}