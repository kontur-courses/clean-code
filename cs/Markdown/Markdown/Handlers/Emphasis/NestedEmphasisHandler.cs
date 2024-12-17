using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class NestedEmphasisHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var tagsToTextIndices = new HashSet<int>();
        var tagStack = new Stack<IToken>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (!IsEmphasisTag(token)) continue;

            if (tagStack.Count == 0)
            {
                tagStack.Push(token);
                continue;
            }

            if (token.TagPair == tagStack.Peek())
                tagStack.Pop();
            else if (IsStrongInsideItalic(tagStack.Peek(), token))
                tagsToTextIndices.Add(i);
        }

        return RewriteTagsToText(tokens, tagsToTextIndices);
    }

    private static bool IsStrongInsideItalic(IToken firstToken, IToken secondToken) =>
        firstToken.TagType is TagType.Italic &&
        secondToken.TagType is TagType.Strong;
}