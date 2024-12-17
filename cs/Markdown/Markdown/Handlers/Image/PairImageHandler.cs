using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Image;

public class PairImageHandler : ImageHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens) =>
        HandlePairForTagType(tokens, TagType.Image);

    private static List<IToken> HandlePairForTagType(IReadOnlyList<IToken> tokens, TagType tagType)
    {
        var lastTagIndex = -1;
        var handledTokens = new List<IToken>();

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (lastTagIndex > -1 && token.TagType == tagType)
            {
                handledTokens.Add(MarkdownTokenCreator.CreateCloseTag(token, token.TagType, handledTokens[lastTagIndex]));
                lastTagIndex = i;
                continue;
            }

            if (token.TagType == tagType)
                lastTagIndex = i;

            handledTokens.Add(token);
        }

        return handledTokens;
    }
}