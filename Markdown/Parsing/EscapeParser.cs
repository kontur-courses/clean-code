namespace Markdown;

// Нужен для обработки экранированных символов
public class EscapeParser
{
    // Проверяет, начинается ли в escapeIndex escape-последовательность, и возвращает следующий за '\' символ
    public bool TryGetEscapedCharacter(
        string text,
        int escapeIndex,
        out char escapedCharacter)
    {
        throw new NotImplementedException();
    }
}
