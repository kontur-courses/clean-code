using Markdown.Builders;
using Markdown.Parsers;

namespace Markdown;

public static class Markdown
{
    public static string Render(string text)
    {
        var parser = new TextParser();
        var builder = new MarkdownToHtmlBuilder();

        var textTokens = parser.ParseMarkdownText(text);

        return builder.Build(textTokens);
    }
}