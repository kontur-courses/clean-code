namespace Markdown;

public interface IScoreRule
{
    bool TryApply(List<Node> children, TokenCursor tokenCursor);
}