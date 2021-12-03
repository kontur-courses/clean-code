using System;
using System.Collections.Generic;
using Markdown.Parser;
using Markdown.Renderer;
using Markdown.Tokens;

namespace Markdown
{
    public class Md
    {
        private readonly IParser<MarkdownToken> parser;
        private readonly IRenderer<MarkdownToken> renderer;

        public Md(IParser<MarkdownToken> parser, IRenderer<MarkdownToken> renderer)
        {
            this.parser = parser;
            this.renderer = renderer;
        }


        public string Render(string text)
        {
            var tokens = parser.Parse(text);
            return renderer.Render(tokens);
        }
    }
}