using System;
using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.TokenRenderer
{
    public class HtmlTokenRenderer : ITokenRenderer
    {
        public string Render(IEnumerable<TokenNode> tokens) => throw new NotImplementedException();
    }
}