using System.Text;
using Markdown.Parser.Nodes;
using Markdown.Parser.Nodes.TagNodes;
using Markdown.Parser.Tools;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Generator.Generators;

public class HtmlGenerator : IGenerator
{
    public string Render(Node root, List<Token> tokens) 
        => RenderSpecificNode(root, tokens); 

    private string RenderSpecificNode(Node node, List<Token> tokens) => node switch
    {
        { NodeType: NodeType.TEXT or NodeType.ESCAPE } => node.ToText(tokens),
        
        SpecNode { Nodes: var nodes } => RenderChildren(nodes, tokens),
        
        TagNode { NodeType: NodeType.BODY, Children: var children } 
            => $"<div>{RenderChildren(children, tokens)}</div>",
        
        TagNode { NodeType: NodeType.ITALIC, Children: var children } 
            => $"<em>{RenderChildren(children, tokens)}</em>",
        
        TagNode { NodeType: NodeType.PARAGRAPH, Children: var children } 
            => $"<p>{RenderChildren(children, tokens)}</p>",
        
        HeadlineNode { Level: var level, Children: var children } 
            => $"<h{level}>{RenderChildren(children, tokens)}</h{level}>",
        
        TagNode { NodeType: NodeType.BOLD, Children: var children } 
            => $"<strong>{RenderChildren(children, tokens)}</strong>",
        
        HrefNode { Href: var href, Children: var children }
            => $"<a href=\"{href}\">{RenderChildren(children, tokens)}</a>",
        
        _ => throw new ArgumentOutOfRangeException(nameof(node))
    };

    private string RenderChildren(List<Node> children, List<Token> tokens)
        => children.Aggregate(new StringBuilder(), (sb, n) => sb.Append(RenderSpecificNode(n, tokens))).ToString();
}