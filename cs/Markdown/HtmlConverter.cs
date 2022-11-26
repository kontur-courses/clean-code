using System.Text;
using Markdown.Enums;
using Markdown.Interfaces;
using Markdown.Tokens;

namespace Markdown;

public class HtmlConverter : IConverter
{
    private readonly Dictionary<TokenType, string> tagHtmlDictionary = new()
    {
        [TokenType.Italic] = "em",
        [TokenType.Header] = "h1",
        [TokenType.Strong] = "strong"
    };

    public string ConvertTokens(List<Token> tokens)
    {
        var sb = new StringBuilder();
        var tokensHasHeader = false;
        foreach (var token in tokens)
        {
            if (token.Type == TokenType.Header)
            {
                tokensHasHeader = true;
                continue;
            }

            sb.Append(GetValue(token));
        }

        if (tokensHasHeader)
        {
            sb.Insert(0, "<h1>");
            sb.Append("</h1>");
        }

        return sb.ToString();
    }

    private string GetTag(Tag tag)
    {
        return tag.Status == TagStatus.Open
            ? $"<{tagHtmlDictionary[tag.Type]}>"
            : $"</{tagHtmlDictionary[tag.Type]}>";
    }

    private string GetValue(Token token)
    {
        if (token is Text text)
            return text.Value;

        return GetTag(token as Tag);
    }
}