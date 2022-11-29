using System.Text;

namespace Markdown.MarkdownDocument;

public class PlaintextDocumentNode : IDocumentNode
{
    private readonly StringBuilder _text;

    public PlaintextDocumentNode(StringBuilder text)
    {
        _text = text;
    }

    public PlaintextDocumentNode(string text)
    {
        _text = new StringBuilder(text);
    }


    public string GetText()
    {
        return _text.ToString();
    }
}