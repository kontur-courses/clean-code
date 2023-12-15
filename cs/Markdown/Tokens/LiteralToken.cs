namespace Markdown.Tokens;

public class LiteralToken
{
    public string TagWrapper { get; private set; } = "";
    public string Designation { get; private set; } = "";
    public bool IsCanContainAnotherTags { get; private set; }
    public IEnumerable<IToken> Content{ get; private set; }
    public string TextContent { get; private set; }

    public override string ToString()
    {
        return TextContent;
    }
}