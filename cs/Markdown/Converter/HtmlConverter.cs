namespace Markdown.Converter
{
    public class HtmlConverter : IHtmlConverter
    {
        private Dictionary<TagType, string> openingTags = new()
        {
            { TagType.Bold, "<strong>" },
            { TagType.Italic, "<em>" },
            { TagType.Header, "<h1>" },
            { TagType.EscapedSymbol, "" },
        };

        private Dictionary<TagType, string> closingTags = new()
        {
            { TagType.Bold, "</strong>" }, 
            { TagType.Italic, "</em>" },
            { TagType.Header, "</h1>" }, 
            { TagType.EscapedSymbol, "" },
        };
        public string ConvertFromMarkdownToHtml(List<Token> tokens)
        {
            //Тут я думаю проходиться по списку всех токенов и заменять md теги на html теги
            //Возможно понадобиться добавлять новые методы
            throw new NotImplementedException();
        }
    }
}
