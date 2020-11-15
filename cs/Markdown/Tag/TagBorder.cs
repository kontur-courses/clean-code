namespace Markdown.Tag
{
    public class TagBorder
    {
        public string Open { get; }
        public string Close { get; }

        public TagBorder(string openingBorder, string closingBorder)
        {
            Open = openingBorder;
            Close = closingBorder;
        }
    }
}