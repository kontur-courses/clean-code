using Markdown.Tokens;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

//если в паре за откр. тегом следует пробел или перед закр. тегом идет пробел, то удаляем всю пару
public class SpaceInterruptionFilter : TokenFilterChain
{
    public override List<Token> Handle(List<Token> tokens, string line)
    {
        var types = TokenUtils.CreatePairedTypesDictionary(tokens);

        foreach (var type in types)
        {
            for (var i = 0; i < type.Value.Count; i++)
            {
                if (!type.Value[i].IsClosingTag && TokenUtils.IsFollowedBySymbol(type.Value[i], line, char.IsWhiteSpace))
                    TokenUtils.FindPairAndMarkBothForDeletion(i, type.Value);

                if (type.Value[i].IsClosingTag && TokenUtils.IsPrecededBySymbol(type.Value[i], line, char.IsWhiteSpace))
                    TokenUtils.FindPairAndMarkBothForDeletion(i, type.Value);
            }
        }

        return base.Handle(TokenUtils.DeleteMarkedTokens(tokens), line);
    }
}