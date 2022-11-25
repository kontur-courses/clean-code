using System.Collections.Generic;
using Markdown.DataStructures;

namespace Markdown.Logic
{
    public interface ITextParser
    {
        /// <summary>
        /// Возвращает список токенов, построенных согласно спецификации языка Markdown
        /// </summary>
        ///
        TokensTree Parse(string text);
    }
}