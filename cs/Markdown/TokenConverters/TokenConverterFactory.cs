using Markdown.Interfaces;
using Markdown.Tokens;

namespace Markdown.TokenConverters;

public static class TokenConverterFactory
{
    private static readonly Dictionary<string, string> TagPairs;

    static TokenConverterFactory()
    {
        TagPairs = new Dictionary<string, string>
        {
            { " </em>", " <em>" },
            { "<em> ", "</em> " },
            { " </strong>", " <strong>" },
            { "<strong> ", "</strong> " }
        };
    }

    public static ITokenConverter GetConverter(TokenType type)
    {
        return type switch
        {
            TokenType.Text => new TextConverter(),
            TokenType.Emphasis => new EmphasisConverter(),
            TokenType.Strong => new StrongConverter(),
            TokenType.Header => new HeaderConverter(),
            TokenType.Link => new LinkConverter(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static Dictionary<string, string> GetTagPairs()
    {
        return TagPairs;
    }
}