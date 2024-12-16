namespace Markdown.Markdown;

/// <summary>
/// Интерфейс для конвертера Markdown в HTML
/// </summary>
public interface IMd
{
    string Render(string md);
}