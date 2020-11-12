using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class HTMLConverter
    {
        public string GetHTMLString(List<TextToken> splittedText)
        {
            //превращает разделенный текст в построчный html файл
            //TODO оставил работу с этим классом на будущее, потому что написать его легче, чем Parser
            var stringedHtml = new StringBuilder();

            return stringedHtml.ToString();
        }
    }
}