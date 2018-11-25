namespace Markdown.TokenTypes
{
    class MdBold : IMdToken
    {
        public string Delimiter { get; set; } = "__";
        public string HtmlTag { get; set; } = "strong";
        public bool IsPair { get; set; } = true;
    }
}
