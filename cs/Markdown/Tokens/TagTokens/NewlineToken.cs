namespace Markdown
{
    class NewlineToken : TagToken
    {
        protected override string HtmlValue => "br";
    }
}
