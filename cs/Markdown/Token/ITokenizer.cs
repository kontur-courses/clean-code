namespace Markdown;

public interface ITokenizer
{
    public List<Token> GetTokens(Paragraph paragraph);
}