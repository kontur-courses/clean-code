namespace Markdown.Tokens;

public static class TokensGenerators
{
    public static readonly IReadOnlyDictionary<string, TokenGenerator> TokenGenerators =
        new Dictionary<string, TokenGenerator>()
        {
            {"_", new TokenGenerator("_", false,
                (int openIndex,int closeIndex)=>new ItalicsToken(openIndex,closeIndex))
            },
            {"__", new TokenGenerator("__",false,
                (int openIndex,int closeIndex)=>new BoldToken(openIndex,closeIndex))
            },
            { "#", new TokenGenerator("#",true,
                    (int openIndex,int closeIndex)=>new ParagraphToken(openIndex,closeIndex))
            }
        };
}

public class TokenGenerator
{
    private readonly string separator;
    public readonly bool IsSingleSeparator;
    public readonly Func<int, int, Token> CreateToken;
    
    public TokenGenerator(string separator,bool isSingleSeparator,Func<int,int,Token> createToken)
    {
        this.separator = separator;
        IsSingleSeparator = isSingleSeparator;
        CreateToken = createToken;
    }

    public int GetPreviosIndex(int index)
    {
        return index - separator.Length - 1;
    }

    public int GetTokenStartIndex(int index)
    {
        return index - separator.Length;
    }
    
}