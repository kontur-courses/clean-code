namespace Markdown;

public interface IWrapper
{
    IEnumerable<string> WrapTokensInTags(IEnumerable<Token> tokens);
}