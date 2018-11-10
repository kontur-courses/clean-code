using System;
using System.Collections.Generic;

namespace Markdown
{
    internal class TextParser
    {
        public TextParser(IEnumerable<LexerRule> lexerRules)
        {
            LexerRules = lexerRules;
        }

        public IEnumerable<LexerRule> LexerRules { get; }

        /// <summary>
        /// Тут будет какой-то конечный автомат по символам строки.
        /// Для каждого нового символа будем выбирать обработчик из LexerRules.
        /// Идея для парных разделителей в том, чтобы сделать нечто вроде алгоритма Дейкстры для ОПН -
        ///  - складывать в стек и "закрывать", как скобки.
        /// 
        /// </summary>
        public IEnumerable<Token> Parse(string text)
        {
            var delimiters = GetDelimiterPositions(text);
            return GetTokensFromDelimiters(delimiters, text);
        }

        private IEnumerable<Token> GetTokensFromDelimiters(IEnumerable<Delimiter> delimiters, string text)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Delimiter> GetDelimiterPositions(string text)
        {
            throw new NotImplementedException();
        }
        
        private class Delimiter
        {
            public Delimiter(bool isPaired, string value, int position, TokenType originatingToken)
            {
                IsPaired = isPaired;
                Value = value;
                Position = position;
                OriginatingToken = originatingToken;
            }

            public TokenType OriginatingToken { get; private set; }
            public int Position { get; private set; }
            public string Value { get; private set; }
            public bool IsPaired { get; private set; }

        }
    }
}
