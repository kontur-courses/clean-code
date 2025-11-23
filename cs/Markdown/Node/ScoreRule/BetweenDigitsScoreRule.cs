namespace Markdown;

public sealed class BetweenDigitsScoreRule : IScoreRule
{
    public bool TryApply(List<Node> children, TokenCursor tokenCursor)
    {
        var current = tokenCursor.Current;
        var prev = tokenCursor.tokens.ElementAtOrDefault(tokenCursor.Position - 1);
        var next = tokenCursor.tokens.ElementAtOrDefault(tokenCursor.Position + 1);

        if (!ScoreUtils.TokenBetweenDigits(prev, next))
            return false;

        children.Add(NodeFactory.Create(NodeType.Text, current.Value, null));
        tokenCursor.Move();
        return true;
    }
}