using System;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Test
    {
        [Test]
        public void Render_ShouldReturn_HtmlString()
        {
            const string text = "# Заголовок __с _разными_ символами__";
            const string expectedResult = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>";
            Md.Render(text).Should().Be(expectedResult);
        }

        [Test]
        [Timeout(1000)]
        public void Render_ShouldWorkFast_OnBigText()
        {
            var text = string.Concat(Enumerable.Repeat("# Заголовок __с _разными_ символами__", 50000));
            Md.Render(text);
        }
    }
}