using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class EmphasisVisitor : IVisitor
{
    private static readonly Dictionary<TypeOfNode, Func<IVisitor>> SentenceVisitors = new()
    {
        { TypeOfNode.Emphasis, () => new EmphasisVisitor() },
        { TypeOfNode.Text, () => new TextVisitor() }
    };
    
    public string Visit(Node emphasisNode)
    {
        if (emphasisNode.Descendants == null) return string.Empty;
        
        var convertedSentences = emphasisNode.Descendants.Select(sentence => VisitorFor(sentence).Visit(sentence));
        var formattedContent = string.Join(string.Empty, convertedSentences);

        return $"<em>{formattedContent}</em>";
    }
    
    private static IVisitor VisitorFor(Node node)
    {
        if (SentenceVisitors.TryGetValue(node.Type, out var visitorFactory))
        {
            return visitorFactory();
        }
        throw new ArgumentException("Invalid sentence node type");
    }
}