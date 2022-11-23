using Markdown.Tags;

namespace Markdown.Token;

public class TokenTree
{
    public bool IsLeaf => children.Count == 0;
    public string? Value { get; set; }
    public Tag? Tag { get; }
    public bool Closed { get; private set; }
    public bool Valid { get; private set; }
    public IEnumerable<TokenTree> Children => children;
    private readonly List<TokenTree> children = new ();
    
    public TokenTree()
    {
        Closed = false;
        Valid = true;
    }
    
    public TokenTree(Tag tag)
    {
        Tag = tag;
        Closed = false;
        Valid = true;
    }

    public void AppendChild(TokenTree child)
    {
        children.Add(child);
    }

    public void Invalidate()
    {
        Valid = false;
    }

    public void Close()
    {
        Closed = true;
    }
}