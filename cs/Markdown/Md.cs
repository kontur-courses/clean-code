using System;
using Markdown.Logic;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var parser = new TextParser();
            var tokenTree = parser.Parse(text);
            return new HTMLBuilder().Build(text, tokenTree, parser.IndexesEscapeCharacters);
        }
    }
}