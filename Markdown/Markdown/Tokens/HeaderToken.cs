namespace Markdown.Tokens;

public class HeaderTokenBase : SingleTokenBase
{
    public HeaderTokenBase(TokenBase? parent = null) : base("# ", TokenType.Header, parent)
    {
    }

    public override bool CanStartsHere(string text, int start)
    {
        return base.CanStartsHere(text, start) && start == 0;
    }
}