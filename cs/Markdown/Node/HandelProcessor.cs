namespace Markdown;

public static class HandelProcessor
{
    private static readonly IScoreRule[] ScoreRules =
    [
        new BetweenDigitsScoreRule(),
        new NextIsWhitespaceOrEndScoreRule(),
        new NextIsScoreScoreRule(),
        new DefaultScoreRule()
    ];
    public static void Escape(List<Node> children, TokenCursor tokenCursor)
    {
        tokenCursor.Move();
        switch (tokenCursor.Current!.Type)
        {
            case TokenType.NewLine
                or TokenType.EndOfText:
                children.Add(NodeFactory.Create(NodeType.Text, tokenCursor.Current.Value, null));
                return;
            case TokenType.Text:
                children.Add(NodeFactory.Create(NodeType.Text, @"\" + tokenCursor.Current.Value, null));
                break;
            case TokenType.Escape:
            default:
                children.Add(NodeFactory.Create(NodeType.Text, tokenCursor.Current.Value, null));
                break;
        }

        tokenCursor.Move();
    }

    
    public static void Score(List<Node> children, TokenCursor tokenCursor)
    {
        if (ScoreRules.Any(rule => rule.TryApply(children, tokenCursor)));
    }
}