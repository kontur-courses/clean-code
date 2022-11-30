namespace Markdown.Parsing.Tags;

public class BoldMdTag : GeneralMdTag
{
    public BoldMdTag() : base("__", CreatePatterns(), escapeSymbol: '_', canBeParsed: true)
    {
    }


    private static PatternTree[] CreatePatterns()
    {
        return new[]
        {
            FirstBoldPattern(), SecondBoldPattern()
        };
    }


    public override string ClearText(string text)
    {
        return text.Remove(text.Length - 2, 2).Remove(0, 2);
        // return text.Remove(text.Length - 3, 2).Remove(0, 2);
    }


    private static PatternTree FirstBoldPattern()
    {
        var endNode = new StateNode(true);

        var lookaheadNode = new StateNode(endNode, StateNodeType.Lookahead, token =>
        {
            if (token.Symbol == '\0' || token.Symbol == ' ' || token.Symbol == '\n' || token.Symbol == '\r')
                return NodeCheckResult.Success;

            return NodeCheckResult.NotSuccess;
        });

        var secondEndTag = new StateNode(lookaheadNode, token => token.Symbol == '_' ? NodeCheckResult.Success : NodeCheckResult.NotSuccess);
        var firstEnd = new StateNode(secondEndTag, token =>
        {
            if (token.Symbol == '_')
                return NodeCheckResult.Success;

            return NodeCheckResult.NotSuccess;
        });

        var middleNodeThird = new StateNode(firstEnd, token =>
        {
            return char.IsLetterOrDigit(token.Symbol)
                ? NodeCheckResult.SuccessToSelf
                : NodeCheckResult.NotSuccess;
        });

        var middleNodeSecond = new StateNode(middleNodeThird, token =>
        {
            return char.IsLetterOrDigit(token.Symbol) || token.Symbol == ' '
                ? NodeCheckResult.SuccessToSelf
                : NodeCheckResult.NotSuccess;
        });

        var middleNodeFirst = new StateNode(middleNodeSecond, token =>
        {
            // if (token.Symbol == '_')
            //     return NodeCheckResult.NotSuccess;

            return char.IsLetterOrDigit(token.Symbol) ? NodeCheckResult.Success : NodeCheckResult.NotSuccess;
        });

        var secondStartTag = new StateNode(middleNodeFirst, token => token.Symbol == '_' ? NodeCheckResult.Success : NodeCheckResult.NotSuccess);
        var startNode = new StateNode(secondStartTag, StateNodeType.StartPosition, token => token.Symbol == '_' ? NodeCheckResult.Success : NodeCheckResult.NotSuccess);

        var rootNode = new StateNode(startNode, StateNodeType.Lookbehind, token =>
        {
            if (token.Symbol == '\0' || token.Symbol == ' ' || token.Symbol == '\n' || token.Symbol == '\r')
                return NodeCheckResult.Success;

            return NodeCheckResult.NotSuccess;
        });

        return new PatternTree(rootNode);
    }

    private static PatternTree SecondBoldPattern()
    {
        var endNode = new StateNode(true);

        var lookaheadNode = new StateNode(endNode, StateNodeType.Lookahead, token =>
        {
            if (token.Symbol == '\0' || token.Symbol == ' ' || token.Symbol == '\n' || token.Symbol == '\r')
                return NodeCheckResult.Success;

            return NodeCheckResult.NotSuccess;
        });

        var secondEndTag = new StateNode(lookaheadNode, token => token.Symbol == '_' ? NodeCheckResult.Success : NodeCheckResult.NotSuccess);
        var firstEnd = new StateNode(secondEndTag, token =>
        {
            if (token.Symbol == '_')
                return NodeCheckResult.Success;

            return NodeCheckResult.NotSuccess;
        });

        var middleNodeFirst = new StateNode(firstEnd, token =>
        {
            if (token.Symbol == '\r' || token.Symbol == '\n')
                return NodeCheckResult.NotSuccess;

            return !char.IsWhiteSpace(token.Symbol) ? NodeCheckResult.SuccessToSelf : NodeCheckResult.NotSuccess;
        });

        var secondStartTag = new StateNode(middleNodeFirst, token => token.Symbol == '_' ? NodeCheckResult.Success : NodeCheckResult.NotSuccess);
        var startNode = new StateNode(secondStartTag, StateNodeType.StartPosition, token => token.Symbol == '_' ? NodeCheckResult.Success : NodeCheckResult.NotSuccess);

        var rootNode = new StateNode(startNode, StateNodeType.Lookbehind, token =>
        {
            if (token.Symbol == '\0' || token.Symbol == ' ' || token.Symbol == '\n' || token.Symbol == '\r')
                return NodeCheckResult.Success;

            return NodeCheckResult.NotSuccess;
        });

        return new PatternTree(rootNode);
    }

    private static PatternTree ThirdBoldPattern()
    {
        var endNode = new StateNode(true);
        var secondEndTagNode = new StateNode(endNode, token => token.Symbol == '_' ? NodeCheckResult.Success : NodeCheckResult.NotSuccess);

        var middleNodeSecond = new StateNode(secondEndTagNode, token =>
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

        var startPositionSecondNode = new StateNode(middleNodeFirst,
            token => token.Symbol == '_' ? NodeCheckResult.Success : NodeCheckResult.NotSuccess);
        var startPositionNode = new StateNode(startPositionSecondNode, StateNodeType.StartPosition,
            token => token.Symbol == '_' ? NodeCheckResult.Success : NodeCheckResult.NotSuccess);

        return new PatternTree(startPositionNode);
    }
}