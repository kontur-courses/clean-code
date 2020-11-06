namespace Markdown
{
    public class TagInfo
    {
        public readonly int Length;
        public readonly string Html;
        public readonly bool IsPaired;

        public TagInfo(int length, string html, bool isPaired)
        {
            Length = length;
            Html = html;
            IsPaired = isPaired;
        }
    }
}