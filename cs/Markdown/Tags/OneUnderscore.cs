namespace Markdown.Tags
{
    public class OneUnderscore : SimpleTag
    {
        public OneUnderscore(Md md) : base(md, "_")
        {
        }

        protected override string FormatTag(Token start, Token end, string contains)
        {
            var inWordTag = IsInType(start, TokenType.Word);
            return end == null ? $"{start.Value}{contains}" : $"<em>{contains}</em>";
        }
    }
}
