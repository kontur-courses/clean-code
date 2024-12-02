using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public class TagsPartsInTagHandler(TagType tagType) : ITokenHandler
{
    public int Priority => 3;
    
    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens)
    {
        var handledTokens = tokens.ToList();
        var tagTokens = handledTokens.GetAllTagTypesTokens().ToList();
        var tagTypePairs = tagTokens.GetTagsPairsOfTag(tagType).ToList();
        handledTokens
            .DisableTagTokensInPairs(
                tagTypePairs, 
                tagTokens.Select(x => x.position).ToList());
        
        return handledTokens;
    }
}