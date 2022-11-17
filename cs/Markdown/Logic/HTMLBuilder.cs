using System;
using System.Collections.Generic;
using Markdown.DataStructures;

namespace Markdown.Logic
{
    public class HTMLBuilder : IHTMLBuilder
    {
        public string Build(string text, IEnumerable<Token> tokens, List<int> escapeSymbolsIndexes)
        {
            throw new NotImplementedException();
        }
    }
}