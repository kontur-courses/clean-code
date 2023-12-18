using Markdown.Tokens.Decorators;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

//если в паре за откр. тегом следует пробел или перед закр. тегом идет пробел, то удаляем всю пару
public class SpaceInterruptionFilter : TokenFilterChain
{
    public override List<TokenFilteringDecorator> Handle(List<TokenFilteringDecorator> tokens, string line)
    {
        var types = FilteringUtils.CreatePairedTypesDictionary(tokens);

        foreach (var (_, value) in types)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if (!value[i].IsClosingTag && TokenUtils.IsFollowedBySymbol(value[i], line, char.IsWhiteSpace))
                    FilteringUtils.FindPairAndMarkBothForDeletion(i, value);

                if (value[i].IsClosingTag && TokenUtils.IsPrecededBySymbol(value[i], line, char.IsWhiteSpace))
                    FilteringUtils.FindPairAndMarkBothForDeletion(i, value);
            }
        }

        return base.Handle(FilteringUtils.DeleteMarkedTokens(tokens), line);
    }
}