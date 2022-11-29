using Markdown.Reading;

namespace Markdown.Parsing;

public class StateNode
{
    public StateNode? ChildNode { get; }
    public bool EndNode { get; }
    public StateNodeType StateNodeType { get; }
    public Func<Token, NodeCheckResult>? CheckToken { get; }

    public StateNode(bool endNode)
    {
        EndNode = endNode;
        if (endNode)
        {
            StateNodeType = StateNodeType.End;
            CheckToken = token => NodeCheckResult.End;
        }
        else
        {
            StateNodeType = StateNodeType.Main;
            CheckToken = token => NodeCheckResult.NotSuccess;
        }
    }

    public StateNode(StateNode? childNode,  Func<Token, NodeCheckResult> checkToken)
    {
        ChildNode = childNode;
        CheckToken = checkToken;
        StateNodeType = StateNodeType.Main;
    }

    public StateNode(StateNode? childNode, StateNodeType stateNodeType, Func<Token, NodeCheckResult> checkToken)
    {
        ChildNode = childNode;
        CheckToken = checkToken;
        StateNodeType = stateNodeType;
    }
}