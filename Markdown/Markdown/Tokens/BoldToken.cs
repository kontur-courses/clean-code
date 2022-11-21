namespace Markdown.Tokens;

public class BoldTokenBase : DoubleTokenBase
{
    public BoldTokenBase(TokenBase? parent = null) : base("__", "__", TokenType.Bold, parent)
    {
    }

    public override bool CanStartsHere(string text , int start)
    {
        return base.CanStartsHere(text , start) && /*ToDo*/Parent.TokenType != TokenType.Italic;
    }
}