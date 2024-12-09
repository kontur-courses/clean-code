namespace Markdown.Tokens.HtmlToken.UnorderedList;

public class UnorderedListToken : HtmlTokenWithTag
{
    public UnorderedListToken(List<BaseHtmlToken>? children) : base(children)
    {
    }
    public override Type OpenTypeToken => typeof(UnorderedListOpenToken);
    public override Type CloseTypeToken => typeof(UnorderedListCloseToken);
    public override string TagValue => "ul";
}