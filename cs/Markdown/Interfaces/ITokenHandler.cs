using Markdown.Parsers;

namespace Markdown.Interfaces;

public interface ITokenHandler
{
    bool CanHandle(char current, char next, MarkdownParseContext context);
    void Handle(MarkdownParseContext context);
}