namespace Markdown.Models
{
    public class TokenHtmlRepresentation
    {
        public TagReplacer OpenTag { get; init; }
        public TagReplacer CloseTag { get; init; }
    }
}