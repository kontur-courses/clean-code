using Markdown.Helpers;
using Markdown.Tokens;

namespace Markdown.Extensions;

public static class TokensIEnumerableExtensions
{
    public static IEnumerable<(int position, Token token)> GetTagTypesTokens(
        this IEnumerable<Token> tokens,
        params TagType[] tagTypes) => 
        tokens
            .Select((t, i) => (position: i, token: t))
            .Where(tokenInfo => tagTypes.Any(type => TokenUtilities.IsTagToken(tokenInfo.token, type)));
    
    public static IEnumerable<(int position, Token token)> GetAllTagTypesTokens(this IEnumerable<Token> tokens) => 
        tokens.GetTagTypesTokens(Enum.GetValues<TagType>());

    public static IEnumerable<(int left, int right)> GetTagsPairsOfTag(
        this IEnumerable<(int position, Token token)> tagTokens,
        TagType tagType)
    {
        var openTags = new Queue<int>();

        foreach (var tokenInfo in tagTokens)
        {
            if (openTags.Count != 0 && 
                TokenUtilities.TryGetTagTypeByCloseTag(tokenInfo.token, out var closeTagType) && 
                closeTagType == tagType)
            {
                yield return (openTags.Dequeue(), tokenInfo.position);
            }
            else if (TokenUtilities.TryGetTagTypeByOpenTag(tokenInfo.token, out var openTagType) && 
                     openTagType == tagType)
            {
                openTags.Enqueue(tokenInfo.position);
            }
        }
    }

    public static void DisableTagTokensInPairs(
        this IList<Token> tokens,
        IReadOnlyList<(int left, int right)> checkPairs,
        IReadOnlyList<int> tagPositions)
    {
        var index = 0;
        foreach (var checkPair in checkPairs)
        {
            while (index < tagPositions.Count && tagPositions[index] <= checkPair.left)
                index++;
            
            while (index < tagPositions.Count && IntervalHelper.IsPointInInterval(checkPair, tagPositions[index]))
                tokens[tagPositions[index]] = TokenUtilities.CreateTextToken(tokens[tagPositions[index++]].Content);
        }
    }

    public static void DisableTagTokensPairsBy(
        this IList<Token> tokens,
        IReadOnlyList<(int left, int right)> checkPairs,
        IReadOnlyList<(int left, int right)> disablePairs,
        Func<(int left, int right), (int left, int right), bool> predicate)
    {
        var pairIndex = 0;
        foreach (var checkPair in checkPairs)
        {
            while (pairIndex < disablePairs.Count && disablePairs[pairIndex].right < checkPair.left)
                pairIndex++;
            
            while (pairIndex < disablePairs.Count && predicate(checkPair, disablePairs[pairIndex]))
                DisableTagTokenPair(tokens, disablePairs[pairIndex++]);
        }
    }
    
    private static void DisableTagTokenPair(IList<Token> handledTokens, (int left, int right) tagTokenPair)
    {
        var (left, right) = tagTokenPair;
        handledTokens[left] = TokenUtilities.CreateTextToken(handledTokens[left].Content);
        handledTokens[right] = TokenUtilities.CreateTextToken(handledTokens[right].Content);
    }
}