namespace Markdown.Tokens;

public static class TokensGenerators
{
    public static readonly IReadOnlyDictionary<string, TokenGenerator> TokenGenerators =
        new Dictionary<string, TokenGenerator>()
        {
            {"_", new TokenGenerator("_", false,
                (int openIndex)=>new ItalicsToken(openIndex))
            },
            {"__", new TokenGenerator("__",false,
                (int openIndex)=>new BoldToken(openIndex))
            },
            { "#", new TokenGenerator("#",true,
                    (int openIndex)=>new ParagraphToken(openIndex))
            }
        };
}

public class TokenGenerator
{
    private readonly string separator;
    public readonly bool IsSingleSeparator;
    public readonly Func<int, Token> CreateToken;
    
    public TokenGenerator(string separator,bool isSingleSeparator,Func<int,Token> createToken)
    {
        this.separator = separator;
        IsSingleSeparator = isSingleSeparator;
        CreateToken = createToken;
    }

    public int GetPreviousIndex(int index)
    {
        return index - separator.Length-1;
    }

    public int GetTokenStartIndex(int index)
    {
        return index - separator.Length;
    }
    
}