namespace Markdown.Tokens;

public abstract class ContainerToken : Token
{
    protected ContainerToken(Token parent, string value) : base(parent, value)
    {
    }

    public override void AddChildren(Token child)
    {
        if (child is ContainerToken)
            throw new ApplicationException($"Cannot add token {child}");
        base.AddChildren(child);
    }
}