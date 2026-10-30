namespace Markdown
{
    public class InlineParser
    {
        private readonly IInlineHandler firstHandler;

        public InlineParser()
        {
            // Собрать цепочку: EscapeHandler -> StrongHandler -> ItalicHandler
            firstHandler = null;
        }

        // Обрабатывает текст без блочных тегов
        public string ConvertToHtml(string text)
        {
            // Идем циклом по тексту. 
            // Для каждого символа пытаемся применить цепочку обработчиков.
            // Если никто не обработал — просто добавляем символ в результат.
            throw new NotImplementedException();
        }
        
    }
}
