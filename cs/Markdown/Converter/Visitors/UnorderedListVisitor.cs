using System;
using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class UnorderedListVisitor : IVisitor
{
    private readonly ListItemVisitor listItemVisitor = new();
    
    public string Visit(Node unorderedListNode)
    {
        if (unorderedListNode.Descendants == null) return string.Empty;
        
        var convertedListItems = unorderedListNode.Descendants.Select(listItem => listItemVisitor.Visit(listItem));
        var formattedContent = string.Join(string.Empty, convertedListItems);

        return $"<ul>{formattedContent}</ul>";
    }
}