namespace Markdown
{
    /// <summary>
    /// Публичный класс, который будет работать с другими приватными классами, скрывая реализацию и предоставляя интерфейс
    /// </summary>
    public class Md
    {
        private readonly IInlineParser parser;
        private readonly IHtmlRenderer renderer;

        public Md (IInlineParser inlineParser, IHtmlRenderer HtmlRenderer)
        {
            parser = inlineParser;
            renderer = HtmlRenderer;
        }

        public string Render(string  markdownText)
        {

            //разбиваем на строки
            //отдаем строки в parser -> токены
            //токены отдаем в renderer -> строки
            //строки собираем в html текст
            return htmlText;
        }
    }
}
