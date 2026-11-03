namespace Markdown;

internal readonly record struct LinkMatch(string Text, string Url, int End);

/// <summary>
/// Распознаёт [текст](адрес) с позиции [
/// Не зависит от других классов
/// </summary>
internal static class LinkParser
{
    public static bool TryParse(string text, int start, out LinkMatch link)
    {
        throw new NotImplementedException();
    }
}