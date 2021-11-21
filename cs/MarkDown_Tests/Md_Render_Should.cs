using NUnit.Framework;
using Markdown;
using FluentAssertions;

namespace MarkDown_Tests
{
    public class Md_Render_Should
    {
        private static IMdSpecification _specification;
        private static Md _mdProcessor;

        [OneTimeSetUp]
        public static void SetUp()
        {
            _specification = new MdSpecification();
            _mdProcessor = new Md(_specification);
        }

        [Test]
        public static void ProcessStringWithoutTags_ToItself()
        {
            var mdText = "abcdefg";
            var expected = mdText;
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("__", "__")]
        [TestCase("____", "____")]
        [TestCase("_   _", "_   _")]
        [TestCase("__   __", "__   __")]
        public static void NotProcess_WhenEmptyStringInTag(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("_abc_d", "<em>abc</em>d")]
        [TestCase("a_bc_d", "a<em>bc</em>d")]
        [TestCase("a_bcd_", "a<em>bcd</em>")]
        [TestCase("_abc def_", "<em>abc def</em>")]
        public static void ProcessEmTag(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("__abc__d", "<strong>abc</strong>d")]
        [TestCase("a__bc__d", "a<strong>bc</strong>d")]
        [TestCase("a__bcd__", "a<strong>bcd</strong>")]
        [TestCase("__abc def__", "<strong>abc def</strong>")]
        public static void ProcessStrongTag(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("__a_b_c__d", "<strong>a<em>b</em>c</strong>d")]
        [TestCase("__a _b_ abc _c_ d__e", "<strong>a <em>b</em> abc <em>c</em> d</strong>e")]
        public static void ProcessEmTagInsideStrongTag(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("_a__b__c_d", "<em>a__b__c</em>d")]
        [TestCase("_a __b__ __c___d", "<em>a __b__ __c__</em>d")]
        public static void NotProcessStrongTagInsideEmTag(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("_123_", "_123_")]
        [TestCase("_ab1c_", "_ab1c_")]
        public static void NotProcessEm_WhenDigitInside(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("a_bc de_f", "a_bc de_f")]
        [TestCase("a__bc de__f", "a__bc de__f")]
        public static void NotProcessEmAndStrong_WhenDifferentWords(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("_a__bc_d__", "_a__bc_d__")]
        [TestCase("__a_bc__d_", "__a_bc__d_")]
        public static void NotProcessEmAndStrongTags_WhenHasIntersects(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }


        [TestCase("_abc__def", "_abc__def")]
        [TestCase("__abc_def", "__abc_def")]
        [TestCase("_abcdef", "_abcdef")]
        [TestCase("__abcdef", "__abcdef")]
        [TestCase("_abc\nd_ef", "_abc\nd_ef")]
        [TestCase("__abc\nd__ef", "__abc\nd__ef")]
        public static void NotProcessEmAndStrongTags_WhenHasNoPair(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("# abcd\n", "<h1>abcd</h1>")]
        [TestCase("# abcd", "<h1>abcd</h1>")]
        public static void ProcessHeadingTag(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("# a_bc_d\n", "<h1>a<em>bc</em>d</h1>")]
        [TestCase("# a__bc__d\n", "<h1>a<strong>bc</strong>d</h1>")]
        public static void ProcessHeadingTag_WithOneNestingLevel(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("# a__b_c_d__e\n", "<h1>a<strong>b<em>c</em>d</strong>e</h1>")]
        public static void ProcessHeadingTag_WithTwoNestingLevels(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase(@"\_abc_", "_abc_")]
        public static void NotProcess_EscapedTag(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase(@"\\", @"\")]
        public static void EscapeSymbol_EscapeItself(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase(@"a\bc", @"a\bc")]
        public static void EscapeSymbol_Remain_WhenNotEscapeSequence(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("a_ bc _de_f", "a_ bc <em>de</em>f")]
        [TestCase("a__ bc __de__f", "a__ bc <strong>de</strong>f")]
        public static void SkipEmAndStrong_WhenSpaceAfterOpenTag(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("_abc _de_ f", "<em>abc _de</em> f")]
        [TestCase("__abc __de__ f", "<strong>abc __de</strong> f")]
        public static void SkipEmAndStrong_WhenSpaceBeforeCloseTag(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }
    }
}