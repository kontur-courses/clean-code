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

        [Test]
        public void EmptyItalic_ShouldBeTreatAsSingleBold()
        {
            tokenizer.Tokenize("__")
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeEquivalentTo(new BoldToken(0, "__"),
                    "Потому что это символ жирного текста, это нормально");
        }

        [TestCaseSource(nameof(BoldCases))]
        public void BoldTests(TokensCollectionBuilder tokens)
        {
            TestTokenize(tokens);
        }

        [TestCaseSource(nameof(BoldItalicInteractionCases))]
        public void BoldItalicInteractionTests(TokensCollectionBuilder tokens)
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
            TestCase("Empty should be text", t => t.AsText(tt => tt.Bold().Bold())),
        };

        private static IEnumerable<TestCaseData> BoldItalicInteractionCases => new[]
        {
            TestCase("When Italic inside Bold wrap both", t => t
                .Bold()
                .Text("bold ")
                .Italic().Text("italic").Italic()
                .Text(" bold")
                .Bold()),
            TestCase("When Bold inside Italic treat ignore Bold", t => t
                .Italic()
                .Text("italic ")
                .AsText(tt => tt.Bold().Text("bold").Bold())
                .Text(" italic")
                .Italic()),
            TestCase("Ignore unpaired Bold and Italic", t => t
                .AsText(tt => tt
                    .Italic().Text("italic ")
                    .Bold().Text("bold"))),
            TestCase("Ignore crossing Bold and Italic", t => t
                .AsText(tt => tt
                    .Bold().Text("abc ")
                    .Italic().Text("def ")
                    .Bold().Text("ghi ")
                    .Italic().Text("jkl ")))
        };

        private void TestTokenize(TokensCollectionBuilder tokens)
        {
            TestContext.Out.WriteLine($"Parsing {tokens}");
            tokenizer.Tokenize(tokens.ToString())
                .Should()
                .BeEquivalentTo(tokens);
            foreach (var token in tokens.ToArray())
                TestContext.Out.WriteLine(token);
        }

        private static TestCaseData TestCase(string name, Action<TokensCollectionBuilder> tokenBuilder)
        {
            var builder = new TokensCollectionBuilder();
            tokenBuilder.Invoke(builder);
            return new TestCaseData(arg: builder) {TestName = name};
        }
    }
}