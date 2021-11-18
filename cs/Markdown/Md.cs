using System;
using Markdown.Token;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdown)
        {
            var converter = new TokenConverter();
            var tokens = converter.Convert(markdown);
            return converter.Convert(tokens);
        }
    }
}