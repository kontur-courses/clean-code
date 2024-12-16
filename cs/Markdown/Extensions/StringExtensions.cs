namespace Markdown.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Производит проверку наличия строки в строке на позиции position без копирования
    /// </summary>
    /// <param name="origin">Исходная строка</param>
    /// <param name="substring">Проверяемая строка</param>
    /// <param name="position">Позиция в исходной строке</param>
    /// <returns>True, если строка содержится в исходной строке на позиции position, иначе false</returns>
    public static bool ContainsSubstringOnIndex(this string origin, string substring, int position)
    {
        for (var j = 0; j < substring.Length; j++)
        {
            if (position + j >= origin.Length || origin[position + j] != substring[j])
                return false;
        }

        return true;
    }

    /// <summary>
    /// Проверяет, является ли символ экранированным
    /// </summary>
    /// <param name="str">Строка</param>
    /// <param name="position">Индекс символа</param>
    /// <returns>True, если символ экранирован, иначе false</returns>
    public static bool IsEscaped(this string str, int position)
    {
        var previousIndex = position - 1;
        var backslashCount = 0;
        while (previousIndex >= 0 && str[previousIndex] == '\\')
        {
            backslashCount++;
            previousIndex--;
        }

        return backslashCount % 2 == 1;
    }
}