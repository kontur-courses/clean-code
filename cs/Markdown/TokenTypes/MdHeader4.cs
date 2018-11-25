namespace Markdown.Types
{
    class MdHeader4 : IMdToken
    {
        public string Delimiter { get; set; } = "####";
        public string HtmlTag { get; set; } = "h4";
        public bool IsPair { get; set; } = false;

    }
}
