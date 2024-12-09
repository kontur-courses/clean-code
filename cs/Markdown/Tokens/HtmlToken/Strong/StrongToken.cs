namespace Markdown.Tokens.HtmlToken.Strong;

public class StrongToken : HtmlTokenWithTag
{
    public StrongToken(List<BaseHtmlToken>? children) : base(children)
    {
    }

    public override Type OpenTypeToken => typeof(StrongOpenToken);
    public override Type CloseTypeToken => typeof(StrongCloseToken);
    public override string TagValue => "strong";
}