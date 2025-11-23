namespace Markdown;

public sealed class NextIsScoreScoreRule :  IScoreRule
{
    public bool TryApply(List<Node> children, TokenCursor tokenCursor)
    {
        var current = tokenCursor.Current;
        var next = tokenCursor.tokens.ElementAtOrDefault(tokenCursor.Position + 1);
        if (next is null)
            return false;

        if (next.Type is not (TokenType.Underscore or TokenType.DoubleUnderscore))
            return false;

        children.Add(NodeFactory.Create(NodeType.Text, current.Value, null));
        tokenCursor.Move();
        return true;
    }
}