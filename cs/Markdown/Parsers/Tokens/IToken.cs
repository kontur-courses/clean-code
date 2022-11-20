using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsers.Tokens
{
    public interface IToken
    {
        public IToken ToText();
        public IToken ToHtml();
        public IToken ToMarkdown();
    }
}
