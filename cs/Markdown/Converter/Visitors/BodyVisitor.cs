using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class BodyVisitor : IVisitor
{
    private static readonly Dictionary<TypeOfNode, Func<IVisitor>> SentenceVisitors = new()
    {
        { TypeOfNode.Newline, () => new NewlineVisitor() },
        { TypeOfNode.Heading, () => new HeadingVisitor() },
        { TypeOfNode.Paragraph, () => new ParagraphVisitor() }
    };
    
    public string Visit(Node bodyNode)
    {
        if (bodyNode.Descendants == null) return string.Empty;
        
        var convertedParagraphs = bodyNode.Descendants.Select(paragraph => VisitorFor(paragraph).Visit(paragraph));
        var formattedContent = string.Join(string.Empty, convertedParagraphs);

        return formattedContent;
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