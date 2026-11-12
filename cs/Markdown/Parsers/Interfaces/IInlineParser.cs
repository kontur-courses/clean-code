using Markdown.Models.Interfaces;
using System;
using System.Collections.Generic;


namespace Markdown
{
    public interface IInlineParser
    {
        public List<IToken> ParseToTokens(string line);
    }
}
