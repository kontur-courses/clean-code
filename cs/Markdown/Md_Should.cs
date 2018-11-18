using System;
using System.IO;
using System.Web.UI;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    
    [TestFixture]
    public class Md_Should
    {
        private Md rendererer;
        [SetUp]
        public void SetUp()
        {
            rendererer = new Md(new []
            {
                new MdWrappingTagMatcher("__",HtmlTextWriterTag.Strong,new Lazy<Md>(()=>rendererer)),
                new MdWrappingTagMatcher("_",HtmlTextWriterTag.U,new Lazy<Md>(()=>rendererer)),
            });
        }

        [TestCase("__Hello__","<strong>Hello</strong>")]
        [TestCase("__Hello world!__","<strong>Hello world!</strong>")]
        [TestCase("_Hello_","<u>Hello</u>")]
        [TestCase("_Hello world!_","<u>Hello world!</u>")]
        [TestCase("___Hello world!___","<strong><u>Hello world!</u></strong>")]
        [TestCase("_Hello world!","_Hello world!")]
        [TestCase("Hello_ world!","Hello_ world!")]
        [TestCase("Hello __world!","Hello __world!")]
        [TestCase("Hello world__!","Hello world__!")]
        public void RenderIntoHtml(string markdowned, string expectedRendered) =>
            rendererer.Render(markdowned).Should().Be(expectedRendered);
    }
}