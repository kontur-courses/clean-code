namespace Markdown.Tags
{
    public class OneUnderscore : SimpleTag
    {
        public OneUnderscore(Md md) : base(md, "_")
        {
        }

        protected override string FormatTag(Token start, Token end, string contains)
        {
            var simpleFormat = base.FormatTag(start, end, contains);
            if (simpleFormat != null)
                return simpleFormat;

            return end == null ? $"{start.Value}{contains}" : $"<em>{contains}</em>";
        }
    }
}
