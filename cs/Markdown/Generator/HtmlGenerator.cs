using System.Text;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Generator;

public class HtmlGenerator : IGenerator
{
    public string Render(Node? root, List<Token> tokens) 
        => RenderSpecificNode(root, tokens); 
    private string RenderSpecificNode(Node? node, List<Token> tokens) => node switch
    {
        { NodeType: NodeType.Text or NodeType.Escape } => node.ToText(tokens),
        
        SpecNode { Nodes: var nodes } => RenderChildren(nodes, tokens),
        
        TagNode { NodeType: NodeType.Body, Children: var children } 
            => $"<div>{RenderChildren(children, tokens)}</div>",
        
        TagNode { NodeType: NodeType.Italic, Children: var children } 
            => $"<em>{RenderChildren(children, tokens)}</em>",
        
        TagNode { NodeType: NodeType.Paragraph, Children: var children } 
            => $"<p>{RenderChildren(children, tokens)}</p>",
        
        TagNode { NodeType: NodeType.UnorderedList, Children: var children }
            => $"<ul>{RenderChildren(children, tokens)}</ul>",
        
        TagNode { NodeType: NodeType.ListItem, Children: var children }
            => $"<li>{RenderChildren(children, tokens)}</li>",
        
        TagNode { NodeType: NodeType.Header, Children: var children } 
            => $"<h1>{RenderChildren(children, tokens)}</h1>",
        
        TagNode { NodeType: NodeType.Bold, Children: var children } 
            => $"<strong>{RenderChildren(children, tokens)}</strong>",
        
        _ => throw new ArgumentOutOfRangeException(nameof(node))
    };
    private string RenderChildren(List<Node> children, List<Token> tokens)
        => children.Aggregate(new StringBuilder(), (sb, n) => sb.Append(RenderSpecificNode(n, tokens))).ToString();
}