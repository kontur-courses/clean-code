namespace Markdown.TokenTypes
{
    class MdHeader6 : IMdToken
    {
        public string Delimiter { get; set; } = "######";
        public string HtmlTag { get; set; } = "h6";
        public bool IsPair { get; set; } = false;

    }
}
