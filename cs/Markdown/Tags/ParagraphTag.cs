using Markdown.Interfaces;
using Markdown.Parsers;

namespace Markdown.Tags;

public sealed class ParagraphTag : IBlockTag
{
    public bool CanHandle(string blockText) => true;

    public string Render(string blockText, InlineParser inlineParser) =>
        inlineParser.Parse(blockText);
}