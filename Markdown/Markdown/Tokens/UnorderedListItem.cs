namespace Markdown.Tokens;

public class UnorderedListItem : LineToken
{
    public UnorderedListItem() : base(TokenType.UnorderedListItem, "- ", string.Empty)
    {
        IsStackable = true;
    }
}