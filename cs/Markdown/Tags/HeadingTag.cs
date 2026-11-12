using Markdown.Interfaces;
using Markdown.Parsers;

namespace Markdown.Tags;

public sealed class HeadingTag : IBlockTag
{
    public bool CanHandle(string blockText) => blockText.StartsWith("# ");

    public string Render(string blockText, InlineParser inlineParser)
    {
        var content = inlineParser.Parse(blockText[2..]);
        return $"<h1>{content}</h1>";
    }
}