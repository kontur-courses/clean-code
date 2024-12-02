using System.Text;
using Markdown.Tokens;

namespace Markdown.Renderers;

public class HtmlTokenConverter : ITokenConverter
{
    public string ConvertTokens(List<Token> tokens)
    {
        var result = new StringBuilder();
        foreach (var token in tokens)
            result.Append(ConvertToTag(token));
        return result.ToString();
    }
    
    private static string ConvertToTag(Token token)
    {
        if (token.TagWrapper == null)
            return token.Content;
        return token.IsClosing ? $"</{token.TagWrapper}>" : $"<{token.TagWrapper}>";
    }
}