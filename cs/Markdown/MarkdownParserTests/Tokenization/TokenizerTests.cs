using System;
using System.Collections.Generic;
using FluentAssertions;
using MarkdownParser.Concrete.Bold;
using MarkdownParser.Concrete.Italic;
using MarkdownParser.Infrastructure.Tokenization;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using NUnit.Framework;

namespace MarkdownParserTests.Tokenization
{
    [TestFixture]
    public class Tokenizer_TokenizeShould
    {
        private Tokenizer tokenizer;

        [SetUp]
        public void SetUp()
        {
            tokenizer = new Tokenizer(new ITokenBuilder[]
            {
                new ItalicTokenBuilder(),
                new BoldTokenBuilder(),
            });
        }

        [Test]
        public void WhenOnlyText_ReturnIt()
        {
            var tokens = new TokensCollectionBuilder().Text("some text here");
            tokenizer.Tokenize(tokens.ToString())
                .Should()
                .BeEquivalentTo(tokens);
        }

        [Test]
        public void WhenEmptyString_ReturnEmptyCollection()
        {
            tokenizer.Tokenize(string.Empty)
                .Should()
                .BeEmpty();
        }

        [TestCaseSource(nameof(ItalicCases))]
        public void ItalicTests(TokensCollectionBuilder tokens)
        {
            TestTokenize(tokens);
        }

        [TestCaseSource(nameof(BoldCases))]
        public void BoldTests(TokensCollectionBuilder tokens)
        {
            TestTokenize(tokens);
        }

        private static IEnumerable<TestCaseData> ItalicCases => new[]
        {
            TestCase("Should create Before word", t => t.Italic().Text("abc")),
            TestCase("Should create After word", t => t.Text("abc").Italic()),
            TestCase("Should create Inside word", t => t.Text("abc").Italic().Text("def")),
            TestCase("Should not create Inside digit", t => t.AsText(tt => tt.Text("123").Italic().Text("456"))),
            TestCase("Should not create Before digit", t => t.AsText(tt => tt.Italic().Text("123"))),
            TestCase("Should not create After digit", t => t.AsText(tt => tt.Text("123").Italic())),
            TestCase("Should not create when inside whitespaces", t => t.AsText(tt => tt.Text(" ").Italic().Text(" "))),
        };

        private static IEnumerable<TestCaseData> BoldCases => new[]
        {
            TestCase("Should create Before word", t => t.Bold().Text("abc")),
            TestCase("Should create After word", t => t.Text("abc").Bold()),
            TestCase("Should create Inside word", t => t.Text("abc").Bold().Text("def")),
            TestCase("Should not create Inside digit", t => t.AsText(tt => tt.Text("123").Bold().Text("456"))),
            TestCase("Should not create Before digit", t => t.AsText(tt => tt.Bold().Text("123"))),
            TestCase("Should not create After digit", t => t.AsText(tt => tt.Text("123").Bold())),
            TestCase("Should not create when inside whitespaces", t => t.AsText(tt => tt.Text(" ").Bold().Text(" "))),
            TestCase("Bold inside another bold", t => t.Bold().AsText(tt => tt.Bold().Text("123").Bold()).Bold()),
        };

        private void TestTokenize(TokensCollectionBuilder tokens)
        {
            tokenizer.Tokenize(tokens.ToString())
                .Should()
                .BeEquivalentTo(tokens);
        }

        private static TestCaseData TestCase(string name, Action<TokensCollectionBuilder> tokenBuilder)
        {
            var builder = new TokensCollectionBuilder();
            tokenBuilder.Invoke(builder);
            return new TestCaseData(arg: builder) {TestName = name};
        }
    }
}