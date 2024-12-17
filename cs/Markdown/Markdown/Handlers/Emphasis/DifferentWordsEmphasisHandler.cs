using Markdown.Extensions;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class DifferentWordsEmphasisHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        if (tokens.Count < 1) return tokens;

        var tagToTextIndices = new HashSet<int>();

        for (var closeTagIndex = tokens.Count - 1; closeTagIndex > -1; closeTagIndex--)
        {
            var token = tokens[closeTagIndex];
            if (!token.IsCloseTag) continue;

            var openTagIndex = tokens.IndexOf(token.TagPair!);
            if (openTagIndex == -1) continue;

            if (!IsSurroundedByTextAndSpace(tokens, openTagIndex, closeTagIndex)) continue;
            tagToTextIndices.Add(openTagIndex);
            tagToTextIndices.Add(closeTagIndex);
        }

        return RewriteTagsToText(tokens, tagToTextIndices);
    }

    private static bool IsSurroundedByTextAndSpace(IReadOnlyList<IToken> tokens, int openTagIndex, int closeTagIndex)
    {
        if (openTagIndex <= 0 || closeTagIndex >= tokens.Count - 1 || openTagIndex >= closeTagIndex) return false;

        return tokens[openTagIndex - 1].Type == TokenType.Text &&
               tokens[openTagIndex + 1].Type == TokenType.Text &&
               tokens[closeTagIndex - 1].Type == TokenType.Text &&
               tokens[closeTagIndex + 1].Type == TokenType.Text &&
               HasSpaceBetween(tokens, openTagIndex, closeTagIndex);
    }

    private static bool HasSpaceBetween(IReadOnlyList<IToken> tokens, int startIndex, int endIndex)
    {
        for (var i = startIndex + 1; i < endIndex; i++)
            if (tokens[i].Type == TokenType.Space)
                return true;

        return false;
    }
}