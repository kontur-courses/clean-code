using System.Text;

namespace Markdown.Tokens;

public class Token(string value, int start, string openingHtml = "", string closingHtml = "")
{
    public string Value { get; } = value;

    public int Start { get; } = start;

    public int End => Start + Value.Length;

    public TokenRole Role { get; set; } = TokenRole.Literal;

    public string OpeningHtml { get; } = openingHtml;

    public string ClosingHtml { get; } = closingHtml;
}