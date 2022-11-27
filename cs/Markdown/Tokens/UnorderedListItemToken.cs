namespace Markdown.Tokens;

public class UnorderedListItemToken : Token
{
    public UnorderedListItemToken(Token parent, string value) : base(parent, value)
    {
    }

    public override TokenType Type => TokenType.UnorderedListItem;

    public override void AddChildren(Token child)
    {
        if (child is not TextContainedToken)
            throw new ApplicationException($"Cannot add token {child}");
        base.AddChildren(child);
    }
}