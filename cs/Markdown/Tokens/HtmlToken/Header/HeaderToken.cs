namespace Markdown.Tokens.HtmlToken.Header;

public class HeaderToken : HtmlTokenWithTag
{
    public HeaderToken(List<BaseHtmlToken>? children) : base(children)
    {
    }

    public override Type OpenTypeToken => typeof(HeaderOpenToken);
    public override Type CloseTypeToken => typeof(HeaderCloseToken);
    public override string TagValue => "h1";
}