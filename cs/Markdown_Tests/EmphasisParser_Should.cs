using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Markdown.Parsers;
using Markdown.Elements;
using Markdown.Analyzers;

namespace Markdown_Tests
{
    [TestFixture]
    public class EmphasisParser_Should
    {
        [Test]
        public void ReturnPlainRootElement_WhenNoInnerElements()
        {
            var markdown = "hello world";
            var expected = new MarkdownElement(
                RootElementType.Create(), markdown, 0, markdown.Length, new List<MarkdownElement>());

            var parser = new EmphasisParser(SyntaxAnalyzer.AnalyzeSyntax(markdown),
                0, RootElementType.Create());
            var actual = parser.Parse();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ReturnUnderscoredElement_WhenValidClosing()
        {
            var markdown = "_hello world_";
            var expected = new MarkdownElement(
                SingleUnderscoreElementType.Create(), markdown, 1, markdown.Length - 1, new List<MarkdownElement>());

            var parser = new EmphasisParser(SyntaxAnalyzer.AnalyzeSyntax(markdown), 
                1, SingleUnderscoreElementType.Create());
            var actual = parser.Parse();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ReturnDoubleUnderscoredElement_WhenValidClosing()
        {
            var markdown = "__hello world__";
            var expected = new MarkdownElement(
                DoubleUnderscoreElementType.Create(), markdown, 2, markdown.Length - 2, new List<MarkdownElement>());

            var parser = new EmphasisParser(SyntaxAnalyzer.AnalyzeSyntax(markdown), 
                2, DoubleUnderscoreElementType.Create());
            var actual = parser.Parse();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ReturnBrokenElement_WhenNoClosing()
        {
            var markdown = "_hello world";
            var expected = new MarkdownElement(
                BrokenElementType.Create(), markdown, 1, markdown.Length, new List<MarkdownElement>());

            var parser = new EmphasisParser(SyntaxAnalyzer.AnalyzeSyntax(markdown), 
                1, SingleUnderscoreElementType.Create());
            var actual = parser.Parse();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ReturnRootWithInnerElement_WhenContainsValidInnerElement()
        {
            var markdown = "hello _world_";
            var expected = new MarkdownElement(
                RootElementType.Create(), markdown, 0, markdown.Length, 
                new List<MarkdownElement>()
                {
                    new MarkdownElement(
                        SingleUnderscoreElementType.Create(), markdown, 7, markdown.Length - 1, new List<MarkdownElement>())
                });

            var parser = new EmphasisParser(SyntaxAnalyzer.AnalyzeSyntax(markdown), 
                0, RootElementType.Create());
            var actual = parser.Parse();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ReturnBrokenElement_WhenNotPairedIndicators()
        {
            var markdown = "_hello world__";
            var expected = new MarkdownElement(
                BrokenElementType.Create(), markdown, 1, markdown.Length - 2, new List<MarkdownElement>());

            var parser = new EmphasisParser(SyntaxAnalyzer.AnalyzeSyntax(markdown), 
                1, SingleUnderscoreElementType.Create());
            var actual = parser.Parse();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ReturnElementWithInner_WhenCanContainInner()
        {
            var markdown = "__hello _world_ !__";
            var expected = new MarkdownElement(
                DoubleUnderscoreElementType.Create(), markdown, 2, markdown.Length - 2,
                new List<MarkdownElement>()
                {
                    new MarkdownElement(
                        SingleUnderscoreElementType.Create(), markdown, 9, markdown.Length - 5, new List<MarkdownElement>())
                });

            var parser = new EmphasisParser(SyntaxAnalyzer.AnalyzeSyntax(markdown), 
                2, DoubleUnderscoreElementType.Create());
            var actual = parser.Parse();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ReturnInner_WhenElementCantContainInner()
        {
            var markdown = "_hello __world__ !_";
            var expected = new MarkdownElement(
                RootElementType.Create(), markdown, 0, markdown.Length,
                new List<MarkdownElement>()
                {
                    new MarkdownElement(
                        DoubleUnderscoreElementType.Create(), markdown, 9, 14, new List<MarkdownElement>())
                });
        
            var parser = new EmphasisParser(SyntaxAnalyzer.AnalyzeSyntax(markdown), 
                0, RootElementType.Create());
            var actual = parser.Parse();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void NotRecognizeMarkup_WhenTooManyUnderscores()
        {
            var markdown = "___hello___";
            var expected = new MarkdownElement(
                RootElementType.Create(), markdown, 0, markdown.Length, new List<MarkdownElement>());
            var parser = new EmphasisParser(SyntaxAnalyzer.AnalyzeSyntax(markdown), 
                0, RootElementType.Create());
            var actual = parser.Parse();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void NotRecognizeMarkup_WhenUnderscoreEscaped()
        {
            var markdown = @"\_hello\_";
            var expected = new MarkdownElement(
                RootElementType.Create(), markdown, 0, markdown.Length, new List<MarkdownElement>());
            var parser = new EmphasisParser(SyntaxAnalyzer.AnalyzeSyntax(markdown), 
                0, RootElementType.Create());
            var actual = parser.Parse();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void RecognizeSingleUnderscore_WhenOneUnderscoreEscaped()
        {
            var markdown = @"\__hello_";
            var expected = new MarkdownElement(
                RootElementType.Create(), markdown, 0, markdown.Length,
                new List<MarkdownElement>
                {
                    new MarkdownElement(
                        SingleUnderscoreElementType.Create(), markdown, 3, markdown.Length - 1,
                        new List<MarkdownElement>())
                });
            var parser = new EmphasisParser(SyntaxAnalyzer.AnalyzeSyntax(markdown), 
                0, RootElementType.Create());
            var actual = parser.Parse();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void RecognizeUnderscoresInsideWordWithoutDigits()
        {
            var markdown = "h_ell_o";
            var expected = new MarkdownElement(
                RootElementType.Create(), markdown, 0, markdown.Length, new List<MarkdownElement>
                {
                    new MarkdownElement(
                        SingleUnderscoreElementType.Create(), markdown, 2, 5, 
                        new List<MarkdownElement>())
                });

            var parser = new EmphasisParser(SyntaxAnalyzer.AnalyzeSyntax(markdown),
                0, RootElementType.Create());
            var actual = parser.Parse();
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void NotRecognizeUnderscoresInsideWordWithDigits()
        {
            var markdown = "_1_ __2__";
            var expected = new MarkdownElement(
                RootElementType.Create(), markdown, 0, markdown.Length, new List<MarkdownElement>());

            var parser = new EmphasisParser(SyntaxAnalyzer.AnalyzeSyntax(markdown),
                0, RootElementType.Create());
            var actual = parser.Parse();
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
