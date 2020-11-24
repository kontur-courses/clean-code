namespace Markdown.Tags
{
    public class DoubleUnderscore : SimpleTag
    {
        public DoubleUnderscore(Md md) : base(md, "__")
        {
        }

        protected override string FormatTag(Token start, Token end, string contains)
        {
            var simpleFormat = base.FormatTag(start, end, contains);
            if (simpleFormat != null)
                return simpleFormat;

            if (Markdown.IsIn("_"))
                return WithoutFormat(start, end, contains);

            return end == null ? $"{start.Value}{contains}" : $"<strong>{contains}</strong>";
        }
    }
}
