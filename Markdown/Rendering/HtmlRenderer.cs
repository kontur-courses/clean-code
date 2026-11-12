using System.Text;
using Markdown.Syntax;

namespace Markdown.Rendering;

/// <summary>
/// Обработчик AST структуры, оборачивает узлы в Html теги
/// </summary>
public class HtmlRenderer
{
    /// <summary>
    /// Добавляет HTML-представление блока в указанный буфер
    /// </summary>
    /// <param name="output">Текстовый буфер для вывода HTML</param>
    /// <param name="content">Inline-содержимое блока</param>
    /// <param name="blockKind">Тип блока</param>
    /// <param name="headerLevel">Уровень заголовка, если рендерится заголовок</param>
    public void RenderBlock(
        StringBuilder output,
        IReadOnlyList<InlineNode> content,
        HtmlBlockKind blockKind,
        int headerLevel = 1)
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(content);

        switch (blockKind)
        {
            case HtmlBlockKind.Paragraph:
                AppendParagraph(output, content);
                break;
            case HtmlBlockKind.Header:
                AppendHeader(output, headerLevel, content);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(blockKind), blockKind, "Неизвестный тип блока.");
        }
    }

    /// <summary>
    /// Добавляет абзац (<p>)
    /// </summary>
    /// <param name="output">Текстовый буфер для вывода HTML</param>
    /// <param name="content">Содержимое абзаца</param>
    private static void AppendParagraph(
        StringBuilder output,
        IReadOnlyList<InlineNode> content)
    {
        output.Append("<p>");
        AppendInlineNodes(output, content);
        output.Append("</p>");
    }

    /// <summary>
    /// Добавляет заголовок (<h1>, <h2> и т.д.)
    /// </summary>
    /// <param name="output">Текстовый буфер для вывода HTML</param>
    /// <param name="level">Уровень заголовка</param>
    /// <param name="content">Содержимое заголовка</param>
    private static void AppendHeader(
        StringBuilder output,
        int level,
        IReadOnlyList<InlineNode> content)
    {
        if (level is < 1 or > 6)
        {
            throw new ArgumentOutOfRangeException(nameof(level), level, "Уровень заголовка должен быть от 1 до 6");
        }

        output.Append($"<h{level}>");
        AppendInlineNodes(output, content);
        output.Append($"</h{level}>");
    }

    /// <summary>
    /// Обрабатывает inline-узлы
    /// </summary>
    /// <param name="output">Текстовый буфер для вывода HTML</param>
    /// <param name="nodes">Список inline-узлов</param>
    private static void AppendInlineNodes(
        StringBuilder output,
        IReadOnlyList<InlineNode> nodes)
    {
        foreach (var node in nodes)
        {
            switch (node)
            {
                case TextNode textNode:
                    AppendHtmlEscapedText(output, textNode.Text);
                    break;

                case EmphasisNode emphasisNode:
                    output.Append("<em>");
                    AppendInlineNodes(output, emphasisNode.Children);
                    output.Append("</em>");
                    break;

                case StrongNode strongNode:
                    output.Append("<strong>");
                    AppendInlineNodes(output, strongNode.Children);
                    output.Append("</strong>");
                    break;

                default:
                    throw new ArgumentException(
                        $"Неизвестный тип inline-узла: {node.GetType().Name}.",
                        nameof(nodes));
            }
        }
    }

    /// <summary>
    /// Экранирует специальные символы HTML в тексте
    /// </summary>
    /// <param name="output">Текстовый буфер для вывода HTML</param>
    /// <param name="text">Текст для экранирования</param>
    private static void AppendHtmlEscapedText(
        StringBuilder output,
        string text)
    {
        foreach (var character in text)
        {
            switch (character)
            {
                case '&':
                    output.Append("&amp;");
                    break;
                case '<':
                    output.Append("&lt;");
                    break;
                case '>':
                    output.Append("&gt;");
                    break;
                default:
                    output.Append(character);
                    break;
            }
        }
    }
}
