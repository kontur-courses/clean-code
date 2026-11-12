using System.Text;
using Markdown.Rendering;

namespace Markdown.Parsing;

/// <summary>
/// Распознаёт блочную разметку и передаёт содержимое блоков в InlineParser
/// </summary>
public class BlockParser()
{
    private readonly InlineParser inlineParser = new();
    private readonly HtmlRenderer htmlRenderer = new();

    /// <summary>
    /// Разбирает текст на блоки и возвращает их HTML-представление
    /// </summary>
    /// <param name="text">Исходный текст в упрощённом Markdown</param>
    /// <returns>HTML-код абзацев и заголовков</returns>
    public string Parse(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        var html = new StringBuilder();
        var currentBlock = new StringBuilder();

        using var reader = new StringReader(text);
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                AppendBlock(html, currentBlock);
                continue;
            }

            if (currentBlock.Length > 0)
            {
                currentBlock.Append('\n');
            }

            currentBlock.Append(line);
        }

        AppendBlock(html, currentBlock);
        return html.ToString();
    }

    /// <summary>
    /// Разбирает и добавляет в HTML-буфер накопленный блок
    /// </summary>
    /// <param name="html">Буфер с HTML-результатом</param>
    /// <param name="blockText">Текст блока</param>
    private void AppendBlock(StringBuilder html, StringBuilder blockText)
    {
        if (blockText.Length == 0)
        {
            return;
        }

        var content = blockText.ToString();
        var blockKind = HtmlBlockKind.Paragraph;

        if (content.StartsWith("# "))
        {
            content = content[2..];
            blockKind = HtmlBlockKind.Header;
        }

        var inlineNodes = inlineParser.Parse(content);
        htmlRenderer.RenderBlock(html, inlineNodes, blockKind);
        blockText.Clear();
    }
}
