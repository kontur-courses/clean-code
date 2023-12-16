namespace Markdown.TokenSearcher
{
    public interface ITokenSearcher
    {
        List<Token> FindTokens(string line);
    }
}
