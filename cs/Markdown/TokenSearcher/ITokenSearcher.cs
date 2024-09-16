namespace Markdown.TokenSearcher
{
    public interface ITokenSearcher
    {
        List<Token> SearchTokens(string line);
    }
}
