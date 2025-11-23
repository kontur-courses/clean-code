namespace Markdown;

public sealed class NextIsWhitespaceOrEndScoreRule : IScoreRule
{
    public bool TryApply(List<Node> children, TokenCursor tokenCursor)
    {
        var current = tokenCursor.Current;
        var next = tokenCursor.tokens.ElementAtOrDefault(tokenCursor.Position + 1);

        if (!ScoreUtils.IsWhiteSpaceOrEndToken(next))
            return false;

        children.Add(NodeFactory.Create(NodeType.Text, current.Value, null));
        tokenCursor.Move();
        return true;
    }
}