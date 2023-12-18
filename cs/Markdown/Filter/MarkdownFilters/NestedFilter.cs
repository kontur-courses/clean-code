using Markdown.Tokens;
using Markdown.Tokens.Types;
using Markdown.Tokens.Utils;

namespace Markdown.Filter.MarkdownFilters;

//удаляет пару открывающихся/закрывающихся тегов, если она находится внутри пары открывающихся/закрывающихся тегов другого типа
public class NestedFilter : TokenFilterChain
{
    private readonly ITokenType inner;
    private readonly ITokenType outer;
    
    public NestedFilter(ITokenType inner, ITokenType outer)
    {
        this.inner = inner;
        this.outer = outer;
    }
    
    private static bool ViolateNestingRules(Token token1, Token token2, Token token3, Token token4, ITokenType inner,
        ITokenType outer)
    {
        return TokenUtils.TokenTypeEqualityComparer.Equals(token1.Type, outer)
               && TokenUtils.TokenTypeEqualityComparer.Equals(token2.Type, inner)
               && TokenUtils.TokenTypeEqualityComparer.Equals(token3.Type, inner)
               && TokenUtils.TokenTypeEqualityComparer.Equals(token4.Type, outer)
               && !token1.IsClosingTag
               && !token2.IsClosingTag
               && token3.IsClosingTag
               && token4.IsClosingTag;
    }
    
    public override List<Token> Handle(List<Token> tokens, string line)
    {
        for (var i = 0; i < tokens.Count - 3; i++)
        {
            if (!ViolateNestingRules(tokens[i], tokens[i + 1], tokens[i + 2], tokens[i + 3], inner, outer))
                continue;
            tokens[i + 1].IsMarkedForDeletion = true;
            tokens[i + 2].IsMarkedForDeletion = true;
        }

        return base.Handle(TokenUtils.DeleteMarkedTokens(tokens), line);
    }
}