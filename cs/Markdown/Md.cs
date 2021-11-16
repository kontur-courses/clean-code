using System;
using Markdown.Interfaces;

namespace Markdown
{
    public class Md
    {
        private readonly ILexer lexer;
        private readonly ITokenParser parser;
        private readonly ITokenRenderer renderer;

        public Md(ILexer lexer, ITokenParser parser, ITokenRenderer renderer)
        {
            this.lexer = lexer;
            this.parser = parser;
            this.renderer = renderer;
        }

        public string Render(string mdText) => throw new NotImplementedException();
    }
}