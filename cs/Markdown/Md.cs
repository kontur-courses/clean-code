using Markdown.Blocks;
using Markdown.Pairing;
using Markdown.Rendering;
using Markdown.Tokens;
using Markdown.Tree;

namespace Markdown;

/// <summary>
///     LineSplitter → BlockParser → Tokenizer → TagPairer → TreeBuilder → HtmlRenderer.
/// </summary>
public class Md
{
    private readonly BlockParser blockParser = new();
    private readonly LineSplitter lineSplitter = new();
    private readonly HtmlRenderer renderer = new();
    private readonly TagPairer tagPairer = new();
    private readonly Tokenizer tokenizer = new();
    private readonly TreeBuilder treeBuilder = new();

    public string Render(string markdown)
    {
        var blocks = blockParser.Parse(lineSplitter.Split(markdown));
        var html = new List<string>(blocks.Count);

        foreach (var block in blocks)
        {
            var tokens = tokenizer.Tokenize(block.Content);
            tagPairer.Pair(tokens);
            html.Add(renderer.Render(block.Kind, treeBuilder.Build(tokens)));
        }

        return string.Join("\n", html);
    }
}