namespace Markdown;

public class PatternTree
{
    private Node _currentNode;

    public MatchState CheckToken(Token token)
    {
        var checkResult = _currentNode.CheckToken(token);
        if (checkResult == NodeCheckResult.Success)
        {
            if (_currentNode.EndNode)
                return MatchState.FullMatch;
            else
            {
                _currentNode = _currentNode.ChildNode;
                return MatchState.Process;
            }
        }
        else
        {
            return MatchState.NotSuccess;
        }
    }
}