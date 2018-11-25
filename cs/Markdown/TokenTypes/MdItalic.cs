namespace Markdown.TokenTypes
{
    class MdItalic : IMdToken
    {
        public string Delimiter { get; set; } = "_";
        public string HtmlTag { get; set; } = "em";
        public bool IsPair { get; set; } = true;

    }
}
