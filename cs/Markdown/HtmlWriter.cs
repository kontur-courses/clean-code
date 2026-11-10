using System.Text;
using Markdown.Tokens;

namespace Markdown;

public class HtmlWriter
{
    public string Write(List<Token> tokens)
    {
        var result = new StringBuilder();
        foreach (var token in tokens)
        {
            result.Append(WriteToken(token));
        }

        return result.ToString();
    }
    
    private static StringBuilder WriteToken(Token token)
    {
        return token.Role switch
        {
            TokenRole.Opening => new StringBuilder(token.OpeningHtml),
            TokenRole.Closing => new StringBuilder(token.ClosingHtml),
            _ => new StringBuilder(token.Value)
        };
    }
}