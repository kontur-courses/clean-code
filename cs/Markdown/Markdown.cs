using System;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdown)
        {
            var parser = new TextParser();
            var parsedText = parser.ParseText(markdown);

            return parsedText;
        }
    }
}