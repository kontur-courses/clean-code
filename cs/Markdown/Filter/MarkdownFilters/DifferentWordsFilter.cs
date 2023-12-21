using Markdown.Filter.Utils;
using Markdown.Tokens;
using Markdown.Tokens.Decorators;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

/// <summary>
/// Удаляет сразу всю пару открывающийся/закрывающийся тег, если хотя бы один из них находится полностью в другом слове
/// </summary>
public class DifferentWordsFilter : TokenFilterChain
{
    public override List<TokenFilteringDecorator> Handle(List<TokenFilteringDecorator> tokens, string line)
    {
        var types = FilteringUtils.CreatePairedTypesDictionary(tokens);

        foreach (var (_, type) in types)
        {
            TokenFilteringDecorator? opening = null;
            foreach (var token in type)
            {
                if (AreInDifferentWords(opening!, token, line))
                {
                    token.IsMarkedForDeletion = true;
                    opening!.IsMarkedForDeletion = true;
                    opening = null;
                    continue;
                }

                opening = token.IsClosingTag ? null : token;
            }
        }

        return base.Handle(FilteringUtils.DeleteMarkedTokens(tokens), line);
    }
    
    
    private static bool AreInDifferentWords(Token first, Token second, string line)
        => first is not null
           && second.IsClosingTag
           && (TokenUtils.IsInWord(second, line)
               || TokenUtils.IsInWord(first, line))
           && TokenUtils.HasSymbolInBetween(line, ' ', first.StartingIndex, second.StartingIndex);
}