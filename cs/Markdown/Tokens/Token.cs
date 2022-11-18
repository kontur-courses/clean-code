namespace Markdown.Tokens;

public abstract class Token
{
    private readonly LinkedList<Token> children = new();

    protected Token(Token parent, string value)
    {
        Parent = parent;
        Value = value;
    }

    public IEnumerable<Token> Children => children;

    public Token Parent { get; }

    public string Value { get; }

    public abstract TokenType Type { get; }

    public virtual void AddChildren(Token child)
    {
        children.AddLast(child);
    }


    public virtual void RemoveChildren()
    {
        if (children.Count > 0) children.RemoveLast();
    }

    public override string ToString()
    {
        if (Children.Any())
            return !string.IsNullOrWhiteSpace(Value)
                ? $"[{Type}({Value}){{\n{string.Join(", ", Children)}\n}}]"
                : $"[{Type}{{\n{string.Join(", ", Children)}\n}}]";

        return string.IsNullOrWhiteSpace(Value) ? $"[{Type}]" : $"[{Type}({Value})]";
    }
}