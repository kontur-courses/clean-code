using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

using System.Collections.Generic;
using System.Linq;

public class TextInsideEmphasisHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var tagStack = new Stack<IToken>();
        var handledTokens = tokens.ToList();

        var i = 0;
        while (i < handledTokens.Count)
        {
            var token = handledTokens[i];
            if (tagStack.Count != 0 && token.TagPair == tagStack.Peek())
            {
                handledTokens = HandleClosingTag(tagStack, handledTokens, i);
                continue;
            }

            if (IsEmphasisTag(token))
                tagStack.Push(token);

            i++;
        }

        return handledTokens;
    }

    private static List<IToken> HandleClosingTag(Stack<IToken> tagStack, List<IToken> handledTokens,
        int closingTagIndex)
    {
        var openingTag = tagStack.Pop();
        var innerTokens = ExtractInnerTokens(handledTokens, closingTagIndex);

        var newTokens = new List<IToken>();
        newTokens.AddRange(handledTokens.Take(closingTagIndex));
        newTokens.Add(CreateProcessedTag(openingTag, innerTokens));
        newTokens.AddRange(
            handledTokens.Skip(closingTagIndex + 1));

        return newTokens;
    }

    private static List<IToken> ExtractInnerTokens(List<IToken> tokens, int closingTagIndex)
    {
        var openingTagIndex = tokens.FindLastIndex(t => t == tokens[closingTagIndex].TagPair);
        return openingTagIndex == -1
            ? []
            : tokens.GetRange(openingTagIndex + 1, closingTagIndex - openingTagIndex - 1);
    }

    private static IToken CreateProcessedTag(IToken openingTag, IEnumerable<IToken> innerTokens)
    {
        var hasText = innerTokens.Any(t => t.Type == TokenType.Text);
        return hasText
            ? MarkdownTokenCreator.CreateCloseTag(openingTag, openingTag.TagType, openingTag)
            : MarkdownTokenCreator.CreateTextToken(openingTag.Content);
    }
}