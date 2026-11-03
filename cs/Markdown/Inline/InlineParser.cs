namespace Markdown.Inline
{
    public class InlineParser
    {
        private readonly List<IInlineHandler> handlers;

        public InlineParser()
        {
            // Инициализируем список

        }

        // Обрабатывает текст без блочных тегов
        public string ConvertToHtml(string text)
        {
            // Идем циклом по тексту. 
            // Для каждого символа пытаемся применить обработчики из списка.
            // Если никто не обработал — просто добавляем символ в результат.
            throw new NotImplementedException();
        }

    }
}
