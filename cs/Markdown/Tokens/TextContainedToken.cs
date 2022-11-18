namespace Markdown.Tokens;

public abstract class TextContainedToken : Token
{
    protected TextContainedToken(Token parent, string value) : base(parent, value)
    {
    }

    public override void AddChildren(Token child)
    {
        if (child is not TextContainedToken)
            throw new ApplicationException($"Cannot add token {child}");
        base.AddChildren(child);
    }
}