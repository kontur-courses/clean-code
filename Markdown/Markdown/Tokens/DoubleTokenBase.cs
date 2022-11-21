using System.Text;

namespace Markdown.Tokens;

public abstract class DoubleTokenBase : TokenBase
{
    public string Opening { get; }
    public string Ending { get; }
    
    public DoubleTokenBase(string opening, string ending, TokenType type, TokenBase? parent = null) : base(type, parent)
    {
        Opening = opening;
        Ending = ending;
    }

    public override bool CanStartsHere(string text , int start)
    {
        return base.CanStartsHere(text , start);
    }

    public override string ToHtmlWithText(string text)
    {
        return "";
    }
}