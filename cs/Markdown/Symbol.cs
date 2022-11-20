namespace Markdown;

public class Symbol
{
    public string Value { get; }
    public string RelatedTag { get; }
    public string RelatedCloseTag { get; }

    public int Length => Value.Length;

    public static implicit operator string(Symbol symbol) => symbol.Value;
    
    public Symbol(string value, string relatedTag, string relatedCloseTag)
    {
        Value = value;
        RelatedTag = relatedTag;
        RelatedCloseTag = relatedCloseTag;
    }
}