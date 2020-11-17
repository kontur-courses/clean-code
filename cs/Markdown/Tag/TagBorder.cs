namespace Markdown.Tag
{
    public class TagBorder
    {
        public string Open { get; }
        public string Middle { get; }
        public string Close { get; }

        public TagBorder(string openingBorder, string closingBorder,
            string tagMiddlePart = null)
        {
            Open = openingBorder;
            Middle = tagMiddlePart;
            Close = closingBorder;
        }
    }
}