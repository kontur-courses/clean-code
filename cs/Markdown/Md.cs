using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdownSource)
        {
            var tokens = new List<Token>();
            return Parser.ParseToHtml(tokens);
        }
    }
}
