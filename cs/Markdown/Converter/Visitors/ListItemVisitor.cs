using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class ListItemVisitor : IVisitor
{
    private readonly SentenceVisitor sentenceVisitor = new();
    
    public string Visit(Node listItemNode)
    {
        if (listItemNode.Descendants == null) return string.Empty;
        
        var convertedSentences = listItemNode.Descendants.Select(sentence => sentenceVisitor.Visit(sentence));
        var formattedContent = string.Join(string.Empty, convertedSentences);

        return $"<li>{formattedContent}</li>";
    }
}