using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MdShould
    {
        [TestCase("Текст, _окруженный с двух сторон_", "Текст, \\<em>окруженный с двух сторон\\</em>", TestName = "Simple Italic test with one tag")]
        [TestCase("__Выделенный двумя символами текст__", "<strong> Выделенный двумя символами текст </strong>",
            TestName = "Simple bold test")]
        [TestCase("\\_Вот это\\_", "_Вот это_",
            TestName = "Simple eeem test with two tags in row")]
        [TestCase("# Заголовок _с __разными__ символами_", "<Italic>text</Italic> <Italic>text</Italic>",
            TestName = "Field")]
        [TestCase("# Заголовок _с __разными__ символами_", "<Italic>text</Italic> <Italic>text</Italic>", TestName = "digits")]

        public void EmTagTests(string mdString, string htmlString)
        {
            var result = Md.Render(mdString);
            result.Should().Be(htmlString);
        }

        [Test]
        public void Test()
        {
            var mdString = "# markdown _test_ sentence__";
            Md.Render(mdString).Should().Be("<h1>markdown <em>test</em> sentence</strong></h1>");
        }
    }

}
