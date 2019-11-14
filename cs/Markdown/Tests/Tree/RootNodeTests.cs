using FluentAssertions;
using Markdown.Tree;
using NUnit.Framework;

namespace Markdown.Tests.Tree
{
    [TestFixture]
    public class RootNodeTests
    {
        [Test]
        public void GetText_ShouldReturnJoinedTextsOfAllChildren()
        {
            var root = new RootNode();
            var bold = new BoldNode();
            var italic = new ItalicNode();
            var plain = new PlainTextNode("Text");
            bold.AddNode(new PlainTextNode("Bold"));
            italic.AddNode(new PlainTextNode("Italic"));
            root.AddNode(plain);
            root.AddNode(bold);
            root.AddNode(italic);

            var actual = root.GetText();

            actual.Should().BeEquivalentTo($"{plain.GetText()}{bold.GetText()}{italic.GetText()}");
        }
    }
}