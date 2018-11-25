namespace Markdown.TokenTypes
{
    class MdHeader1 : IMdToken
    {
        public string Delimiter { get; set; } = "#";
        public string HtmlTag { get; set; } = "h1";
        public bool IsPair { get; set; } = false;
    }
}
