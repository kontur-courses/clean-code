using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    class Markdown_Should
    {
        static Markdown _markdown;

        [SetUp]
        public void SetUp() => _markdown = new Markdown();

        //Пока что тесты в таком виде, чуть позже разделю их по категориям :)

        [TestCase("_Hello World!_", "<em>Hello World!</em>")]
        [TestCase("__Hello World!__", "<strong>Hello World!</strong>")]
        [TestCase(@"\_Hello World!\_", "_Hello World!_")]
        [TestCase(@"_Hello World!\_", "_Hello World!_")]
        [TestCase(@"\_Hello World!_", "_Hello World!_")]
        [TestCase("123_456_789", "123_456_789")]
        [TestCase("123__456__789", "123__456__789")]
        [TestCase("numbers_12_3", "numbers_12_3")]
        [TestCase("Hello_World!", "Hello_World!")]
        [TestCase("_Hello World!", "_Hello World!")]
        [TestCase("Hello World!_", "Hello World!_")]
        [TestCase("__Hello World!", "__Hello World!")]
        [TestCase("Hello World!__", "Hello World!__")]
        [TestCase("__Hello World!", "__Hello World!")]
        [TestCase("Hello World!__", "Hello World!__")]
        [TestCase("этот_ символ_ не считается", "этот_ символ_ не считается")]
        [TestCase("этот _cимвол _не считаются", "этот _cимвол _не считаются")]
        [TestCase("___Hello World!___", "___Hello World!___")]
        public void MarkDownRender(string input, string expected)
        {
            var actual = _markdown.Render(input);

            actual.Should().Be(expected);
        }
    }
}
