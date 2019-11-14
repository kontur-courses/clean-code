using FluentAssertions;
using Markdown.Tree;
using NUnit.Framework;

namespace Markdown.Tests.Tree
{
    [TestFixture]
    public class PlainTextNodeTests
    {
        [Test]
        public void GetText_ShouldReturnTextItself()
        {
            var text = "This is text sample";
            var node = new PlainTextNode(text);

            var actual = node.GetText();

            actual.Should().BeEquivalentTo(text);
        }
    }
}