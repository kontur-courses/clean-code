namespace Markdown;

public class InlineTokenizer
{
    public IReadOnlyList<Token> Tokenize(
        string text,
        int startIndex = 0)
    {
        // Проходим по строке от startIndex до text.Length.

        // Если встретили экранирование
        // создаём Escape
        // пропускаем символ

        // Если встретили подчёрки
        // определяем вид маркера;
        // проверяем локальную возможность открытия/закрытия; (предыдущий символ не пробел, следующий символ не пробел)
        // создаём токен с позицией и длиной.

        // Остальной текст пропускаем.
        // Возвращаем токены в порядке появления.

        throw new NotImplementedException();
    }
}