using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class NonPairEmphasisTagHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var tagStack = new Stack<IToken>();
        var indexTagQueue = new Queue<int>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (tagStack.Count > 0 && token.TagPair == tagStack.Peek())
            {
                tagStack.Pop();
                indexTagQueue.Dequeue();
                continue;
            }

            if (!IsEmphasisTag(token)) continue;
            indexTagQueue.Enqueue(i);
            tagStack.Push(token);
        }

        return RewriteTagsToText(tokens, indexTagQueue.ToHashSet());
    }
}