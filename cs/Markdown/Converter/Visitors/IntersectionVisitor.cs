using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class IntersectionVisitor : IVisitor
{
    private readonly TextVisitor textVisitor = new();
    public string Visit(Node intersectionNode)
    {
        if (intersectionNode.Descendants == null) return string.Empty;
        
        var convertedParagraphs = intersectionNode.Descendants.Select(sentence => textVisitor.Visit(sentence));
        var formattedContent = string.Join(string.Empty, convertedParagraphs);

        return formattedContent;
    }
}