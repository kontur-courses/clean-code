namespace Markdown
{
    public interface IInlineHandler
    {
        bool TryConvertToHtml(string text, ref int position, out string result);
        IInlineHandler SetNextHandler(IInlineHandler nextHandler);
    }

    // Обработчик экранирования (\_)
    public class EscapeHandler : IInlineHandler
    {
        public bool TryConvertToHtml(string text, ref int position, out string result)
        {
            // Если текущий символ '\', проверить следующий.
            // Если следующий символ - спецсимвол (_, \, #), вернуть его как текст без слэша.
            throw new System.NotImplementedException();
        }

        public IInlineHandler SetNextHandler(IInlineHandler nextHandler)
        {
            throw new System.NotImplementedException();
        }
    }

    // Обработчик курсива (_)
    public class ItalicHandler : IInlineHandler
    {
        public bool TryConvertToHtml(string text, ref int position, out string result)
        {
            // Проверить условия из ТЗ:
            // 1. Слева/справа не цифры.
            // 2. Не пустое внутри (____).
            // 3. После открывающего _ идет не пробел. До закрывающего _ идет не пробел.
            // 4. Символы в рамках одного слова.
            throw new System.NotImplementedException();
        }

        public IInlineHandler SetNextHandler(IInlineHandler nextHandler)
        {
            throw new System.NotImplementedException();
        }
    }

    // Обработчик полужирного (__)
    public class StrongHandler : IInlineHandler
    {
        public bool TryConvertToHtml(string text, ref int position, out string result)
        {
            // Проверить, что начинается с "__", найти закрывающие "__".
            // Обработать взаимодействие с _ (вызов ItalicHandler внутри).
            throw new System.NotImplementedException();
        }

        public IInlineHandler SetNextHandler(IInlineHandler nextHandler)
        {
            throw new System.NotImplementedException();
        }
    }

    // Обработчик ссылок [текст](url)
    public class LinkHandler : IInlineHandler
    {
        public bool TryConvertToHtml(string text, ref int position, out string result)
        {
            // Проверить, начинается ли текущий символ с '['.
            // Найти закрывающую ']', затем проверить наличие '(' и ')'.
            // Сформировать HTML-тег <a href="url">текст</a>.
            throw new System.NotImplementedException();
        }

        public IInlineHandler SetNextHandler(IInlineHandler nextHandler)
        {
            throw new System.NotImplementedException();
        }
    }
}
