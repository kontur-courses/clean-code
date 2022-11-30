using System.Text;
using Markdown.MarkdownDocument;

namespace Markdown.Render;

public class HtmlMdRenderer : IMdRenderer
{
    private readonly Dictionary<string, IReplaceRule> _replaceRules;

    public HtmlMdRenderer(List<IReplaceRule> replaceRules)
    {
        _replaceRules = new Dictionary<string, IReplaceRule>();
        foreach (var rule in replaceRules)
        {
            _replaceRules[rule.TagId] = rule;
        }
    }

    public string Render(MdParsedObjectModel objectModel)
    {
        var sbFull = new StringBuilder();
        sbFull.Append("<!doctype html> <html lang=\"en\"> <body>");
        var sbBody = RenderNodesRecursive(objectModel.Nodes);
        sbFull.Append(sbBody);
        sbFull.Append("</body> </html>");
        return sbFull.ToString();
    }

    private StringBuilder RenderNodesRecursive(List<IDocumentNode> nodes)
    {
        var sbFull = new StringBuilder();
        foreach (var node in nodes)
        {
            if (node is MatchedDocumentNode matchedNode)
            {
                var sbChildren = RenderNodesRecursive(matchedNode.ChildNodes);

                sbFull.Append(_replaceRules.TryGetValue(matchedNode.TagId, out var tagReplaceRule) ? tagReplaceRule.ApplyRule(sbChildren.ToString()) : sbChildren.ToString());
            }
            else
            {
                sbFull.Append(node.GetText());
            }
        }

        return sbFull;
    }
}