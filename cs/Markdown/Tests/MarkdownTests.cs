using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
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
            return MarkdownTransformerToHtml.Render(inputString);
        }

        [TestCase("__Hello World!__", ExpectedResult = "<strong>Hello World!</strong>")]
        [TestCase("Hello __World!__", ExpectedResult = "Hello <strong>World!</strong>")]
        [TestCase("__Hello__ __World!__", ExpectedResult = "<strong>Hello</strong> <strong>World!</strong>")]
        public string CorrectStrongTags(string inputString)
        {
            return MarkdownTransformerToHtml.Render(inputString);
        }

        [TestCase("\n>a\n", ExpectedResult = "\n<blockquote>a</blockquote>")]
        [TestCase("_aa_\n>a\n>a\n", ExpectedResult = "<em>aa</em>\n<blockquote>a</blockquote><blockquote>a</blockquote>")]
        public string CorrectBlockquoteTags(string inputString)
        {
            return MarkdownTransformerToHtml.Render(inputString);
        }

        [TestCase(@"\_Hello World!\_", ExpectedResult = "_Hello World!_")]
        [TestCase(@"_Hello World!\_", ExpectedResult = "_Hello World!_")]
        [TestCase(@"\_Hello World!_", ExpectedResult = "_Hello World!_")]
        [TestCase(@"\__Hello World!\__", ExpectedResult = "__Hello World!__")]
        [TestCase(@"__Hello World!\__", ExpectedResult = "__Hello World!__")]
        [TestCase(@"\__Hello World!__", ExpectedResult = "__Hello World!__")]
        [TestCase(@"__\___", ExpectedResult = @"_____", TestName = "EscapedTagInsideTagsNotRecognise")]
        public string EscapedTagsNotRecognized(string inputString)
        {
            return MarkdownTransformerToHtml.Render(inputString);
        }

        [TestCase(@"\", ExpectedResult = "")]
        [TestCase(@"\\", ExpectedResult = "")]
        [TestCase(@"help\", ExpectedResult = "help")]
        [TestCase(@"\help", ExpectedResult = "help")]
        public string AllEscapeSymbolsRemovedFromString(string inputString)
        {
            return MarkdownTransformerToHtml.Render(inputString);
        }

        [TestCase("123_456_789", ExpectedResult = "123_456_789")]
        [TestCase("123__456__789", ExpectedResult = "123__456__789")]
        [TestCase("numbers_12_3", ExpectedResult = "numbers_12_3")]
        [TestCase("numbers__12_3", ExpectedResult = "numbers__12_3")]
        public string TagsBetweenNumbersNotRecognized(string inputString)
        {
            return MarkdownTransformerToHtml.Render(inputString);
        }

        [TestCase("Hello_World!", ExpectedResult = "Hello_World!")]
        [TestCase("_Hello World!", ExpectedResult = "_Hello World!")]
        [TestCase("Hello World!_", ExpectedResult = "Hello World!_")]
        [TestCase("__Hello World!", ExpectedResult = "__Hello World!")]
        [TestCase("Hello World!__", ExpectedResult = "Hello World!__")]
        [TestCase("_aa _aa__ aa__", ExpectedResult = "_aa _aa__ aa__")]
        [TestCase(@"_aaa _a _a aaa_", ExpectedResult = @"_aaa _a <em>a aaa</em>")]
        [TestCase("__aaa a_ a_ aaa__", ExpectedResult = "<strong>aaa a_ a_ aaa</strong>")]
        public string UnpairedTagsNotRecognized(string inputString)
        {
            return MarkdownTransformerToHtml.Render(inputString);
        }

        [TestCase("__Hello _Hello World!_ World!__", ExpectedResult = "<strong>Hello <em>Hello World!</em> World!</strong>", TestName = "EmphasisInsideStrongRecognized")]
        [TestCase("_Hello __Hello World!__ World!_", ExpectedResult = "<em>Hello __Hello World!__ World!</em>", TestName = "StrongInsideEmphasisNotRecognized")]
        [TestCase("__aa _aa_ __aa__ aa__", ExpectedResult = "<strong>aa <em>aa</em> __aa</strong> aa__")]
        [TestCase("__a _a _a_ a__", ExpectedResult = "<strong>a _a <em>a</em> a</strong>")]
        [TestCase("__a _a _a_ a_ a__", ExpectedResult = "<strong>a <em>a _a</em> a_ a</strong>")]
        [TestCase("_a __aa _a_ aa__ a_", ExpectedResult = "<em>a __aa _a</em> aa__ a_")]
        [TestCase(@"\_a __aa _a_ aa__ a\_", ExpectedResult =  "_a <strong>aa <em>a</em> aa</strong> a_")]
        public string NestingTags(string inputString)
        {
            return MarkdownTransformerToHtml.Render(inputString);
        }

        [TestCase("_a_ __a__ _a_", ExpectedResult = "<em>a</em> <strong>a</strong> <em>a</em>", TestName = "TwoEmphasisOneStrongTags")]
        [TestCase("__a__ __a__ _a_ _a_", ExpectedResult = "<strong>a</strong> <strong>a</strong> <em>a</em> <em>a</em>", TestName = "TwoStrongTwoEmphasisTags")]
        public string MultipleTagsInOneStringRecognized(string inputString)
        {
            return MarkdownTransformerToHtml.Render(inputString);
        }
        
        [TestCase("__aa _a aa__ a_", ExpectedResult = "")]
        [TestCase("", ExpectedResult = "", TestName = "EmptyString")]
        [TestCase("_", ExpectedResult = "_", TestName = "SingleEmphasisTag")]
        [TestCase("__", ExpectedResult = "__", TestName = "SingleStrongTag")]
        [TestCase("_111_",ExpectedResult = "<em>111</em>", TestName = "NumbersInsideTag")]
        [TestCase("___Hello World!___", ExpectedResult = "___Hello World!___", TestName = "TwoDifferentTagsNear")]
        [TestCase("_____Hello World!_____", ExpectedResult = "_____Hello World!_____", TestName = "ManyTagsNear")]
        public string ExtremeCases(string input)
        {
            return MarkdownTransformerToHtml.Render(input);
        }

        [TestCaseSource(nameof(TestLines))]
        public void AlgorithСomplexityMustBeLinear(string testString)
        {
            var firstTimeResult = GetRenderTime(testString);
            var secondTimeResult = GetRenderTime(testString + testString);

            (secondTimeResult / firstTimeResult).Should().BeLessThan(2.1);
        }

        private static double GetRenderTime(string inputString)
        {
            var timer = new Stopwatch();

            timer.Start();
            MarkdownTransformerToHtml.Render(inputString);
            timer.Stop();

            return timer.ElapsedMilliseconds;
        }

        private static IEnumerable<TestCaseData> TestLines
        {
            get
            { 
                yield return new TestCaseData(TextCreator.CreateText(1000))
                    .SetName("1000 Substrings");
                yield return new TestCaseData(TextCreator.CreateText(10000))
                    .SetName("10000 Substrings");
                yield return new TestCaseData(TextCreator.CreateText(100000))
                    .SetName("100000 Substrings");
                yield return new TestCaseData(TextCreator.CreateText(1000000))
                    .SetName("1000000 Substrings");
            }
        }
        
    }
}
