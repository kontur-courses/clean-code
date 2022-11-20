namespace Markdown;

public class Node
{
    public Node ChildNode { get; }

    public bool EndNode { get; }
    public Func<Token, NodeCheckResult> CheckToken { get; }


    public Node(Node childNode, bool endNode, Func<Token, NodeCheckResult> checkToken)
    {
        ChildNode = childNode;
        EndNode = endNode;
        CheckToken = checkToken;
    }
}