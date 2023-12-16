using Markdown.TextTokenizer;

namespace Markdown.TokenBuilder;

public class TokenBuilder : ITokenBuilder
{
    private readonly ITextTokenizer textTokenizer;

    public TokenBuilder(ITextTokenizer textTokenizer)
    {
        this.textTokenizer = textTokenizer;
    }

    public List<Token.Token> BuildTokens(string text)
    {
        List<Token.Token> tokens = textTokenizer.Split(text);
        return tokens;
    }
}