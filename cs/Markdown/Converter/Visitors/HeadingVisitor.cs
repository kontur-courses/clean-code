using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class HeadingVisitor : IVisitor
{
    private readonly SentenceVisitor sentenceVisitor = new();
    
    public string Visit(Node headingNode)
    {
        if (headingNode.Descendants == null) return string.Empty;
        
        var convertedSentences = headingNode.Descendants.Select(sentence => sentenceVisitor.Visit(sentence));
        var formattedContent = string.Join(string.Empty, convertedSentences);

        return $"<h1>{formattedContent}</h1>";
    }
}