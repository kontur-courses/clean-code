namespace Markdown.Models
{
    public class TagReplacer
    {
        public string Tag { get; }
        public int TrimLength { get; }

        public TagReplacer(string tag, int trimLength)
        {
            Tag = tag;
            TrimLength = trimLength;
        }
    }
}