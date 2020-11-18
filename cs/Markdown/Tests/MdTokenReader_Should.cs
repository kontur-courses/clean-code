using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MdTokenReader_Should
    {
        [Test]
        public void ReadHeader()
        {
            var text = "# hello, world!";
            var reader = new MdTokenReader("# hello, world!");
            var tokens = reader.ReadAll();
            var header = tokens.Should().ContainSingle(t => t is MdHeaderToken).Subject.As<MdHeaderToken>();
            header.StartPosition.Should().Be(0);
            header.Length.Should().Be(text.Length);
            var rawText = header.EnumerateSubtokens().Should().ContainSingle(t => t is MdRawTextToken)
                .Subject.As<MdRawTextToken>();
            rawText.StartPosition.Should().Be(header.StartPosition + 2);
            rawText.Length.Should().Be(header.Length - 2);
        }
    }
}