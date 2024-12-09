using Markdown.Tokens;

namespace Markdown.Parser.Nodes;

public record TextNode(int Start, int Consumed) : Node(NodeType.Text, Start, Consumed)
{
    public string ToText(List<Token> tokens) 
        => tokens.Skip(Start).Take(Consumed).ToList().ToText(); 
}