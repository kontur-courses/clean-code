using System;

namespace Markdown
{
    public class Md
    {
        private MarkdownParser mdParser;
        private HtmlMaker htmlMaker;
        
        public string Render(string rawText)
        {
            // Сперва разберем с помощью парсера, потом соберем с помощью сборщика,
            // больше в этом классе ничего быть не должно по идее
            throw new NotImplementedException();
        }
    }
}