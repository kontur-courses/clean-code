using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Tokenizer_Should
    {
        private Tokenizer tokenizer;
        private List<IShell> shells;
        [SetUp]
        public void SetUp()
        {
            shells = new List<IShell>()
            {
                new SingleUnderline(),
                new DoubleUnderline()
            };
            tokenizer = new Tokenizer();
        }
        
        [Test]
        public void notChangeText_WhenNotFormatting()
        {
            var tokens = tokenizer.SplitToTokens("some text without shell", shells).ToList();
            tokens.Count.Should().Be(1);
            tokens.First().HasShell().Should().BeFalse();
        }

        [Test]
        public void notFindShell_WhenNotFormatting()
        {
            var text = "_abc_text";
            var position = 5;
            var shell = tokenizer.ReadNextShell(text, ref position, shells);
            position.Should().Be(5);
            shell.Should().BeNull();
        }

        [Test]
        public void findShell_WhenPositionInSingleUnderline()
        {
            var text = "_italic text_";
            var position = 0;
            var shell = tokenizer.ReadNextShell(text, ref position, shells);
            position.Should().Be(1);
            shell.Should().BeOfType(typeof(SingleUnderline));
        }

        [Test]
        public void findShell_WhenPositionInDoubleUnderline()
        {
            var text = "some text__bold text__";
            var position = 9;
            var shell = tokenizer.ReadNextShell(text, ref position, shells);
            position.Should().Be(11);
            shell.Should().BeOfType(typeof(DoubleUnderline));
        }

        [Test]
        public void notFindShell_WhenPositionIsOutside()
        {
            var text = "abcd efgh";
            var position = 10;
            tokenizer.ReadNextShell(text, ref position, shells).Should().BeNull();
        }


        [Test]
        public void notFindShell_WhenSpaceAfterPrefix()
        {
            var text = "abc _ def_";
            var position = 4;
            tokenizer.ReadNextShell(text, ref position, shells).Should().BeNull();
            position.Should().Be(4);
        }


        [Test]
        public void findEndToken_WhenNotFormatting()
        {
            var text = "_abc_ not formatted text_abc_";
            var position = 5;
            IShell currentShell = null;
            var endToken = tokenizer.GetEndPositionToken(text, position, shells, currentShell);
            endToken.Should().Be(23);
        }

        [Test]
        public void findEndToken_WhenOpenSingleUnderline()
        {
            var text = "asdf_qwerty_";
            var position = 5;
            IShell currentshell = new SingleUnderline();
            var endToken = tokenizer.GetEndPositionToken(text, position, shells, currentshell);
            endToken.Should().Be(10);
        }

        [Test]
        public void findEndToken_WhenOpenDoubleUnderline()
        {
            var text = "__qwerty__32";
            var position = 2;
            IShell currentshell = new DoubleUnderline();
            var endToken = tokenizer.GetEndPositionToken(text, position, shells, currentshell);
            endToken.Should().Be(7);
        }

        [Test]
        public void readToken_WhenNotFormatting()
        {
            var text = "_abc_not formatted text_def_";
            var position = 5;
            Token token = tokenizer.ReadNextToken(text, ref position, shells);
            token.Shell.Should().BeNull();
            token.Text.Should().Be("not formatted text");
            position.Should().Be(23);
        }

        [Test]
        public void readToken_WhenNextSingleUnderline()
        {
            var text = "abc_italic text_";
            var position = 3;
            var token = tokenizer.ReadNextToken(text, ref position, shells);
            token.Shell.Should().BeOfType(typeof(SingleUnderline));
            token.Text.Should().Be("italic text");
            position.Should().Be(16);
        }
        [Test]
        public void readToken_WhenNextDoubleUnderline()
        {
            var text = "qwerty__bold text__88";
            var position = 6;
            var token = tokenizer.ReadNextToken(text, ref position, shells);
            token.Shell.Should().BeOfType(typeof(DoubleUnderline));
            token.Text.Should().Be("bold text");
            position.Should().Be(19);
        }

        [Test]
        public void notFindEndToken_WhenSpaceBeforeSuffix()
        {
            var text = "_some _text_";
            var position = 1;
            var end = tokenizer.GetEndPositionToken(text, position, shells, new SingleUnderline());
            end.Should().Be(10);
        }

        [Test]
        public void notFindShell_WhenBeforePrefixShielding()
        {
            var text = "abc\\__italic_";
            var position = 4;
            var shell = tokenizer.ReadNextShell(text, ref position, shells);
            shell.Should().BeNull();
            position.Should().Be(4);
        }

        [Test]
        public void readOneChar_WhenUnpairedTag()
        {
            var text = "__ text__";
            var position = 0;
            var token = tokenizer.ReadNextToken(text, ref position, shells);
            token.Shell.Should().BeNull();
            token.Text.Should().Be("_");
            position.Should().Be(1);
        }
    }
}
