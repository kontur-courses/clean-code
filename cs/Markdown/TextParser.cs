using System;
using System.Collections.Generic;

namespace Markdown
{
    internal class TextParser
    {
        public TextParser(IEnumerable<DelimiterRule> delimitersRules)
        {
            DelimitersRules = delimitersRules;
        }

        public IEnumerable<DelimiterRule> DelimitersRules { get; }

        /// <summary>
        /// Тут будет какой-то конечный автомат по символам строки.
        /// Для каждого нового символа будем выбирать обработчик из DelimetersRules.
        /// </summary>
        public IEnumerable<Token> Parse(string text) => throw new NotImplementedException();
    }
}