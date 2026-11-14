using Markdown.Rendering;
using Markdown.Syntax;

namespace Markdown;

/// <summary>
/// Распасшенный .md документ. Парсинг выполняется один раз,
/// и документ можно отображать в необходимом формате
/// </summary>
public sealed class MarkdownDocument
{
    private readonly DocumentNode syntaxTree;

    internal MarkdownDocument(DocumentNode syntaxTree)
    {
        this.syntaxTree = syntaxTree;
    }

    public string ToHtml()
    {
        return HtmlRenderer.Render(syntaxTree);
    }
}
