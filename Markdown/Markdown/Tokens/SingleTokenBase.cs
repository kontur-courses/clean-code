namespace Markdown.Tokens;

public abstract class SingleTokenBase : TokenBase
{
    public string Opening { get; }
    
    public SingleTokenBase(string opening, TokenType type, TokenBase? parent = null) : base(type, parent)
    {
        Opening = opening;
    }

    public override bool CanStartsHere(string text, int start)
    {
        return start >= 0 
               && start + Opening.Length < text.Length 
               && !Opening.Where((t, i) => text[start + i] != t).Any() 
               && base.CanStartsHere(text, start);
    }
}