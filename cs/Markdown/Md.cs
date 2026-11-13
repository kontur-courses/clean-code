using Markdown.Blocks;
using Markdown.Pairing;
using Markdown.Rendering;
using Markdown.Syntax;
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
        var lines = lineSplitter.Split(markdown);
        var blocks = blockParser.Parse(lines);
        var htmlBlocks = new List<string>(blocks.Count);

        foreach (var block in blocks)
        {
            var tokens = tokenizer.Tokenize(block.Content);
            tagPairer.Pair(tokens);
            var tree = treeBuilder.Build(tokens);
            htmlBlocks.Add(renderer.Render(block.Kind, tree));
        }

        return string.Join(LineBreaks.NewLineSequence, htmlBlocks);
    }
}