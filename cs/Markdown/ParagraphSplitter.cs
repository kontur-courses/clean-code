namespace Markdown;

/// <summary>Gap - пробельный текст перед абзацем, Text это сам абзац</summary>
internal readonly record struct Segment(string Gap, string Text);

/// <summary>
/// Делит текст на абзацы (разделитель - пустая строка), сохраняя все разделители
/// Используется только в Md, не зависит от других классов
/// </summary>
internal static class ParagraphSplitter
{
    public static List<Segment> Split(string text)
    {
        throw new NotImplementedException();
    }
    
}