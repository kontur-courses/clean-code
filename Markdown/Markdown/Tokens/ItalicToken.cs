namespace Markdown.Tokens;

public class ItalicTokenBase : DoubleTokenBase
{
    public ItalicTokenBase(TokenBase? parent = null) : base("_", "_", TokenType.Italic, parent)
    {
    }

    public override bool CanStartsHere(string text , int start)
    {
        return base.CanStartsHere(text , start) && /*ToDo*/Parent.TokenType != TokenType.Italic;
    }
}