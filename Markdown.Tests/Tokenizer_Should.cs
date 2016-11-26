using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown.Shell;
using Markdown.Tokenizer;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Tokenizer_Should
    {
        private readonly List<IShell> shells = new List<IShell>()
        {
            new SingleUnderline(),
            new DoubleUnderline()
        };

        [Test]
        public void notFindShell_WhenNotFormatting()
        {
            var text = "text";
            var tokenizer = new StringTokenizer(text, shells);
            
            var token = tokenizer.NextToken();
            token.Shell.Should().BeNull();
        }

        [Test]
        public void findShell_WhenPositionInSingleUnderline()
        {
            var text = "_italic text_";
            var tokenizer = new StringTokenizer(text, shells);
            var token = tokenizer.NextToken();
            token.Shell.Should().BeOfType(typeof(SingleUnderline));
        }

        [Test]
        public void findShell_WhenPositionInDoubleUnderline()
        {
            var text = "__bold text__";
            var tokenizer = new StringTokenizer(text, shells);
            var token = tokenizer.NextToken();
            token.Shell.Should().BeOfType(typeof(DoubleUnderline));
        }

        [Test]
        public void notFindShell_WhenSpaceAfterPrefix()
        {
            var text = "_ def_";
            var tokenizer = new StringTokenizer(text, shells);
            var token = tokenizer.NextToken();
            token.Shell.Should().BeNull();
            token.Text.Should().Be("_ def");
        }

        [Test]
        public void readToken_WhenNotFormatting()
        {
            var text = "not formatted text_def_";
            var tokenizer = new StringTokenizer(text, shells);
            var token = tokenizer.NextToken();
            token.Shell.Should().BeNull();
            token.Text.Should().Be("not formatted text");
        }

        [Test]
        public void readToken_WhenNextSingleUnderline()
        {
            var text = "_italic text_123";
            var tokenizer = new StringTokenizer(text, shells);
            var token = tokenizer.NextToken();
            token.Shell.Should().BeOfType(typeof(SingleUnderline));
            token.Text.Should().Be("italic text");
        }
        [Test]
        public void readToken_WhenNextDoubleUnderline()
        {
            var text = "__bold text__88";
            var tokenizer = new StringTokenizer(text, shells);
            var token = tokenizer.NextToken();
            token.Shell.Should().BeOfType(typeof(DoubleUnderline));
            token.Text.Should().Be("bold text");
        }

        [Test]
        public void notFindEndToken_WhenSpaceBeforeSuffix()
        {
            var text = "_some _text_";
            var tokenizer = new StringTokenizer(text, shells);
            var token = tokenizer.NextToken();
            token.Shell.Should().BeOfType(typeof(SingleUnderline));
        }

        [Test]
        public void notFindShell_WhenBeforePrefixShielding()
        {
            var text = "\\_italic_";
            var tokenizer = new StringTokenizer(text, shells);
            while (tokenizer.HasMoreTokens())
            {
                tokenizer.NextToken().Shell.Should().BeNull();
            }

        }

        [Test]
        public void notFindShell_WhenPrefixSurroundedByNumbers()
        {
            var text = "12_2text_";
            var tokenizer = new StringTokenizer(text, shells);
            tokenizer.NextToken().Text.Should().Be("12_2text");
        }

        [Test]
        public void throwException_WhenIsImpossibleGetToken()
        {
            var text = "token";
            var tokenizer = new StringTokenizer(text, shells);
            tokenizer.NextToken();
            Assert.Throws<InvalidOperationException>(() => tokenizer.NextToken());
        }
    }
}
