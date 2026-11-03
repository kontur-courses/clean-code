using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    /// <summary>
    /// Класс разделяет текст на строки и делегирует парсинг строк InlineParser
    /// </summary>
    internal class MarkdownParser
    {
        private readonly IInlineParser inlineParser;

        public MarkdownParser(IInlineParser inlineParser)
        {
            this.inlineParser = inlineParser;
        }

        public string Parse(string markdownText)
        {
            throw new NotImplementedException();
            //проверки на валидность
            //var lines = markdownText.Split(...);
            //var result = new StringBuilder()

            //foreach line in lines { ...inlineParser.Paese(line)...}
            
            //return result.ToString();
        }
    }
}
