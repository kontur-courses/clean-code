namespace Markdown.Tokens.HtmlToken.ListItem;

public class ListItemToken : HtmlTokenWithTag
{
    public ListItemToken(List<BaseHtmlToken>? children) : base(children)
    {
        
    }

    public override Type OpenTypeToken => typeof(ListItemOpenToken);
    public override Type CloseTypeToken => typeof(ListItemCloseToken);
    public override string TagValue => "li";
}