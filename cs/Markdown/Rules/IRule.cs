using Markdown.Tags;

namespace Markdown.Rules;

public interface IRule
{
    public TagKind MoveByRule(char ch, int position);

    public List<Token.Token> GetTokens();

    public void ClearTokens();
}