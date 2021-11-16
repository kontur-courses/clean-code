using System;
using System.Collections.Generic;
using Markdown.Interfaces;

namespace Markdown
{
    public class HtmlTokenRenderer : ITokenRenderer
    {
        public string Render(IEnumerable<Token> tokens) => throw new NotImplementedException();
    }
}