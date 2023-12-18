using Markdown.Tokens;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

//удаляет пару открывающийся/закрывающийся тег, если между ними пустая строка
public class BreakingNumbersFilter : TokenFilterChain
{
    private static void ReassignTagPairs(List<Token> tokens)
    {
        var types = TokenUtils.CreatePairedTypesDictionary(tokens);

        foreach (var type in types)
        {
            var isClosingTag = false;
            foreach (var token in type.Value)
            {
                token.IsClosingTag = isClosingTag;
                isClosingTag = !isClosingTag;
            }
        }
    }
    
    public override List<Token> Handle(List<Token> tokens, string line)
    {
        foreach (var token in tokens.Where(currentToken => currentToken.Type.SupportsClosingTag))
            if (TokenUtils.IsTokenSurroundedWith(token, line, char.IsDigit, false))
                token.IsMarkedForDeletion = true;

        var result = TokenUtils.DeleteMarkedTokens(tokens);
        ReassignTagPairs(result);
        return base.Handle(TokenUtils.DeleteMarkedTokens(tokens), line);
    }
}