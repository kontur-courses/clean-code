namespace Markdown.Tokens;

public class UnorderedListToken : ContainerToken
{
    public UnorderedListToken(Token parent, string value) : base(parent, value)
    {
    }

    public override TokenType Type => TokenType.UnorderedList;

    public override void AddChildren(Token child)
    {
        if (child is not UnorderedListItemToken)
            throw new ApplicationException($"Cannot add token {child}");
        base.AddChildren(child);
    }
}