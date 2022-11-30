using Markdown.Reading;

namespace Markdown.Parsing;

public class PatternTree
{
    private List<StateNode> currentStateNodes;


    public PatternTree(StateNode rootStateNodes)
    {
        currentStateNodes = new List<StateNode>()
        {
            rootStateNodes
        };
    }

    private PatternTree(List<StateNode> rootStateNodes)
    {
        currentStateNodes = rootStateNodes;
    }

    public List<StateNodeType> GetCurrentStateNodeTypes()
    {
        return currentStateNodes.Select(x => x.StateNodeType).ToList();
    }

    public MatchState CheckToken(Token token)
    {
        var children = new List<StateNode>();
        var states = new List<MatchState>();
        foreach (var currentNode in currentStateNodes)
        {
            var checkResult = currentNode.CheckToken!(token);
            if (checkResult == NodeCheckResult.SuccessToSelf)
            {
                children.Add(currentNode);
                if (currentNode.ChildNode != null)
                    children.Add(currentNode.ChildNode);

                states.Add(MatchState.TokenMatch);
                continue;
            }

            if (checkResult == NodeCheckResult.Success)
            {
                children.Add(currentNode.ChildNode!);
                states.Add(MatchState.TokenMatch);
            }

            if (checkResult == NodeCheckResult.NotSuccess)
            {
                states.Add(MatchState.NotSuccess);
            }
        }

        currentStateNodes = children;

        if (states.All(x => x == MatchState.NotSuccess))
            return MatchState.NotSuccess;

        if (children.Any(x => x.EndNode))
            return MatchState.FullMatch;

        return MatchState.TokenMatch;
    }

    public MatchState CheckFirstState(Token token)
    {
        foreach (var node in currentStateNodes)
        {
            var checkResult = node.CheckToken!(token);
            if (checkResult != NodeCheckResult.NotSuccess)
                return MatchState.TokenMatch;
        }

        return MatchState.NotSuccess;
    }

    public PatternTree CopyPatternTree()
    {
        return new PatternTree(currentStateNodes);
    }
}