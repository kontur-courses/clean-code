using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

public interface IRenderer
{
    List<Token> HandleTokens(List<Token> tokenList);
    void HandlePairedTag(Tag tag);
    void ClosePairedTag(Tag tag);
}