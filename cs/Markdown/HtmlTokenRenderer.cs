using System;
using System.Collections.Generic;
using Markdown.Interfaces;

namespace Markdown
{
    public class HtmlTokenRenderer : ITokenRenderer
    {
        public string Render(IEnumerable<TokenNode> tokens) => throw new NotImplementedException();
    }
}