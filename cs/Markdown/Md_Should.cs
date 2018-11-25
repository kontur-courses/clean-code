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
            rendererer = new Md();
        }

        [TestCase("#Hello world!\n",
            @"<h1>
	Hello world!
</h1>")]
        [TestCase("##Hello world!\n",@"<h2>
	Hello world!
</h2>")]
        [TestCase("###Hello world!\n",@"<h3>
	Hello world!
</h3>")]
        [TestCase("__Hello__","<strong>Hello</strong>")]
        [TestCase("__Hello world!__","<strong>Hello world!</strong>")]
        [TestCase("_Hello_","<u>Hello</u>")]
        [TestCase("_Hello world!_","<u>Hello world!</u>")]
        [TestCase("_Hello world!","_Hello world!")]
        [TestCase("Hello_ world!","Hello_ world!")]
        [TestCase("Hello __world!","Hello __world!")]
        [TestCase("Hello world__!","Hello world__!")]
        [TestCase("__Hello _markdown_ world!__","<strong>Hello <u>markdown</u> world!</strong>")]
        [TestCase("_Hello __markdown__ world!_","<u>Hello __markdown__ world!</u>")]
        [TestCase("*Hello world!*","<u>Hello world!</u>")]
        [TestCase("**Hello world!**","<strong>Hello world!</strong>")]
        [TestCase("_Hello _Hello _Hello world!_ world!_ world!_","<u>Hello <u>Hello <u>Hello world!</u> world!</u> world!</u>")]
        [TestCase("_Hello _Hello _Hello world!_ world!_ world!_ world!_","<u>Hello <u>Hello <u>Hello world!</u> world!</u> world!</u> world!_")]
        [TestCase("_Hello _Hello _Hello _Hello world!_ world!_ world!_","_Hello <u>Hello <u>Hello <u>Hello world!</u> world!</u> world!</u>")]
        //[TestCase("Hello world!","<u>Hello world!</u>")]
        public void RenderIntoHtml(string markdowned, string expectedRendered) =>
            rendererer.Render(markdowned).Should().Be(expectedRendered);
    }
}