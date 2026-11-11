namespace Markdown
{
    /// <summary>
    /// Публичный класс, проверяет наличие нумерованных списков
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

        public string RenderHeader(string line) => line.StartsWith("# ") ? $"<h1>{line.Skip(2)}</h1>" : line;

        public string RenderUnnumerableListItem(string line) => line.StartsWith("- ") ? $"<li>{line.Skip(2)}</li>" : line;
        public string RenderListStart(string line) => $"<ul> /r/n{line}";
        public string RenderListEnd(string line) => $"line/r/n</ul>/r/n";
        
    }
}
