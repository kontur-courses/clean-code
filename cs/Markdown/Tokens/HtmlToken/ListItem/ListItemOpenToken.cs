namespace Markdown.Tokens.HtmlToken.ListItem;

public class ListItemOpenToken : HtmlTokenWithTag
{
    public ListItemOpenToken(string value) : base(null)
    {
    }

    public override Type OpenTypeToken => typeof(ListItemOpenToken);
    public override Type CloseTypeToken => typeof(ListItemCloseToken);
    public override string TagValue => "li";
}