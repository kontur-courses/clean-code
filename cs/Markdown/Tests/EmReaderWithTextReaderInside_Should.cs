using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using Markdown.Readers;
using Markdown.Tokens;

namespace Markdown.Tests
{
    [TestFixture]
    public class EmReaderWithTextReaderInside_Should : Reader_Should<PairedTagReader>
    {
        private PairedTagReader emReader;
        private ReadingOptions options;
        private string tagSymbols;
        private string tagName;

        [OneTimeSetUp]
        public void SetUpOnce()
        {
            tagName = "em";
            tagSymbols = "_";
            emReader = new PairedTagReader(tagName, tagSymbols);
            var textReader = new TextReader(tagSymbols[0].ToString());
            options = new ReadingOptions(
                new List<AbstractReader> { textReader },
                new Dictionary<AbstractReader, HashSet<AbstractReader>> {
                    [emReader] = new HashSet<AbstractReader> { textReader }
                }
            );
            defaultReader = emReader;
        }

        [Test]
        public void NotReadTag_WhenNothingInsideIt()
        {
            var allText = $"{tagSymbols}{tagSymbols}";
            var (token, _) = emReader.ReadToken(allText, 0, options);
            token.Should().BeNull();
        }

        [Test]
        public void ReadTag_WhenItsLocatedInTheStartOfTheText()
        {
            var innerText = "eeee";
            var allText = $"{tagSymbols}{innerText}{tagSymbols}aa";

            var (token, read) = emReader.ReadToken(allText, 0, options);

            read.Should().Be(6);
            token.Should().Be(new PairedTagToken("em", new List<IToken> { new TextToken(innerText) }));
        }

        [Test]
        public void ReadTag_WhenItsLocatedNotInTheStartOfTheText()
        {
            var innerText = "eeee";
            var allText = $"aa{tagSymbols}{innerText}{tagSymbols}aa";

            var (token, read) = emReader.ReadToken(allText, 2, options);

            read.Should().Be(6);
            token.Should().Be(new PairedTagToken("em", new List<IToken> { new TextToken(innerText) }));
        }

        [Test]
        public void ReadTag_WhenItsLocatedInTheEndOfTheText()
        {
            var innerText = "eeee";
            var allText = $"__{tagSymbols}{innerText}{tagSymbols}";

            var (token, read) = emReader.ReadToken(allText, 2, options);

            read.Should().Be(6);
            token.Should().Be(new PairedTagToken("em", new List<IToken> { new TextToken(innerText) }));
        }

        [Test]
        public void NotReadTag_WhenMdTextHasOnlyTwoClosingTags()
        {
            var allText = $"a{tagSymbols} a{tagSymbols}";
            var (token, _) = emReader.ReadToken(allText, 1, options);
            token.Should().BeNull();
        }

        [Test]
        public void NotReadTag_WhenMdTextHasOnlyTwoOpeningTags()
        {
            var allText = $"{tagSymbols}a {tagSymbols}a";
            var (token, _) = emReader.ReadToken(allText, 0, options);
            token.Should().BeNull();
        }

        [TestCase("{0}word{0}1", TestName = "when it is between letter and number")]
        [TestCase("{0}word1{0}1", TestName = "when it is between numbers")]
        public void NotReadClosingTag(string text)
        {
            var allText = string.Format(text, tagSymbols);
            var (token, _) = emReader.ReadToken(allText, 0, options);
            token.Should().BeNull();
        }

        [TestCase("aaa{0}1{0}", TestName = "when it is between letter and number")]
        [TestCase("aa1{0}1{0}", TestName = "when it is between numbers")]
        public void NotReadOpeningTag(string text)
        {
            var allText = string.Format(text, tagSymbols);
            var (token, _) = emReader.ReadToken(allText, 3, options);
            token.Should().BeNull();
        }
    }
}