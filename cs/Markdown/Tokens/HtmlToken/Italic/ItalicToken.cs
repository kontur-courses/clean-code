namespace Markdown.Tokens.HtmlToken.Italic;

public class ItalicToken :  HtmlTokenWithTag
{
    public ItalicToken(List<BaseHtmlToken>? children) : base(children)
    {
    }
    public override Type OpenTypeToken => typeof(ItalicOpenToken);
    public override Type CloseTypeToken => typeof(ItalicCloseToken);
    public override string TagValue => "em";
}