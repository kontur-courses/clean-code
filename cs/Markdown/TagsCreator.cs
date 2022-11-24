namespace Markdown;

public static class TagsCreator
{
    public static List<Tag> CreateDefaultTags()
    {
        return new List<Tag>()
        {
            CreateItalicTag()
        };
    }


    public static Tag CreateItalicTag()
    {
        return new Tag("_", new[]
        {
            FirstItalicPattern(), SecondItalicPattern()
        });
    }


    private static PatternTree FirstItalicPattern()
    {
        var endNode = new StateNode(true);

        var lookaheadNode = new StateNode(endNode, StateNodeType.Lookahead, token =>
        {
            if (token.Symbol == '\0' || token.Symbol == ' ' || token.Symbol == '\n' || token.Symbol == '\r')
                return NodeCheckResult.Success;

            return NodeCheckResult.NotSuccess;
        });

        var middleNodeSecond = new StateNode(lookaheadNode, token =>
        {
            if (token.Symbol == '_')
                return NodeCheckResult.Success;

            return char.IsLetter(token.Symbol) || char.IsWhiteSpace(token.Symbol)
                ? NodeCheckResult.SuccessToSelf
                : NodeCheckResult.NotSuccess;
        });

        var middleNodeFirst = new StateNode(middleNodeSecond, token =>
        {
            if (token.Symbol == '_')
                return NodeCheckResult.NotSuccess;

            return char.IsLetter(token.Symbol) ? NodeCheckResult.Success : NodeCheckResult.NotSuccess;
        });

        var stateNode = new StateNode(middleNodeFirst, StateNodeType.StartPosition, token => token.Symbol == '_' ? NodeCheckResult.Success : NodeCheckResult.NotSuccess);

        var rootNode = new StateNode(stateNode, StateNodeType.Lookbehind, token =>
        {
            if (token.Symbol == '\0' || token.Symbol == ' ' || token.Symbol == '\n' || token.Symbol == '\r')
                return NodeCheckResult.Success;

            return NodeCheckResult.NotSuccess;
        });

        return new PatternTree(rootNode);
    }


    private static PatternTree SecondItalicPattern()
    {
        var endNode = new StateNode(true);

        var lookaheadNode = new StateNode(endNode, StateNodeType.Lookahead, token => token.Symbol == '_' ? NodeCheckResult.NotSuccess : NodeCheckResult.Success);

        var middleNodeSecond = new StateNode(lookaheadNode, token =>
        {
            if (token.Symbol == '_')
                return NodeCheckResult.Success;

            return char.IsLetter(token.Symbol)
                ? NodeCheckResult.SuccessToSelf
                : NodeCheckResult.NotSuccess;
        });

        var middleNodeFirst = new StateNode(middleNodeSecond, token =>
        {
            if (token.Symbol == '_')
                return NodeCheckResult.NotSuccess;

            return char.IsLetter(token.Symbol) ? NodeCheckResult.Success : NodeCheckResult.NotSuccess;
        });

        var startPositionNode = new StateNode(middleNodeFirst, StateNodeType.StartPosition, token => token.Symbol == '_' ? NodeCheckResult.Success : NodeCheckResult.NotSuccess);

        var rootNode = new StateNode(startPositionNode, StateNodeType.Lookbehind, token =>
        {
            if (token.Symbol == '_' || token.Symbol == '/')
                return NodeCheckResult.NotSuccess;

            return NodeCheckResult.Success;
        });

        return new PatternTree(rootNode);
    }
}