namespace Markdown;

public class NodeGenerator : INodeGenerator
{
    public List<Node> Create(List<Token> tokens)
    {
        var tokenCursor = new TokenCursor(tokens);
        var nods = new List<Node>();
        while (!tokenCursor.End)
            nods.Add(CreateNode(tokenCursor));
        return nods;
    }

    private static Node CreateNode(TokenCursor tokenCursor)
    {
        switch (tokenCursor.Current)
        {
            case { Type: TokenType.NewLine }:
            {
                var newLineNode = NodeFactory.Create(NodeType.Text, tokenCursor.Current.Value, null);
                tokenCursor.Move();
                return newLineNode;
            }
            case { Type: TokenType.HeaderMarker } when tokenCursor.TakeNext() is { Type: TokenType.WhiteSpace }:
                tokenCursor.Move(2);
                return NodeFactory.Create(NodeType.Header, null, GetChildrenNods(tokenCursor));
            default:
                return NodeFactory.Create(NodeType.Text, null, GetChildrenNods(tokenCursor));
        }
    }

    private static List<Node> GetChildrenNods(TokenCursor tokenCursor)
    {
        var children = new List<Node>();
        while (!tokenCursor.End &&
               tokenCursor.Current is not { Type: TokenType.NewLine })
        {
            switch (tokenCursor.Current!.Type)
            {
                case TokenType.Underscore:
                case TokenType.DoubleUnderscore:
                    HandelProcessor.Score(children, tokenCursor);
                    break;
                case TokenType.Escape:
                    HandelProcessor.Escape(children, tokenCursor);
                    break;
                case TokenType.Link:
                    children.Add(NodeFactory.Create(NodeType.Link, tokenCursor.Current.Value, null));
                    tokenCursor.Move();
                    break;
                case TokenType.NewLine:
                case TokenType.EndOfText:
                case TokenType.Text:
                case TokenType.WhiteSpace:
                case TokenType.HeaderMarker:
                default:
                    children.Add(NodeFactory.Create(NodeType.Text, tokenCursor.Current.Value, null));
                    tokenCursor.Move();
                    break;
            }
        }
        
        return children;
    }
}