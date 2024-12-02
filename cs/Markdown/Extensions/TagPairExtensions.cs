using Markdown.Constants;
using Markdown.Models;

namespace Markdown.Extensions;

internal static class TagPairExtensions
{
    internal static IReadOnlyCollection<TagPair> FilterOverlapping(this IReadOnlyCollection<TagPair> tokens,
        IReadOnlyCollection<TagPair> overlappingTokens)
    {
        ArgumentNullException.ThrowIfNull(tokens);
        ArgumentNullException.ThrowIfNull(overlappingTokens);

        var filteredTokens = new List<TagPair>();
        foreach (var token in tokens)
        {
            var isOverlapping = overlappingTokens.Any(overlappingToken => overlappingToken.IsContains(token));
            if (isOverlapping)
            {
                continue;
            }

            filteredTokens.Add(token);
        }

        return filteredTokens;
    }
}