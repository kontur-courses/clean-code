using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    static class Markdown
    {
        public static void Main()
        {
            var text = "";

            var tagsFounder = new TextSplitter();
            var splittedText = tagsFounder.GetSplittedText(text);

            var htmlFormatter = new HTMLFormatter();
            var stringedHtml = htmlFormatter.GetStringedHTML(splittedText);

            HTMLSaver.SaveToHTML(stringedHtml);
        }
    }
}
