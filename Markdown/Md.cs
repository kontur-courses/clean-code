using Markdown.Model;
using Markdown.Transformers;

namespace Markdown;

public static class Md
{
    public static string Convert(string markdown)
    {
        var tokens = StringTokenizer.Tokenize(markdown);

        var nodes = TokenParser.Parse(tokens);

        var document = HtmlDocument.CreateHtmlDocument();
        document.GetBodyElement().Add(HtmlBuilder.ToElements(nodes));

        return HtmlSerializer.ToHtmlString(document);
    }
}