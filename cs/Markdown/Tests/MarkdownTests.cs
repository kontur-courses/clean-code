using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    internal class MarkdownTests
    {
        [TestCase("_Hello World!_", ExpectedResult = "<em>Hello World!</em>")]
        [TestCase("Hello _World!_", ExpectedResult = "Hello <em>World!</em>")]
        [TestCase("_Hello_ _World!_", ExpectedResult = "<em>Hello</em> <em>World!</em>")]
        public string CorrectEmphasisTags(string inputString)
        {
            return Markdown.Render(inputString);
        }

        [TestCase("__Hello World!__", ExpectedResult = "<strong>Hello World!</strong>")]
        [TestCase("Hello __World!__", ExpectedResult = "Hello <strong>World!</strong>")]
        [TestCase("__Hello__ __World!__", ExpectedResult = "<strong>Hello</strong> <strong>World!</strong>")]
        public string CorrectStrongTags(string inputString)
        {
            return Markdown.Render(inputString);
        }

        [TestCase(@"\_Hello World!\_", ExpectedResult = "_Hello World!_")]
        [TestCase(@"_Hello World!\_", ExpectedResult = "_Hello World!_")]
        [TestCase(@"\_Hello World!_", ExpectedResult = "_Hello World!_")]
        [TestCase(@"\__Hello World!\__", ExpectedResult = "__Hello World!__")]
        [TestCase(@"__Hello World!\__", ExpectedResult = "__Hello World!__")]
        [TestCase(@"\__Hello World!__", ExpectedResult = "__Hello World!__")]
        public string EscapedTags(string inputString)
        {
            return Markdown.Render(inputString);
        }

        [TestCase(@"\", ExpectedResult = "")]
        [TestCase(@"\\", ExpectedResult = "")]
        [TestCase(@"help\", ExpectedResult = "help")]
        [TestCase(@"\help", ExpectedResult = "help")]
        public string OnlyEscapedSymbols(string inputString)
        {
            return Markdown.Render(inputString);
        }

        [TestCase("123_456_789", ExpectedResult = "123_456_789")]
        [TestCase("123__456__789", ExpectedResult = "123__456__789")]
        [TestCase("numbers_12_3", ExpectedResult = "numbers_12_3")]
        [TestCase("numbers__12_3", ExpectedResult = "numbers__12_3")]
        public string TagsBetweenNumbers(string inputString)
        {
            return Markdown.Render(inputString);
        }

        [TestCase("Hello_World!", ExpectedResult = "Hello_World!")]
        [TestCase("_Hello World!", ExpectedResult = "_Hello World!")]
        [TestCase("Hello World!_", ExpectedResult = "Hello World!_")]
        [TestCase("__Hello World!", ExpectedResult = "__Hello World!")]
        [TestCase("Hello World!__", ExpectedResult = "Hello World!__")]
        [TestCase("_aa _aa__ aa__", ExpectedResult = "_aa _aa__ aa__")]
        public string UnopenedTags(string inputString)
        {
            return Markdown.Render(inputString);
        }

        [TestCase("__Hello _Hello World!_ World!__", ExpectedResult = "<strong>Hello <em>Hello World!</em> World!</strong>", TestName = "EmphasisInsideStrong")]
        [TestCase("_Hello __Hello World!__ World!_", ExpectedResult = "<em>Hello __Hello World!__ World!</em>", TestName = "StrongInsideEmphasis")]
        public string NestingTags(string inputString)
        {
            return Markdown.Render(inputString);
        }

        [TestCase("_a_ __a__ _a_", ExpectedResult = "<em>a</em> <strong>a</strong> <em>a</em>", TestName = "TwoEmphasisOneStrongTags")]
        public string MultipleTagsInOneString(string inputString)
        {
            return Markdown.Render(inputString);
        }

        [TestCase("___Hello World!___", ExpectedResult = "___Hello World!___", TestName = "TwoDifferentTagsNear")]
        [TestCase("_____Hello World!_____", ExpectedResult = "_____Hello World!_____", TestName = "ManyTagsNear")]
        [TestCase("__aaa a_ a_ aaa__", ExpectedResult = "<strong>aaa a_ a_ aaa</strong>", TestName = "UnopenedTagsInsideOtherTags")]
        [TestCase("", ExpectedResult = "", TestName = "EmptyString")]
        [TestCase("_", ExpectedResult = "_", TestName = "SingleEmphasisTag")]
        [TestCase("__", ExpectedResult = "__", TestName = "SingleStrongTag")]
        [TestCase("_111_",ExpectedResult = "<em>111</em>", TestName = "NumbersInsideTag")]
        [TestCase(@"__\___", ExpectedResult = @"_____")]
        [TestCase(@"__aaa _a _a aaa__", ExpectedResult = @"__aaa _a _a aaa__", TestName = "OpeningTagsNotEscaped")]
        [TestCase(@"__aaa \_a \_a aaa__", ExpectedResult = @"<strong>aaa _a _a aaa</strong>", TestName = "OpeningTagsEscaped")]
        public string ExtremeCases(string input)
        {
            return Markdown.Render(input);
        }
    }
}
