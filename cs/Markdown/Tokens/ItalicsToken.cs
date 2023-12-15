namespace Markdown.Tokens;

public class ItalicsToken:IToken
{
    public string TagWrapper { get; private set; } = "strong";
    public string Designation { get; private set; } = "__";
    public bool IsCanContainAnotherTags { get; private set; }
    public IEnumerable<IToken> Content{ get; private set; }
    public string TextContent { get; private set; }

    public override string ToString()
    {
        return $"<{TagWrapper}>{HtmlBuilder.HtmlBuilder.ConvertToHtml(Content)}</{TagWrapper}>";
    }
}