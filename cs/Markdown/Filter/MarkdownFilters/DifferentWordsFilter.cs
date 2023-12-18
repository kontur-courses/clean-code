using Markdown.Tokens;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

//удаляет сразу всю пару открывающийся/закрывающийся тег, если хотя бы один из них находится полностью в другом слове
public class DifferentWordsFilter : TokenFilterChain
{
    private static bool AreInDifferentWords(Token first, Token second, string line)
        => first is not null
           && second.IsClosingTag
           && (TokenUtils.IsInWord(second, line)
               || TokenUtils.IsInWord(first, line))
           && TokenUtils.HasSymbolInBetween(line, ' ', first.StartingIndex, second.StartingIndex);
    
    public override List<Token> Handle(List<Token> tokens, string line)
    {
        var types = TokenUtils.CreatePairedTypesDictionary(tokens);

        foreach (var type in types)
        {
            Token? opening = null;
            foreach (var token in type.Value)
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

        return base.Handle(TokenUtils.DeleteMarkedTokens(tokens), line);
    }
}