using Markdown.Html.Tags;
using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class PairEmphasisHandler : EmphasisHandlerBase
{
    private readonly TagType[] pairedTagTypes = [TagType.Italic, TagType.Strong];

    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens) =>
        pairedTagTypes.Aggregate(tokens, HandlePairForTagType);

    private static List<IToken> HandlePairForTagType(IReadOnlyList<IToken> tokens, TagType tagType)
    {
        var lastTagIndex = -1;
        var handledTokens = new List<IToken>();

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (lastTagIndex > -1 && token.TagType == tagType)
            {
                handledTokens.Add(MarkdownTokenCreator.CreateCloseTag(token, token.TagType, tokens[lastTagIndex]));
                lastTagIndex = -1;
                continue;
            }

            if (token.TagType == tagType)
                lastTagIndex = i;

            handledTokens.Add(token);
        }

        return handledTokens;
    }
}