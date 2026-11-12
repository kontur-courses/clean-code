using Markdown.Parsers;

namespace Markdown.Interfaces;

public interface IBlockTag
{
    bool CanHandle(string blockText);

    string Render(string blockText, InlineParser inlineParser);
}