using System.Text;
using Markdown.MarkdownDocument;

namespace Markdown.Render;

public class HtmlMdRenderer : IMdRenderer
{
    public string Render(MdParsedObjectModel objectModel)
    {
        var sbFull = new StringBuilder();
        sbFull.Append("<!doctype html> <html lang=\"en\"> <body>");
        foreach (var node in objectModel.Nodes)
        {
            sbFull.Append(node.GetText());
        }

        sbFull.Append("</body> </html>");
        return sbFull.ToString();
    }
}