namespace Markdown.TokenTypes
{
    class MdHeader2 : IMdToken
    {
        public string Delimiter { get; set; } = "##";
        public string HtmlTag { get; set; } = "h2";
        public bool IsPair { get; set; } = false;

    }
}
