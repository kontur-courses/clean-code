using Markdown.Nodes;
using Markdown.Transformers;

namespace Markdown;

public static class Md
{
    public static string Render(string markdown)
    {
        var blocks = new List<BlockNode>();

        foreach (var raw in ParagraphSplitter.Split(markdown))
        {
            var tokens = StringTokenizer.Tokenize(raw.Content);
            var inlines = TokenParser.Parse(tokens);
            blocks.Add(new TextBlock(raw.Kind,  inlines));
        }
        
        
        
        return HtmlRenderer.Render(new Root(blocks));
    }
}