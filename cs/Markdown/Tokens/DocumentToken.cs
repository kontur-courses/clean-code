namespace Markdown.Tokens;

public class DocumentToken : Token
{
    public DocumentToken() : base(null!, string.Empty)
    {
    }

    public override TokenType Type => TokenType.Document;

    public override void AddChildren(Token child)
    {
        if (child is not ContainerToken)
            throw new ApplicationException($"Cannot add token {child}");
        base.AddChildren(child);
    }
}