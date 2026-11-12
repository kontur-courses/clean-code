using NUnit.Framework;
using Markdown;

namespace Markdown.Tests
{
    [TestFixture]
    public class MarkdownTests
    {
        [TestCase("Обычный текст", "<p>Обычный текст</p>")]
        [TestCase("# Заголовок", "<h1>Заголовок</h1>")]
        [TestCase("Текст _курсив_", "<p>Текст <em>курсив</em></p>")]
        [TestCase("# Заголовок __жирный__", "<h1>Заголовок <strong>жирный</strong></h1>")]
        public void Parse_ShouldWrapInCorrectHtmlTag(string input, string expected)
        {
            Assert.That(Md.Render(input), Is.EqualTo(expected));
        }

        [TestCase("_курсив_", "<p><em>курсив</em></p>")]
        [TestCase("__жирный__", "<p><strong>жирный</strong></p>")]
        [TestCase("слово _слово_ слово", "<p>слово <em>слово</em> слово</p>")]
        public void Parse_BasicFormatting(string input, string expected)
        {
            Assert.That(Md.Render(input), Is.EqualTo(expected));
        }

        [TestCase("\\_не курсив\\_", "<p>_не курсив_</p>")]
        [TestCase("\\__не жирный\\__", "<p>__не жирный__</p>")]
        [TestCase("экран\\ирование", "<p>экранирование</p>")]
        [TestCase("много экранирования \\\\", "<p>много экранирования \\</p>")]
        public void Parse_Escaping(string input, string expected)
        {
            Assert.That(Md.Render(input), Is.EqualTo(expected));
        }

        [TestCase("__жирный _курсив_ жирный__", "<p><strong>жирный <em>курсив</em> жирный</strong></p>")]
        [TestCase("_курсив __жирный__ курсив_", "<p><em>курсив __жирный__ курсив</em></p>")]
        public void Parse_NestedTags(string input, string expected)
        {
            Assert.That(Md.Render(input), Is.EqualTo(expected));
        }

        [TestCase("цифры_12_3", "<p>цифры_12_3</p>")]
        [TestCase("нач_але_", "<p>нач<em>але</em></p>")]
        [TestCase("сер_еди_не", "<p>сер<em>еди</em>не</p>")]
        [TestCase("кон_це_", "<p>кон<em>це</em></p>")]
        [TestCase("ра_зные сл_ова", "<p>ра_зные сл_ова</p>")]
        public void Parse_WordBoundaries(string input, string expected)
        {
            Assert.That(Md.Render(input), Is.EqualTo(expected));
        }

        [TestCase("_ пробел_", "<p>_ пробел_</p>")]
        [TestCase("_пробел _", "<p>_пробел _</p>")]
        [TestCase("____", "<p>____</p>")]
        [TestCase("__  __", "<p>__  __</p>")]
        public void Parse_SpacesAndEmptyTags(string input, string expected)
        {
            Assert.That(Md.Render(input), Is.EqualTo(expected));
        }

        [TestCase("_непарные", "<p>_непарные</p>")]
        [TestCase("непарные_", "<p>непарные_</p>")]
        public void Parse_UnpairedAndIntersections(string input, string expected)
        {
            Assert.That(Md.Render(input), Is.EqualTo(expected));
        }

        [Test]
        public void Parse_ComplexHeaderFromSpecification()
        {
            var input = "# Заголовок с __разными__ символами _тут_";
            var expected = "<h1>Заголовок с <strong>разными</strong> символами <em>тут</em></h1>";

            Assert.That(Md.Render(input), Is.EqualTo(expected));
        }
    }
}