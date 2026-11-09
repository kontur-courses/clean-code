using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class DocumentBuilderTests
    {
        [Test]
        public void Build_ShouldJoinParagraphs_WhenTheyAreNotListItems_Test()
        {
            var document = new DocumentBuilder();
            document.AddParagraph("a", false);
            document.AddParagraph("b", false);

            var html = document.Build();

            html
                .Should()
                .Be("a\nb");
        }

        [Test]
        public void Build_ShouldWrapListItemsInUl_WhenTheyGoInRow_Test()
        {
            var document = new DocumentBuilder();
            document.AddParagraph("<li>a</li>", true);
            document.AddParagraph("<li>b</li>", true);

            var html = document.Build();

            html
                .Should()
                .Be("<ul><li>a</li>\n<li>b</li></ul>");
        }

        [Test]
        public void Build_ShouldOpenList_WhenListItemGoesAfterParagraph_Test()
        {
            var document = new DocumentBuilder();
            document.AddParagraph("a", false);
            document.AddParagraph("<li>b</li>", true);

            var html = document.Build();

            html
                .Should()
                .Be("a\n<ul><li>b</li></ul>");
        }
    }
}
