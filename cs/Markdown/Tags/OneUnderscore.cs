namespace Markdown.Tags
{
    public class OneUnderscore : Tag
    {
        public OneUnderscore(Md md) : base(md, "_")
        {
        }

        protected override string FormatTag(Token start, Token end, string contains)
        {
            return end == null ? $"{start.Value}{contains}" : $"<em>{contains}</em>";
        }
    }
}
