namespace Markdown.Tokens.HtmlToken;

public abstract class HtmlTokenWithTag : BaseHtmlToken
{
    public HtmlTokenWithTag(List<BaseHtmlToken>? children) : base(children)
    {
        //проверка на валидность по типам
    }

    public abstract Type OpenTypeToken { get; }
    public abstract Type CloseTypeToken { get; }
    public abstract string TagValue { get; }
    public override string Value => string.Join("", Children!.Select(child => child.Value));
    public override string ToString()
    {
        return $"<{TagValue}>{string.Join("", Children.InnerElements().Select(token => token.ToString()))}</{TagValue}>";
    }
}