using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace MarkdownTest
{
    public class Tests
    {


        [Test]
        public void CheckHeaderOnly()
        {
            var html = Md.Render("#");

            html.Should().Be(@"<h1><\h1>");
        }

        [Test]
        public void CheckAllInputInEmTag()
        {
            var html = Md.Render("_faq fgggf df_");

            html.Should().Be(@"<em>faq fgggf df</em>");
        }

        [Test]
        public void CheckTwoEmTag()
        {
            var html = Md.Render("_faq_ fgggf _df_");

            html.Should().Be(@"<em>faq</em> fgggf <em>df</em>");
        }

        [Test]
        public void CheckWordInEmTag()
        {
            var html = Md.Render("faq _fgggf_ df");

            html.Should().Be(@"faq <em>fgggf</em> df");
        }

        [Test]
        public void CheckCommentEmTag()
        {
            var html = Md.Render("faq \\_fgggf_ df");

            html.Should().Be(@"faq \_fgggf_ df");
        }
        

    }
}  