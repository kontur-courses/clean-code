using FluentAssertions;
using MarkDownConverter;
using MarkDownConverter.Enums;
using MarkDownConverter.Interfaces;
using MarkDownConverter.Nodes;
using MarkDownConverter.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverterTests
{
    [TestFixture]
    public class ConverterTests
    {
        MarkDownConverter.MarkDownConverter converter;
        MarkDownTokenizer tokenizer;
        MarkDownParser parser;

        [SetUp]
        public void Setup()
        {
            tokenizer = new MarkDownTokenizer();
            parser = new MarkDownParser();
            converter = new MarkDownConverter.MarkDownConverter(tokenizer, parser);
        }

        [TestCase("# header", "<h1>header</h1>")]
        [TestCase("_italic_", "<em>italic</em>")]
        [TestCase("ita_lic_", "ita<em>lic</em>")]
        [TestCase("__strong__", "<strong>strong</strong>")]
        [TestCase("st__rong__", "st<strong>rong</strong>")]
        [TestCase("___text___", "<strong><em>text</em></strong>")]
        [TestCase("__text _text_ text__", "<strong>text <em>text</em> text</strong>")]
        [TestCase("# header\n new line", "<h1>header</h1>\n new line")]
        [TestCase(@"\n\_Вот это\_", @"\n_Вот это_")]
        [TestCase("line with _italic_ text", "line with <em>italic</em> text")]
        [TestCase("a _t_ b", "a <em>t</em> b")]
        [TestCase("line with __strong__ text", "line with <strong>strong</strong> text")]
        [TestCase("line with __text _text_ text__ abc", "line with <strong>text <em>text</em> text</strong> abc")]
        [TestCase("# Header 1\n ___Dear Diary___, today has been a _hard_ day",
        "<h1>Header 1</h1>\n <strong><em>Dear Diary</em></strong>, today has been a <em>hard</em> day")]
        [TestCase("# _Header_ 1\n ___Dear Diary___, today has been a _hard_ day",
        "<h1><em>Header</em> 1</h1>\n <strong><em>Dear Diary</em></strong>, today has been a <em>hard</em> day")]
        public void MarkDownConverter_ReturnsExpectedHtml(string md, string expected)
        {
            var actual = converter.Convert(md);
            actual.Should().Be(expected);
        }


        [Test]
        public void MarkDownConverter_WorkLinear()
        {
            const int size1 = 1000;
            const int size2 = 100000;

            var size1Input = GenerateMarkdownInput(size1);
            var size2Input = GenerateMarkdownInput(size2);

            var size1ITime = MeasureTime(() => converter.Convert(size1Input));
            var size2Time = MeasureTime(() => converter.Convert(size2Input));

            var relation = (double)size2Time / size1ITime;
            relation.Should().BeLessThan(size2 / size1 * 1.5);
        }

        private string GenerateMarkdownInput(int size)
        {
            var markDownBuilder = new StringBuilder(size);
            for (var i = 0; i < size; i++)
            {
                markDownBuilder.Append("_cursive_ __fat__ \n# head");
            }

            return markDownBuilder.ToString();
        }

        private long MeasureTime(Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        [Test]
        public void Tokenizer_Correct_WithItalic()
        {
            string text = "_onetwothree_";
            var correctTokens = new IToken[] { 
                new ItalicToken(0),
                new SymbolsToken(1, "onetwothree"), 
                new ItalicToken(12) 
                };
            var tokens = tokenizer.Tokenize(text);
            tokens.Should().BeEquivalentTo(correctTokens);
        }

        [Test]
        public void Tokenizer_Correct_WithBold()
        {
            string text = "__onetwothree__";
            var correctTokens = new IToken[] { 
                new BoldToken(0), 
                new SymbolsToken(2, "onetwothree"), 
                new BoldToken(13) 
                };
            var tokens = tokenizer.Tokenize(text);
            tokens.Should().BeEquivalentTo(correctTokens);
        }

        [Test]
        public void Tokenizer_Correct_WithHeading_NoCloseToken()
        {
            string text = "# head";
            var correctTokens = new IToken[] {
                new HeadingToken(0),
                new SymbolsToken(2, "head") 
                };
            var tokens = tokenizer.Tokenize(text);
            tokens.Should().BeEquivalentTo(correctTokens);
        }

        [Test]
        public void Tokenizer_Correct_WithHeading_AndCloseToken()
        {
            string text = "# head\ntext";
            var correctTokens = new IToken[] {
                new HeadingToken(0),
                new SymbolsToken(2, "head"),
                new NewLineToken(6),
                new SymbolsToken(7, "text")
                };
            var tokens = tokenizer.Tokenize(text);
            tokens.Should().BeEquivalentTo(correctTokens);
        }

        [Test]
        public void Tokenizer_WorksCorrect_WithItalicInBold()
        {
            const string text = "__text _cursive___";
            var correctTokens = new IToken[] {
                new BoldToken(0),
                new SymbolsToken(2, "text"),
                new SpaceToken(6),
                new ItalicToken(7),
                new SymbolsToken(8, "cursive"),
                new ItalicToken(15),
                new BoldToken(16)
                };
            var tokens = tokenizer.Tokenize(text);
            tokens.Should().BeEquivalentTo(correctTokens);
        }

        [Test]
        public void Tokenizer_Correct_AllTokens()
        {
            const string text = "\n# head __text _italic___ 1_23";
            var expected = new IToken[] {
                new NewLineToken(0),
                new HeadingToken(1),
                new SymbolsToken(3, "head"),
                new SpaceToken(7),
                new BoldToken(8),
                new SymbolsToken(10, "text"),
                new SpaceToken(14),
                new ItalicToken(15),
                new SymbolsToken(16, "italic"),
                new ItalicToken(22),
                new BoldToken(23),
                new SpaceToken(25),
                new NumbersToken(26, "1_23")
            };
            var actual = tokenizer.Tokenize(text);
            actual.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Test]
        public void Parse_ReturnsNodes_WithAllTags()
        {
            const string md = "# a b\\_c _d_ __e__\n___f___ 1_234";
            var tokens = tokenizer.Tokenize(md);
            var actual = parser.Parse(tokens);

            var space = new SymbolsNode(" ");
            var newLine = new SymbolsNode("\n");

            var heading = new HeadingNode();
            heading.Children.Add(new SymbolsNode("a"));
            heading.Children.Add(space);
            heading.Children.Add(new SymbolsNode("b"));
            heading.Children.Add(new SymbolsNode("_"));
            heading.Children.Add(new SymbolsNode("c"));
            heading.Children.Add(space);

            var italicD = new ItalicNode();
            italicD.Children.Add(new SymbolsNode("d"));

            heading.Children.Add(italicD);
            heading.Children.Add(space);

            var boldE = new BoldNode();
            boldE.Children.Add(new SymbolsNode("e"));

            heading.Children.Add(boldE);

            var boldF = new BoldNode();
            var italicF = new ItalicNode();
            italicF.Children.Add(new SymbolsNode("f"));
            boldF.Children.Add(italicF);

            var text1_234 = new SymbolsNode("1_234");

            var expected = new ParentNode();
            expected.Children.Add(heading);
            expected.Children.Add(newLine);
            expected.Children.Add(boldF);
            expected.Children.Add(space);
            expected.Children.Add(text1_234);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
