using System;
using System.Collections.Generic;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class SentenceVisitor : IVisitor
{
    private static readonly Dictionary<TypeOfNode, Func<IVisitor>> SentenceVisitors = new()
    {
        { TypeOfNode.UnorderedList, () => new UnorderedListVisitor() },
        { TypeOfNode.Intersection, () => new IntersectionVisitor() },
        { TypeOfNode.Strong, () => new StrongVisitor() },
        { TypeOfNode.Emphasis, () => new EmphasisVisitor() },
        { TypeOfNode.Text, () => new TextVisitor() }
    };

    public string Visit(Node node)
    {
        return VisitorFor(node).Visit(node);
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