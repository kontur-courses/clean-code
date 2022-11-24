namespace Markdown.Tokens;

public class HeaderToken : Token
{
    public HeaderToken() : base("# ", string.Empty, TokenType.Header) { }
    
    public override bool CanStartsHere(string text, int index)
    {
        return base.CanStartsHere(text, index) && index == 0;
    }
}

  