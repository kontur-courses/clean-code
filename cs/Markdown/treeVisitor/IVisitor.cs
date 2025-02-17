using System.Text;

namespace Markdown.treeVisitor;

public interface IVisitor
{
    public string Convert(HeaderNode headerNode);

    string Convert(TextNode textNode);
    string Convert(ItalicNode ItalicNode);
    string Convert(NewLineNode newLineNode);
    string Convert(LineNode lineNodeNode);
    string Convert(DigitNode lineNodeNode);
    string Convert(BoldNode boldNode);
    string Convert(MarkerRangeNode lineNodeNode);
    string Convert(MarkerNode markerNode);
}

public class HtmlVisitor : IVisitor // по сути, visitor, как опасный грибок мицелий, он попадает в объект, а потом его съедает и уже он управляет им, а не оъект им.
{
    public string Convert(HeaderNode headerNode)
    {
        var innerText = InnerText(headerNode);
        return $"<h1>{innerText}</h1>{headerNode.NextNode?.Convert(this) ?? ""}";
    }

    public string Convert(LineNode lineNodeNode)
    {
        var innerText = InnerText(lineNodeNode);
        var text = lineNodeNode.NextNode;
        return $"{innerText}{text?.Convert(this) ?? ""}";
    }

    public string Convert(DigitNode lineNodeNode)
    {
        return lineNodeNode.Digit.ToString();
    }

    public string Convert(BoldNode boldNode)
    {
        var innerText = InnerText(boldNode);

        return $"<strong>{innerText}</strong>{boldNode.NextNode?.Convert(this) ?? ""}";
    }

    public string Convert(MarkerRangeNode lineNodeNode)
    {
        var innerText = InnerText(lineNodeNode);

        return $"<ul>\n{innerText}</ul>{lineNodeNode.NextNode?.Convert(this) ?? ""}";
    }

    public string Convert(MarkerNode markerNode)
    {
        var innerText = InnerText(markerNode);
        
        return $"<li>{innerText}</li>{markerNode.NextNode?.Convert(this) ?? ""}";
    }

    public string Convert(TextNode textNode)
    {
        return textNode.Text;
    }

    public string Convert(NewLineNode newLineNode)
    {
        var innerText = InnerText(newLineNode);
        
        return $"\n{innerText}{newLineNode.NextNode?.Convert(this) ?? ""}";
    }

    public string Convert(ItalicNode italicNode)
    {
        var innerText = InnerText(italicNode);

        return $"<em>{innerText}</em>{italicNode.NextNode?.Convert(this) ?? ""}";
    }
    
    
    private string InnerText(INode lineNodeNode)
    {
        var innerText = new StringBuilder();
        foreach (var node in lineNodeNode.InnerNode)
        {
            innerText.Append(node.Convert(this));
        }

        return innerText.ToString();
    }
}
