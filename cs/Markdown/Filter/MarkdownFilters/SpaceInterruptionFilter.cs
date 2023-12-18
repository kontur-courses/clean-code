using Markdown.Tokens.Decorators;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

//если в паре за откр. тегом следует пробел или перед закр. тегом идет пробел, то удаляем всю пару
public class SpaceInterruptionFilter : TokenFilterChain
{
    public override List<TokenFilteringDecorator> Handle(List<TokenFilteringDecorator> tokens, string line)
    {
        var types = FilteringUtils.CreatePairedTypesDictionary(tokens);

        foreach (var type in types)
        {
            for (var i = 0; i < type.Value.Count; i++)
            {
                if (!type.Value[i].IsClosingTag && TokenUtils.IsFollowedBySymbol(type.Value[i], line, char.IsWhiteSpace))
                    FilteringUtils.FindPairAndMarkBothForDeletion(i, type.Value);

                if (type.Value[i].IsClosingTag && TokenUtils.IsPrecededBySymbol(type.Value[i], line, char.IsWhiteSpace))
                    FilteringUtils.FindPairAndMarkBothForDeletion(i, type.Value);
            }
        }

        return base.Handle(FilteringUtils.DeleteMarkedTokens(tokens), line);
    }
}