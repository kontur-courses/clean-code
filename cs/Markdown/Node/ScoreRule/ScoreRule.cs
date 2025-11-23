using Markdown;

public sealed class DefaultScoreRule : IScoreRule
{
    public bool TryApply(List<Node> children, TokenCursor tokenCursor)
    {
        var ctx = new ScoreContext(children, tokenCursor);

        ctx.InnerScore.Clear();
        ctx.CloseIndex = FindCloseIndex(ctx);

        return ctx.CloseIndex < 0
            ? TryHandleWithoutExplicitCloser(ctx)
            : HandleWithExplicitCloser(ctx);
    }

    private static bool TryHandleWithoutExplicitCloser(ScoreContext ctx)
    {
        ctx.CloseIndex = FindFallbackCloser(ctx);
        if (ctx.CloseIndex < 0)
        {
            EmitLiteral(ctx, ctx.CurrentToken.Value);
            ctx.Cursor.Move();
            return true;
        }

        var nodes = GetChildren(ctx, ctx.CurrentTokenPos + 1, ctx.CloseIndex);
        ctx.Children.Add(NodeFactory.Create(ctx.CurrentNodeType, null, nodes));
        ctx.Cursor.Move(ctx.CloseIndex - ctx.CurrentTokenPos + 1);
        return true;
    }

    private static bool HandleWithExplicitCloser(ScoreContext ctx)
    {
        var start = ctx.CurrentTokenPos + 1;

        if (HasCrossingPairs(ctx))
        {
            EmitLiteral(ctx, ctx.CurrentToken.Value);
            ctx.Children.Add(NodeFactory.Create(
                NodeType.Text, null, GetChildren(ctx, start, ctx.CloseIndex)));
            EmitLiteral(ctx, ctx.CurrentToken.Value);
            ctx.Cursor.Move(ctx.CloseIndex - ctx.CurrentTokenPos + 1);
            return true;
        }

        switch (ctx.CurrentTokenType)
        {
            case TokenType.Underscore:
            {
                var node = NodeFactory.Create(
                    NodeType.Emphasis, null, GetChildren(ctx, start, ctx.CloseIndex));
                ctx.Children.Add(node);
                ctx.Cursor.Move(ctx.CloseIndex - ctx.CurrentTokenPos + 1);
                return true;
            }
            case TokenType.DoubleUnderscore:
            {
                var childTokens = ctx.Cursor.tokens.Slice(
                    ctx.CurrentTokenPos + 1, ctx.CloseIndex - ctx.CurrentTokenPos);
                var childNode = new NodeGenerator().Create(childTokens);
                ctx.Children.Add(NodeFactory.Create(ctx.CurrentNodeType, null, childNode));
                ctx.Cursor.Move(ctx.CloseIndex - ctx.CurrentTokenPos + 1);
                return true;
            }
            default:
                return false;
        }
    }

    private static int FindCloseIndex(ScoreContext ctx)
    {
        var closeIndex = -1;

        for (var i = ctx.CurrentTokenPos + 1; i < ctx.Cursor.TokenCount; i++)
        {
            var t = ctx.Cursor.tokens[i];

            if (t.Type is TokenType.NewLine or TokenType.EndOfText)
                break;

            if (t.Type == ctx.CurrentTokenType)
            {
                var left = ctx.Cursor.tokens.ElementAtOrDefault(i - 1);
                var right = ctx.Cursor.tokens.ElementAtOrDefault(i + 1);
                if (left?.Type != TokenType.WhiteSpace &&
                    right?.Type is TokenType.WhiteSpace or TokenType.EndOfText)
                {
                    closeIndex = i;
                    break;
                }
            }

            if (t.Type == TokenType.Underscore || t.Type == TokenType.DoubleUnderscore)
                ctx.InnerScore.Add((i, t.Type));
        }

        return closeIndex;
    }

    private static bool HasCrossingPairs(ScoreContext ctx)
    {
        var scoreType = ctx.CurrentTokenType == TokenType.Underscore
            ? TokenType.DoubleUnderscore
            : TokenType.Underscore;

        foreach (var u in ctx.InnerScore.Where(x => x.Type == scoreType))
        {
            for (var j = u.Index + 1; j < ctx.Cursor.TokenCount; j++)
            {
                var t = ctx.Cursor.tokens[j];
                if (t.Type is TokenType.NewLine or TokenType.EndOfText)
                    break;

                if (t.Type != scoreType) continue;
                var left = ctx.Cursor.tokens.ElementAtOrDefault(j - 1);
                if (left?.Type == TokenType.WhiteSpace) continue;
                if (j > ctx.CloseIndex)
                    return true;
                break;
            }
        }

        return false;
    }

    private static List<Node> GetChildren(ScoreContext ctx, int start, int end)
    {
        var slice = new List<Node>();
        for (var i = start; i < end; i++)
        {
            var t = ctx.Cursor.tokens[i];
            slice.Add(NodeFactory.Create(NodeType.Text, t.Value, null));
        }

        return slice;
    }

    private static int FindFallbackCloser(ScoreContext ctx)
    {
        for (var i = ctx.CurrentTokenPos + 1; i < ctx.Cursor.TokenCount; i++)
        {
            var token = ctx.Cursor.tokens[i];

            if (token.Type == TokenType.WhiteSpace)
                return -1;

            if (token.Type == ctx.CurrentTokenType)
                return i;
        }

        return -1;
    }

    private static void EmitLiteral(ScoreContext ctx, string value) =>
        ctx.Children.Add(NodeFactory.Create(NodeType.Text, value, null));
}
