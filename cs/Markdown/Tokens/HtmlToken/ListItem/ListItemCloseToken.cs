namespace Markdown.Tokens.HtmlToken.ListItem;

public class ListItemCloseToken : HtmlTokenWithTag
{
    public ListItemCloseToken(string value) : base(null)
    {
    }

    public override Type OpenTypeToken => typeof(ListItemOpenToken);
    public override Type CloseTypeToken => typeof(ListItemCloseToken);
    public override string TagValue => "li";
}