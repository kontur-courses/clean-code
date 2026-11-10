using System.Text;

namespace Markdown.Tokens;

public class Token(string value, int start, string openingHtml = "", string closingHtml = "")
{
    protected string Value { get; } = value;

    public int Start { get; } = start;

    public int End => Start + Value.Length;

    public TokenRole Role { get; set; } = TokenRole.Literal;

    private string OpeningHtml { get; } = openingHtml;

    private string ClosingHtml { get; } = closingHtml;

    public StringBuilder WriteHtml()
    {
        return Role switch
        {
            TokenRole.Opening => new StringBuilder(OpeningHtml),
            TokenRole.Closing => new StringBuilder(ClosingHtml),
            _ => new StringBuilder(value)
        };
    }
}