using Markdown.Extensions;
using Markdown.Helpers;
using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class TagTokensIntersectsHandler(TagType firstTagType, TagType secondTagType) : ITokenHandler
{
    public int Priority => 6;
    
    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        var handledTokens = tokens.ToList();
        var tagTypesTokens = tokens.GetTagTypesTokens(firstTagType, secondTagType).ToList();
        var firstTagTypePairs = tagTypesTokens.GetTagsPairsOfTag(firstTagType).ToList();
        var secondTagTypePairs = tagTypesTokens.GetTagsPairsOfTag(secondTagType).ToList();
        
        handledTokens.DisableTagTokensPairsBy(firstTagTypePairs, secondTagTypePairs, IntervalHelper.IsIntersects);
        handledTokens.DisableTagTokensPairsBy(secondTagTypePairs, firstTagTypePairs, IntervalHelper.IsIntersects);
        
        return handledTokens;
    }
}