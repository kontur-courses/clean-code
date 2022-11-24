using FluentAssertions;
using Markdown.Markdown;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class ShieldShould
    {
        [Test]
        public void NotShieldTag_WhenShieldIsShielded()
        {
            var markdownString = "some \\\\_text_";
            var result = Md.Render(markdownString);
            result.Should().Be("some \\\\<em>text\\</em>");
        }

        [Test]
        public void NotShield_WhenNothingToShield()
        {
            var markdownString = "some \\text";
            var result = Md.Render(markdownString);
            result.Should().Be("some \\text");
        }
    }
}
