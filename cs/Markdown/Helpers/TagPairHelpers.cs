using System.Collections.Immutable;
using Markdown.Constants;
using Markdown.Models;

namespace Markdown.Helpers;

internal static class TagPairHelpers
{
    internal static IEnumerable<TagToken> FilterShielding(IEnumerable<TagPair> tokenPairs, string text)
    {
        ArgumentNullException.ThrowIfNull(tokenPairs);
        ArgumentNullException.ThrowIfNull(text);

        foreach (var tagPair in tokenPairs)
        {
            var shielding = FilterConstants.Shielding.ToString();

            if (IsTokenShielded(tagPair.OpenToken, text))
            {
                yield return CreateShieldingToken(tagPair.OpenToken, shielding);
                continue;
            }

            if (IsTokenShielded(tagPair.CloseToken, text))
            {
                yield return CreateShieldingToken(tagPair.CloseToken, shielding);
                continue;
            }

            if (IsBothBehindIndicesShielding(text, tagPair.OpenToken.TagIndex))
            {
                yield return CreateDoubleShieldingToken(tagPair.OpenToken, shielding);
            }

            if (IsBothBehindIndicesShielding(text, tagPair.CloseToken.TagIndex))
            {
                yield return CreateDoubleShieldingToken(tagPair.CloseToken, shielding);
            }

            yield return tagPair.OpenToken;
            yield return tagPair.CloseToken;
        }
    }

    internal static IReadOnlyCollection<TagPair> FilterNonIntersectingTagPairs(IReadOnlyCollection<TagPair> tagPairs)
    {
        ArgumentNullException.ThrowIfNull(tagPairs);
        if (tagPairs.Count == 0)
        {
            return ImmutableArray<TagPair>.Empty;
        }

        var startIndex = 1;
        var pairIndexToTagPairMap = CreatePairIndexToTagPairMap(tagPairs, startIndex);
        var tagIndexToPairIndexMap = CreateTagIndexMap(tagPairs, startIndex);
        var invalidPairIndices = FindInvalidPairIndices(tagIndexToPairIndexMap);
        return GetValidPairs(pairIndexToTagPairMap, invalidPairIndices);
    }

    private static IReadOnlyCollection<int> CreateTagIndexMap(IReadOnlyCollection<TagPair> tagPairs, int startIndex)
    {
        var maxTagIndex = tagPairs.Max(pair => pair.CloseToken!.TagIndex);
        var tagIndexToPairIndexMap = new int[maxTagIndex + 1];
        var pairIndex = startIndex;

        foreach (var tagPair in tagPairs)
        {
            tagIndexToPairIndexMap[tagPair.OpenToken.TagIndex] = pairIndex;
            tagIndexToPairIndexMap[tagPair.CloseToken.TagIndex] = pairIndex;
            pairIndex++;
        }

        return tagIndexToPairIndexMap;
    }

    private static IReadOnlyDictionary<int, TagPair> CreatePairIndexToTagPairMap(IReadOnlyCollection<TagPair> tagPairs,
        int startIndex)
    {
        var maxTagIndex = tagPairs.Max(pair => pair.CloseToken!.TagIndex);
        var tagIndexToPairIndexMap = new int[maxTagIndex + 1];
        var pairIndexToTagPairMap = new Dictionary<int, TagPair>();
        var pairIndex = startIndex;
        var badTagPairs = new HashSet<TagPair>();
        foreach (var tagPair in tagPairs)
        {
            var openCurrentIndex = tagIndexToPairIndexMap[tagPair.OpenToken.TagIndex];
            var closeCurrentIndex = tagIndexToPairIndexMap[tagPair.CloseToken.TagIndex];

            if (openCurrentIndex != 0 || closeCurrentIndex != 0)
            {
                badTagPairs.Add(tagPair);
                if (openCurrentIndex != 0)
                {
                    badTagPairs.Add(pairIndexToTagPairMap[openCurrentIndex]);
                }
                if (closeCurrentIndex != 0)
                {
                    badTagPairs.Add(pairIndexToTagPairMap[closeCurrentIndex]);
                }
            }
            tagIndexToPairIndexMap[tagPair.OpenToken.TagIndex] = pairIndex;
            tagIndexToPairIndexMap[tagPair.CloseToken.TagIndex] = pairIndex;
            pairIndexToTagPairMap.Add(pairIndex, tagPair);
            pairIndex++;
        }
        
        foreach (var pairIndexToTagPair in pairIndexToTagPairMap.Keys)
        {
            if (badTagPairs.Contains(pairIndexToTagPairMap[pairIndexToTagPair]))
            {
                pairIndexToTagPairMap.Remove(pairIndexToTagPair);
            }
        }

        return pairIndexToTagPairMap;
    }

    private static IReadOnlySet<int> FindInvalidPairIndices(IReadOnlyCollection<int> tagIndexToPairIndexMap)
    {
        var openTags = new Stack<int>();
        var currentlyOpenTags = new HashSet<int>();
        var invalidPairIndices = new HashSet<int>();
        foreach (var pairIndexFromMap in tagIndexToPairIndexMap)
        {
            if (pairIndexFromMap == -1)
            {
                continue;
            }
            
            if (pairIndexFromMap != 0 && !currentlyOpenTags.Contains(pairIndexFromMap))
            {
                openTags.Push(pairIndexFromMap);
                currentlyOpenTags.Add(pairIndexFromMap);
                continue;
            }

            if (pairIndexFromMap == 0 || !currentlyOpenTags.Contains(pairIndexFromMap))
            {
                continue;
            }

            var topPairIndex = openTags.Peek();
            currentlyOpenTags.Remove(pairIndexFromMap);
            if (topPairIndex != pairIndexFromMap)
            {
                invalidPairIndices.Add(topPairIndex);
                invalidPairIndices.Add(pairIndexFromMap);
            }
            else
            {
                openTags.Pop();
            }
        }

        return invalidPairIndices;
    }

    private static IReadOnlySet<TagPair> GetValidPairs(IReadOnlyDictionary<int, TagPair> pairIndexToTagPairMap,
        IReadOnlySet<int> invalidPairIndices)
    {
        var allPairIndices = pairIndexToTagPairMap.Keys;
        var resultPairs = new HashSet<TagPair>();
        foreach (var item in allPairIndices)
        {
            if (!invalidPairIndices.Contains(item))
            {
                resultPairs.Add(pairIndexToTagPairMap[item]);
            }
        }

        return resultPairs;
    }

    private static TagToken CreateShieldingToken(TagToken token, string shielding)
    {
        return new TagToken(shielding, "", token.TagIndex - shielding.Length);
    }

    private static TagToken CreateDoubleShieldingToken(TagToken token, string shielding)
    {
        return new TagToken($"{shielding}{shielding}", shielding, token.TagIndex - shielding.Length * 2);
    }

    private static bool IsTokenShielded(TagToken token, string text)
    {
        return IsBehindOneIndexShielding(text, token.TagIndex)
               && !IsBothBehindIndicesShielding(text, token.TagIndex);
    }

    private static bool IsBothBehindIndicesShielding(string text, int tagIndex)
    {
        var behindTwoIndices = tagIndex - 2;
        var behindOneIndices = tagIndex - 1;
        return behindTwoIndices >= 0
               && IsBehindOneIndexShielding(text, tagIndex)
               && IsBehindOneIndexShielding(text, behindOneIndices);
    }

    private static bool IsBehindOneIndexShielding(string text, int tagIndex)
    {
        return tagIndex > 0 && text[tagIndex - 1].Equals(FilterConstants.Shielding);
    }
}