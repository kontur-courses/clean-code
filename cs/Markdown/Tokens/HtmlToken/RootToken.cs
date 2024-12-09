namespace Markdown.Tokens.HtmlToken;

public class RootToken : BaseHtmlToken, ITranslationResult
{
    public RootToken(List<BaseHtmlToken>? children) : base(children)
    {
    }

    public override string ToString() => string.Join("", Children!.Select(child => child.ToString()));
    public override string Value => string.Join("", Children!);
}