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
        public static void ProcessToItself_WhenStringWithoutTags()
        {
            var mdText = "abcdefg";
            _mdProcessor.Render(mdText).Should().Be(mdText);
        }

        [TestCase("__")]
        [TestCase("____")]
        [TestCase("_   _")]
        [TestCase("__   __")]
        public static void NotProcess_WhenEmptyStringInTag(string mdText)
        {
            _mdProcessor.Render(mdText).Should().Be(mdText);
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
        public static void ProcessEmTag_InsideStrongTag(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("_a__b__c_d", "<em>a__b__c</em>d")]
        [TestCase("_a __b__ __c___d", "<em>a __b__ __c__</em>d")]
        public static void NotProcessStrongTag_InsideEmTag(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase("_123_")]
        [TestCase("_ab1c_")]
        public static void NotProcessEm_WhenDigitInside(string mdText)
        {
            _mdProcessor.Render(mdText).Should().Be(mdText);
        }

        [TestCase("a_bc de_f")]
        [TestCase("a__bc de__f")]
        public static void NotProcessEmAndStrong_WhenPlacedInDifferentWords(string mdText)
        {
            _mdProcessor.Render(mdText).Should().Be(mdText);
        }

        [TestCase("_a__bc_d__")]
        [TestCase("__a_bc__d_")]
        public static void NotProcessEmAndStrongTags_WhenHasIntersects(string mdText)
        {
            _mdProcessor.Render(mdText).Should().Be(mdText);
        }

        [TestCase("_abc__def")]
        [TestCase("__abc_def")]
        [TestCase("_abcdef")]
        [TestCase("__abcdef")]
        [TestCase("_abc\nd_ef")]
        [TestCase("__abc\nd__ef")]
        public static void NotProcessEmAndStrongTags_WhenHasNoPair(string mdText)
        {
            _mdProcessor.Render(mdText).Should().Be(mdText);
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

        [Test]
        public static void ProcessHeadingTag_WithTwoNestingLevels()
        {
            var mdText = "# a __b _c_ d__ e\n";
            var expected = "<h1>a <strong>b <em>c</em> d</strong> e</h1>";
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase(@"\_abc_", "_abc_")]
        [TestCase(@"\__abc__", "__abc__")]
        [TestCase(@"\# abc\n", @"# abc\n")]
        public static void NotProcess_EscapedTag(string mdText, string expected)
        {
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        public static void EscapeSymbol_EscapeItself()
        {
            var mdText = @"\\";
            var expected = @"\";
            _mdProcessor.Render(mdText).Should().Be(expected);
        }

        [TestCase(@"a\bc")]
        [TestCase(@"a\ bc")]
        [TestCase(@"a\1bc")]
        public static void EscapeSymbol_Remain_WhenNotEscapeSequence(string mdText)
        {
            _mdProcessor.Render(mdText).Should().Be(mdText);
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