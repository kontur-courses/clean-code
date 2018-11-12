using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    internal class TextParser
    {
        private readonly List<ILexerRule> rules;

        public TextParser(IEnumerable<ILexerRule> rules)
        {
            this.rules = rules.ToList();
        }

        public void AddRule(ILexerRule rule) => rules.Add(rule);

        /// <summary>
        ///     Тут будет какой-то конечный автомат по символам строки.
        ///     Для каждого нового символа будем выбирать обработчик из LexerRules.
        ///     Идея для парных разделителей в том, чтобы сделать нечто вроде алгоритма Дейкстры для ОПН -
        ///     - складывать в стек и "закрывать", как скобки.
        /// </summary>
        public IEnumerable<Token> Parse(string text)
        {
            var delimiters = GetDelimiterPositions(text);
            delimiters = RemoveEscapedDelimiters(delimiters, text);
            delimiters = RemoveNonValidDelimiters(delimiters, text);
            delimiters = RemoveNonPairedDelimiters(delimiters, text);

            return GetTokensFromDelimiters(delimiters, text);
        }

        internal IEnumerable<Delimiter> RemoveNonPairedDelimiters(IEnumerable<Delimiter> delimiters, string text) =>
            delimiters;

        internal IEnumerable<Delimiter> RemoveNonValidDelimiters(IEnumerable<Delimiter> delimiters, string text) =>
            delimiters;

        internal IEnumerable<Delimiter> RemoveEscapedDelimiters(IEnumerable<Delimiter> delimiters, string text) =>
            delimiters;

        internal IEnumerable<Token> GetTokensFromDelimiters(IEnumerable<Delimiter> delimiters, string text) =>
            new List<Token>();

        internal IEnumerable<Delimiter> GetDelimiterPositions(string text) => new List<Delimiter>();

        internal ILexerRule GetRuleForSymbol(char symbol)
        {
            return rules.FirstOrDefault(rule => rule.Check(symbol));
        }
    }

    [TestFixture]
    public class TextParser_Tests
    {
        [SetUp]
        public void SetUp()
        {
            parser = new TextParser(null);
        }

        private TextParser parser;

        [Category("GetDelimiterPositions_Should")]
        [TestCase]
        public void ReturnEmptyList_WhenNoDelimiters()
        {
            parser.GetDelimiterPositions("abcd efg")
                  .Should()
                  .BeEmpty();
        }

        [Category("GetDelimiterPositions_Should")]
        [TestCase]
        public void ReturnOneDelimiterOfGivenRule_WhenOneExistsOfThisRule()
        {
            parser.GetDelimiterPositions("abcd_efg")
                  .Should()
                  .BeEmpty();
        }
    }
}
