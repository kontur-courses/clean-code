namespace Markdown.Tokens;

public static class TokensGenerators
{
    public static readonly IReadOnlyDictionary<string, TokenGenerator> Generators =
        new Dictionary<string, TokenGenerator>()
        {
            {
                "_", new TokenGenerator(
                    (int openIndex) => new ItalicsToken(openIndex))
            },
            {
                "__", new TokenGenerator(
                    (int openIndex) => new BoldToken(openIndex))
            },
            {
                "# ", new TokenGenerator(
                    (int openIndex) => new ParagraphToken(openIndex))
            },
            {
                "\\", new TokenGenerator(
                    (int openIndex)=>new ScreeningToken(openIndex,openIndex))
            }
        };
}

public class TokenGenerator
{
    public readonly Func<int, Token> CreateToken;

    public TokenGenerator( Func<int, Token> createToken)
    {
        CreateToken = createToken;
    }
}