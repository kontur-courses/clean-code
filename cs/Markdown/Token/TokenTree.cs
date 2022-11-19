using Markdown.Tags;

namespace Markdown.Token;

public class TokenTree
{
    public bool IsLeaf => Value != null;
    public string? Value { get; set; }
    public Tag Tag { get; }
    public IEnumerable<TokenTree> Children => children;
    private readonly List<TokenTree> children = new ();
    
    public TokenTree(Tag tag)
    {
        Tag = tag;
    }

    public void AppendChild(TokenTree child)
    {
        children.Add(child);
    }
}