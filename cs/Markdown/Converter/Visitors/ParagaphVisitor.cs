using System;
using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class ParagraphVisitor : IVisitor
{
    private readonly SentenceVisitor sentenceVisitor = new();
    
    public string Visit(Node paragraphNode)
    {
        if (paragraphNode.Descendants == null) return string.Empty;
        
        var convertedParagraphs = paragraphNode.Descendants.Select(sentence => sentenceVisitor.Visit(sentence));
        var formattedContent = string.Join(string.Empty, convertedParagraphs);

        return formattedContent;
    }
}