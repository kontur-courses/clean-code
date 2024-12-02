using Markdown.Extensions;
using Markdown.Helpers;
using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class NestedTagTokensHandler(TagType outerTagType, TagType nestedTagType) : ITokenHandler
{
    public int Priority => 7;
    
    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        var handledTokens = tokens.ToList();
        var tagTypesTokens = tokens.GetTagTypesTokens(outerTagType, nestedTagType).ToList();
        var outerTagTypePairs = tagTypesTokens.GetTagsPairsOfTag(outerTagType).ToList();
        var nestedTagTypePairs = tagTypesTokens.GetTagsPairsOfTag(nestedTagType).ToList();
        
        handledTokens.DisableTagTokensPairsBy(outerTagTypePairs, nestedTagTypePairs, IntervalHelper.IsPartOfInterval);
        
        return handledTokens;
    }
}