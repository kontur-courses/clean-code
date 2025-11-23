namespace Markdown;

public interface INodeGenerator
{
    public List<Node> Create(List<Token> tokens);
}