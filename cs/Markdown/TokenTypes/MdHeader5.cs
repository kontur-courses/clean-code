namespace Markdown.Types
{
    class MdHeader5 : IMdToken
    {
        public string Delimiter { get; set; } = "#####";
        public string HtmlTag { get; set; } = "h5";
        public bool IsPair { get; set; } = false;

    }
}
