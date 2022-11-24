namespace Markdown;

public class PatternTree
{
    private StateNode currentStateNode;

    public StateNodeType StateNodeType => currentStateNode.StateNodeType;

    public PatternTree(StateNode rootStateNode)
    {
        currentStateNode = rootStateNode;
    }


    public MatchState CheckToken(Token token)
    {
        var checkResult = currentStateNode.CheckToken!(token);
        if (checkResult == NodeCheckResult.SuccessToSelf)
        {
            return MatchState.TokenMatch;
        }

        if (checkResult == NodeCheckResult.NotSuccess)
            return MatchState.NotSuccess;

        currentStateNode = currentStateNode.ChildNode!;

        if (currentStateNode.EndNode)
            return MatchState.FullMatch;

        return MatchState.TokenMatch;
    }

    public MatchState CheckFirstState(Token token)
    {
        var checkResult = currentStateNode.CheckToken!(token);
        return checkResult == NodeCheckResult.NotSuccess ? MatchState.NotSuccess : MatchState.TokenMatch;
    }

    public PatternTree CopyPatternTree()
    {
        return new PatternTree(currentStateNode);
    }
}