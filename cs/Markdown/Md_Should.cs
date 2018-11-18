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
                new MdWrappingTagLocator("__",HtmlTextWriterTag.Strong,new Lazy<Md>(()=>rendererer)),
                new MdWrappingTagLocator("_",HtmlTextWriterTag.U,new Lazy<Md>(()=>rendererer)),
            });
        }

        [TestCase("__Hello__","<strong>Hello</strong>")]
        [TestCase("__Hello world!__","<strong>Hello world!</strong>")]
        [TestCase("_Hello_","<u>Hello</u>")]
        [TestCase("_Hello world!_","<u>Hello world!</u>")]
        public void RenderIntoHTML(string markdowned, string expectedRendered) =>
            rendererer.Render(markdowned).Should().Be(expectedRendered);
    }
}