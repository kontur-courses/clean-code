using System;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdown)
        {
            var converter = new TokenConverter();
            return converter
                .Initialize(markdown)
                .FindTokens()
                .Build();
        }
    }
}