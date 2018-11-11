using System;
using Markdown;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Should
    {
        [TestCase("plain text", ExpectedResult = "plain text", TestName = "Plain text as is")]

        [TestCase("_undersored_ text", ExpectedResult = "<em>undersored</em> text", TestName = "Underscored text in italic")]
        [TestCase("_undersored_ and again _undersored_ text", ExpectedResult = "<em>undersored</em> and again <em>undersored</em> text", 
            TestName = "Several occurences of underscored text in italic")]
        [TestCase(@"ecranned \_undersored\_ text", ExpectedResult = @"ecranned \_undersored\_ text", 
            TestName = "Ecranned underscored text in plain text")]

        [TestCase("double __undersored__ text", ExpectedResult = "double <strong>undersored</strong> text", 
            TestName = "Double underscored text in bold")]
        [TestCase(@"double \__undersored\__ text", ExpectedResult = @"double \__undersored\__ text",
            TestName = "Ecranned double underscored text in plain text")]
        [TestCase("double __undersored__ and again double __undersored__ text",
            ExpectedResult = "double <strong>undersored</strong> and again double <strong>undersored</strong> text",
            TestName = "Several occurences of double underscored text in bold")]
        public string Render(string markdown)
        {
            var md = new Md();
            return md.Render(markdown);

        }
    }
}
