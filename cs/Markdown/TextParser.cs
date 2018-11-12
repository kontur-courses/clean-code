using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class TextParser
    {
        private readonly List<ILexerRule> rules;

        public TextParser(IEnumerable<ILexerRule> rules)
        {
            this.rules = rules?.ToList() ?? new List<ILexerRule>();
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

        internal IEnumerable<Delimiter> GetDelimiterPositions(string text)
        {
            var delimiters = new List<Delimiter>();
            foreach (var (symbol, position) in text.Select((symbol, i)=>(symbol, i)))
            {
                var rule = GetRuleForSymbol(symbol);
                if (rule == null)
                    continue;
                var delimiter = rule.ProcessIncomingChar(position, delimiters.LastOrDefault(), out var shouldRemovePrevious);
                if (shouldRemovePrevious)
                    delimiters.RemoveAt(delimiters.Count-1);
                delimiters.Add(delimiter);
            }

            return delimiters;
        }

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
        [Test]
        public void ReturnEmptyList_WhenNoDelimiters()
        {
            parser.GetDelimiterPositions("abcd efg")
                  .Should()
                  .BeEmpty();
        }

        [Category("GetDelimiterPositions_Should")]
        [Test]
        public void ReturnOneDelimiterOfUnderscoreRule_WhenOneExistsOfThisRule()
        {
            parser.AddRule(new UnderscoreRule());
            parser.GetDelimiterPositions("abcd_efg")
                  .Should().HaveCount(1).And.Subject.First().ShouldBeEquivalentTo(new Delimiter(true, "_", 4));

        }
        [Category("GetDelimiterPositions_Should")]
        [Test]
        public void ReturnOneDelimiterOfDoubleUnderscoreRule_WhenOneExistsOfThisRule()
        {
            parser.AddRule(new UnderscoreRule());
            parser.GetDelimiterPositions("abcd__efg")
                  .Should().HaveCount(1).And.Subject.First().ShouldBeEquivalentTo(new Delimiter(true, "__", 4));

        }
        [Category("GetDelimiterPositions_Should")]
        [Test]
        public void ReturnOneDelimiterOfDoubleUnderscoreRuleAndOneOfUnderscore_WhenThereAre3Underscores()
        {
            parser.AddRule(new UnderscoreRule());
            parser.GetDelimiterPositions("abcd___efg")
                  .Should().HaveCount(2).And.Subject.ShouldBeEquivalentTo(new[] { new Delimiter(true, "__", 4), new Delimiter(true, "_", 6) });

        }
    }
}
