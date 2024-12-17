using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Image;

public class NonPairImageHandler : ImageHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var contentQueue = new Queue<string>();
        contentQueue.Enqueue(MarkdownConstants.EndImage);
        contentQueue.Enqueue(MarkdownConstants.MiddleImage);
        contentQueue.Enqueue(MarkdownConstants.StartImage);

        for (var i = tokens.Count - 1; i > -1; i--)
        {
            var token = tokens[i];
            if (!IsImageTag(token)) continue;

            if (contentQueue.Peek() == token.Content)
                contentQueue.Dequeue();
        }

        return contentQueue.Count > 0 ? RewriteAllImageTagsToText(tokens) : tokens;
    }

    private static List<IToken> RewriteAllImageTagsToText(IReadOnlyList<IToken> tokens) =>
        tokens.Select(token => IsImageTag(token) ? MarkdownTokenCreator.CreateTextToken(token.Content) : token)
            .ToList();
}