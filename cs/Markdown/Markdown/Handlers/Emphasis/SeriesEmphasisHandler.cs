using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers.Emphasis;

public class SeriesEmphasisHandler : EmphasisHandlerBase
{
    public override IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        var tagToTextIndices = new HashSet<int>();
        var lastTagIndex = -1;
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (!IsEmphasisTag(token)) continue;
            
            if (lastTagIndex > -1 && i - lastTagIndex == 1)
                tagToTextIndices.Add(i);

            lastTagIndex = i;
        }
        
        return RewriteTagsToText(tokens, tagToTextIndices);
    }
}