namespace Markdown.TokenTypes
{
    class MdHeader3 : IMdToken
    {
        public string Delimiter { get; set; } = "###";
        public string HtmlTag { get; set; } = "h3";
        public bool IsPair { get; set; } = false;

    }
}
