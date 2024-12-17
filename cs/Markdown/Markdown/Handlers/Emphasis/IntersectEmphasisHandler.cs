using Markdown.Extensions;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class IntersectEmphasisHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var tagToTextIndices = new HashSet<int>();
        var lastOpenTagIndex = -1;
        var lastCloseTagIndex = -1;

        for (var closeTagIndex = tokens.Count - 1; closeTagIndex > -1; closeTagIndex--)
        {
            var token = tokens[closeTagIndex];
            if (!token.IsCloseTag) continue;

            var openTagIndex = tokens.IndexOf(token.TagPair!);
            if (openTagIndex == -1) continue;

            if (closeTagIndex > lastOpenTagIndex && openTagIndex < lastOpenTagIndex)
            {
                tagToTextIndices.Add(lastOpenTagIndex);
                tagToTextIndices.Add(lastCloseTagIndex);
                tagToTextIndices.Add(openTagIndex);
                tagToTextIndices.Add(closeTagIndex);
            }

            lastCloseTagIndex = closeTagIndex;
            lastOpenTagIndex = openTagIndex;
        }

        return RewriteTagsToText(tokens, tagToTextIndices);
    }
}