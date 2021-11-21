using Markdown;
using NUnit.Framework;

namespace MarkdownUnitTests
{
    public class MdTest
    {
        [TestCase("#  Заголовок __с _разными_ символами__\n#  Заголовок __с _разными_ символами__\n", ExpectedResult = "<h1> Заголовок <strong>с <em>разными</em> символами</strong></h1>\n<h1> Заголовок <strong>с <em>разными</em> символами</strong></h1>\n")]
        public string Render_ShouldBeCorrectly_When(string input)
        {
            var mdRender = new Md(new HtmlConvector());

            return mdRender.Render(input);
        }
    }
}




