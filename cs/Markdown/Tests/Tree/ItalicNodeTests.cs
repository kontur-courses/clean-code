using FluentAssertions;
using Markdown.Tree;
using NUnit.Framework;

namespace Markdown.Tests.Tree
{
    [TestFixture]
    public class ItalicNodeTests
    {
        public static readonly string Text1 = "text1";
        public static readonly string Text2 = "text2";

        [Test]
        public void GetText_WithOneChild_ShouldReturnTextOfChildInEmTag()
        {
            var italic = new ItalicNode();
            italic.AddNode(new PlainTextNode(Text1));

            var actual = italic.GetText();

            actual.Should().BeEquivalentTo($"<em>{Text1}</em>");
        }

        [Test]
        public void GetText_WithSeveralChildren_ShouldReturnJoinedTextsOfChildrenInEmTag()
        {
            var italic = new ItalicNode();
            italic.AddNode(new PlainTextNode(Text1));
            italic.AddNode(new PlainTextNode(Text2));

            var actual = italic.GetText();

            actual.Should().BeEquivalentTo($"<em>{Text1}{Text2}</em>");
        }
    }
}