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
        foreach (var node in objectModel.Nodes)
        {
            if (node is MatchedDocumentNode matchedNode)
            {
                sbFull.Append(_replaceRules.TryGetValue(matchedNode.TagId, out var tagReplaceRule) ? tagReplaceRule.ApplyRule(matchedNode.GetText()) : node.GetText());
            }
            else
            {
                sbFull.Append(node.GetText());
            }
        }

        sbFull.Append("</body> </html>");
        return sbFull.ToString();
    }
}