using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Markdown.MarkdownDocument;

public class MatchedDocumentNode : IDocumentNode
{
    private readonly List<IDocumentNode> _childNodes;

    private readonly string _tagId;

    public string TagId => _tagId;

    public List<IDocumentNode> ChildNodes => _childNodes;

    public MatchedDocumentNode(string tagId, List<IDocumentNode> childNodes)
    {
        _tagId = tagId;
        _childNodes = childNodes;
    }

    public MatchedDocumentNode(string tagId, IDocumentNode childNode)
    {
        _tagId = tagId;
        _childNodes = new List<IDocumentNode>()
        {
            childNode
        };
    }


    public string GetText()
    {
        var sbText = new StringBuilder();
        if (_childNodes.Any())
        {
            foreach (var node in _childNodes)
            {
                sbText.Append(node.GetText());
            }
        }

        return sbText.ToString();
    }
}