using FluentAssertions;
using Markdown.Tree;
using NUnit.Framework;

namespace Markdown.Tests.Tree
{
    [TestFixture]
    public class BoldNodeTests
    {
        private static readonly string Text1 = "Text1";
        private static readonly string Text2 = "Text2";

        [Test]
        public void GetText_WithoutItalicParent_ShouldReturnJoinedTextOfAllChildrenInStrongTag()
        {
            var plainText1 = new PlainTextNode(Text1);
            var plainText2 = new PlainTextNode(Text2);
            var bold = new BoldNode();
            bold.AddNode(plainText1);
            bold.AddNode(plainText2);

            var actual = bold.GetText();

            actual.Should().BeEquivalentTo($"<strong>{Text1}{Text2}</strong>");
        }

        [Test]
        public void GetText_WithItalicParent_ShouldReturnJoinedTextOfAllChildren()
        {
            var italic = new ItalicNode();
            var plainText1 = new PlainTextNode(Text1);
            var plainText2 = new PlainTextNode(Text2);
            var bold = new BoldNode();
            bold.AddNode(plainText1);
            bold.AddNode(plainText2);
            italic.AddNode(bold);

            var actual = bold.GetText();

            actual.Should().BeEquivalentTo(Text1 + Text2);
        }

        [Test]
        public void GetText_WithItalicChild_ShouldReturnRightTextWithTags()
        {
            var bold = new BoldNode();
            var italic = new ItalicNode();
            bold.AddNode(italic);
            bold.AddNode(new PlainTextNode(Text1));
            italic.AddNode(new PlainTextNode(Text2));

            var actual = bold.GetText();

            actual.Should().BeEquivalentTo($"<strong><em>{Text2}</em>{Text1}</strong>");
        }
    }
}