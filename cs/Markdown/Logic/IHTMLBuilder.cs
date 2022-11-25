using System.Collections.Generic;
using Markdown.DataStructures;

namespace Markdown.Logic
{
    public interface IHTMLBuilder
    {
        /// <summary>
        /// Из последовательности токенов собирает строку с html
        /// </summary>
        string Build(string text, TokensTree tree, List<int> escapeSymbolsIndexes);
    }
}