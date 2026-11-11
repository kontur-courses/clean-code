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
            document.AddParagraph("a");
            document.AddParagraph("b");

            var html = document.Build();

            html
                .Should()
                .Be("a\nb");
        }

        [Test]
        public void Build_ShouldWrapListItemsInUl_WhenTheyGoInRow_Test()
        {
            var document = new DocumentBuilder();
            document.AddListItem("<li>a</li>");
            document.AddListItem("<li>b</li>");

            var html = document.Build();

            html
                .Should()
                .Be("<ul><li>a</li>\n<li>b</li></ul>");
        }

        [Test]
        public void Build_ShouldOpenList_WhenListItemGoesAfterParagraph_Test()
        {
            var document = new DocumentBuilder();
            document.AddParagraph("a");
            document.AddListItem("<li>b</li>");

            var html = document.Build();

            html
                .Should()
                .Be("a\n<ul><li>b</li></ul>");
        }
    }
}
