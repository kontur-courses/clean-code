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
        public void returnNullShell_WhenNotFormatting()
        {
            var text = "_abc_text";
            var position = 5;
            var shell = tokenizer.GetNextShell(text, ref position, shells);
            position.Should().Be(position);
            shell.Should().BeNull();
        }

        [Test]
        public void findShell_WhenPositionInSingleUnderline()
        {
            var text = "_italic text_";
            var position = 0;
            var shell = tokenizer.GetNextShell(text, ref position, shells);
            position.Should().Be(1);
            shell.Should().BeOfType(typeof(SingleUnderline));
        }
    }
}
