using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Nodes;

public record TextNode(int Start, int Consumed) : Node(NodeType.TEXT, Start, Consumed)
{
    public string ToText(List<Token> tokens) 
        => tokens.Skip(Start).Take(Consumed).ToList().ToText(); 
}