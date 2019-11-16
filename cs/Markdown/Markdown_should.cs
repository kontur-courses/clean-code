using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    class Markdown_Should
    {
        static Markdown markdown;

        [SetUp]
        public void SetUp() => markdown = new Markdown();

        [TestCase("_Hello World!_", ExpectedResult = "<em>Hello World!</em>")]
        [TestCase("Hello _World!_", ExpectedResult = "Hello <em>World!</em>")]
        [TestCase("_Hello_ _World!_", ExpectedResult = "<em>Hello</em> <em>World!</em>")]
        public string Render_ShouldCanChange_SingleCharacterTags(string inputString)
        {
            return markdown.Render(inputString);
        }

        [TestCase("__Hello World!__", ExpectedResult = "<strong>Hello World!</strong>")]
        [TestCase("Hello __World!__", ExpectedResult = "Hello <strong>World!</strong>")]
        [TestCase("__Hello__ __World!__", ExpectedResult = "<strong>Hello</strong> <strong>World!</strong>")]
        public string Render_ShouldCanChange_DoubleCharacterTags(string inputString)
        {
            return markdown.Render(inputString);
        }

        [TestCase(@"\_Hello World!\_", ExpectedResult = "_Hello World!_")]
        [TestCase(@"_Hello World!\_", ExpectedResult = "_Hello World!_")]
        [TestCase(@"\_Hello World!_", ExpectedResult = "_Hello World!_")]
        [TestCase(@"\__Hello World!\__", ExpectedResult = "__Hello World!__")]
        [TestCase(@"__Hello World!\__", ExpectedResult = "__Hello World!__")]
        [TestCase(@"\__Hello World!__", ExpectedResult = "__Hello World!__")]
        public string Render_ShouldCan_DistinguishEscapedTags(string inputString)
        {
            return markdown.Render(inputString);
        }

        [TestCase("123_456_789", ExpectedResult = "123_456_789")]
        [TestCase("123__456__789", ExpectedResult = "123__456__789")]
        [TestCase("numbers_12_3", ExpectedResult = "numbers_12_3")]
        [TestCase("numbers__12_3", ExpectedResult = "numbers__12_3")]
        public string Render_ShouldBeNot_ChangeTagsBetweenNumbers(string inputString)
        {
            return markdown.Render(inputString);
        }

        [TestCase("Hello_World!", ExpectedResult = "Hello_World!")]
        [TestCase("_Hello World!", ExpectedResult = "_Hello World!")]
        [TestCase("Hello World!_", ExpectedResult = "Hello World!_")]
        [TestCase("__Hello World!", ExpectedResult = "__Hello World!")]
        [TestCase("Hello World!__", ExpectedResult = "Hello World!__")]
        [TestCase("этот_ символ_ не считается", ExpectedResult = "этот_ символ_ не считается")]
        [TestCase("этот _cимвол _не считается", ExpectedResult = "этот _cимвол _не считается")]
        public string Render_ShouldBeNot_ChangeUnclosedTags(string inputString)
        {
            return markdown.Render(inputString);
        }

        [TestCase("___Hello World!___", ExpectedResult = "___Hello World!___")]
        [TestCase("__Hello _Hello World!_ World!__", ExpectedResult = "<strong>Hello <em>Hello World!</em> World!</strong>")]
        [TestCase("_Hello __Hello World!__ World!_", ExpectedResult = "<em>Hello __Hello World!__ World!</em>")]
        public string MarkDownRender(string input)
        {
            return markdown.Render(input);
        }
    }
}
