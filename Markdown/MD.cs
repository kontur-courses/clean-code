using Markdown.Parsing;

namespace Markdown;

public static class Md
{
    /// <summary>
    /// Парсит текст в документ, из которого можно получить HTML:
    /// <c>Md.Parse(text).ToHtml()</c>.
    /// </summary>
    public static MarkdownDocument Parse(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        var tokens = MarkdownLexer.Tokenize(text);
        var syntaxTree = MarkdownParser.Parse(tokens);

        return new MarkdownDocument(syntaxTree);
    }

    /// <summary>Сокращение для <c>Md.Parse(text).ToHtml()</c>.</summary>
    public static string Render(string text)
    {
        return Parse(text).ToHtml();
    }
}