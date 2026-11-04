using System.Text;
using Markdown.Syntax;

namespace Markdown.Rendering;

/// <summary>
/// Тут уже рендеринг в Html по построенному дереву
/// </summary>
public class HtmlRenderer
{
    /// <summary>
    /// Добавляет абзац (<p>)
    /// </summary>
    /// <param name="output">Текстовый буфер для вывода HTML</param>
    /// <param name="content">Содержимое абзаца</param>
    public void AppendParagraph(
        StringBuilder output,
        IReadOnlyList<InlineNode> content)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Добавляет заголовок (<h1>, <h2> и т.д.)
    /// </summary>
    /// <param name="output">Текстовый буфер для вывода HTML</param>
    /// <param name="level">Уровень заголовка</param>
    /// <param name="content">Содержимое заголовка</param>
    public void AppendHeader(
        StringBuilder output,
        int level,
        IReadOnlyList<InlineNode> content)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Обрабатывает inline-узлы
    /// </summary>
    /// <param name="output">Текстовый буфер для вывода HTML</param>
    /// <param name="nodes">Список inline-узлов</param>
    private void AppendInlineNodes(
        StringBuilder output,
        IReadOnlyList<InlineNode> nodes)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Экранирует специальные символы HTML в тексте
    /// </summary>
    /// <param name="output">Текстовый буфер для вывода HTML</param>
    /// <param name="text">Текст для экранирования</param>
    private void AppendHtmlEscapedText(
        StringBuilder output,
        string text)
    {
        throw new NotImplementedException();
    }
}
