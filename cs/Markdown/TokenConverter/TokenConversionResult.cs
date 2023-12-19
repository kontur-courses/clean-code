using Markdown.Tokens.Types;

namespace Markdown.TokenConverter;

public class TokenConversionResult
{
    public string ConvertedTokens { get; }

    public TagPair? OuterTag { get; }
    public TokenConversionResult(string convertedTokens, TagPair? outerTag)
    {
        ConvertedTokens = convertedTokens;
        OuterTag = outerTag;
    }
}