namespace Markdown.Types
{
    class MdText : IMdToken
    {
        public string Delimiter { get; set; } = null;
        public string HtmlTag { get; set; } = null;
        public bool IsPair { get; set; } = false;
    }
}