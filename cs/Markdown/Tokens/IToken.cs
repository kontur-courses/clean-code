namespace Markdown.Tokens;

public interface IToken
{
    public string TagWrapper { get; }

    public string Designation { get; }
    
    public bool IsCanContainAnotherTags { get; }

    public string TextContent { get; }

    public IEnumerable<IToken> Content { get; }

}