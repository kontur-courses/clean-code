using Markdown.Models.Interfaces;
using System;
using System.Collections.Generic;


namespace Markdown
{
    public interface IHtmlRenderer
    {
        public string Render(List<IToken> tokens);
    }
}
