using NUnit.Framework;
using Markdown;

namespace Md_Tests
{
    [TestFixture]
    public class Md_Tests
    {
        private Md md;
        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [TestCase("hello")]
        [TestCase("Hello")]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("hello world!")]
        public void Render_ReturnsInputValue_OnNoTags(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("_hello_", "<em>hello</em>")]
        [TestCase("_Hello_ _world_", "<em>Hello</em> <em>world</em>")]
        [TestCase("_Hello world_", "<em>Hello world</em>")]
        [TestCase("_Italic_ NotItalic", "<em>Italic</em> NotItalic")]
        [TestCase("_Italic_ NotItalic _Italic_", "<em>Italic</em> NotItalic <em>Italic</em>")]
        public void Render_SupportsItalicTag_OnDifferentWords(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("nn_III_nn", "nn<em>III</em>nn")]
        [TestCase("nn_III_", "nn<em>III</em>")]
        [TestCase("_III_nn", "<em>III</em>nn")]
        public void Render_SupportsItalicTag_InsideOneWord(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("__hello__", "<strong>hello</strong>")]
        [TestCase("__Hello__ __world__", "<strong>Hello</strong> <strong>world</strong>")]
        [TestCase("__Hello world__", "<strong>Hello world</strong>")]
        [TestCase("__Strong__ NotStrong", "<strong>Strong</strong> NotStrong")]
        [TestCase("__Strong__ NotStrong __Strong__", "<strong>Strong</strong> NotStrong <strong>Strong</strong>")]
        public void Render_SupportsStrongTag_OnDifferentWords(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("nn__SSS__nn", "nn<strong>SSS</strong>nn")]
        [TestCase("nn__SSS__", "nn<strong>SSS</strong>")]
        [TestCase("__SSS__nn", "<strong>SSS</strong>nn")]
        public void Render_SupportsStrongTag_InsideOneWord(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("hel__lo wo__rld")]
        [TestCase("hel_lo wo_rld")]
        public void Render_ReturnsInputValue_OnTagsInsideTwoDifferentWords(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("__Strong _Intersection__ Italic_", "<strong>Strong _Intersection</strong> Italic_")]
        public void Render_SupportsIntersectionTags(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("hel__12__lo")]
        [TestCase("hel_12_lo")]
        public void Render_ReturnsInputValue_OnNumberInTagInsideWord(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("__")]
        [TestCase("____")]
        public void Render_ReturnsInputValue_OnEmptyTags(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("__hello")]
        [TestCase("_hello")]
        [TestCase("hello__")]
        [TestCase("hello_")]
        [TestCase("hel__lo")]
        [TestCase("hel_lo")]
        public void Render_ReturnsInputValue_OnSingleTags(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("__hello \n world__")]
        [TestCase("_hello \n world_")]
        [TestCase("__hello_ \n __world_")]
        public void Render_ReturnsInputValue_OnSingleTags_SupportingNewLine(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("__hello __")]
        [TestCase("__ hello__")]
        [TestCase("_hello _")]
        [TestCase("_ hello_")]
        public void Render_ReturnsInputValue_OnWhiteSpacesAroundTags(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("__Hello _my_", "__Hello <em>my</em>")]
        [TestCase("_Hello __my__", "_Hello <strong>my</strong>")]
        [TestCase("__Hello _my_ world__", "<strong>Hello <em>my</em> world</strong>")]
        [TestCase("_Hello __my__ world_", "<em>Hello __my__ world</em>")]
        [TestCase("___Hello my_ world__", "<strong><em>Hello my</em> world</strong>")]
        [TestCase("__Hello _my world___", "<strong>Hello _my world</strong>_")]
        [TestCase("___Hello my world___", "<strong>_Hello my world</strong>_")]
        [TestCase("___Hello my__ world_", "<strong>_Hello my</strong> world_")]
        public void Render_SupportsNestedTags(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("# Hello", "<h1>Hello</h1>")]
        [TestCase("# _Hello_", "<h1><em>Hello</em></h1>")]
        [TestCase("# __Hello__", "<h1><strong>Hello</strong></h1>")]
        [TestCase("# __Hello _my_ world__", "<h1><strong>Hello <em>my</em> world</strong></h1>")]
        [TestCase("# m__Hello__m", "<h1>m<strong>Hello</strong>m</h1>")]
        [TestCase("# Hello\n world", "<h1>Hello</h1> world")]
        [TestCase("_# Hello_", "<em># Hello</em>")]
        [TestCase("__# Hello__", "<strong># Hello</strong>")]
        public void Render_SupportsHeaderTag(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("\\_Hello_", "_Hello_")]
        [TestCase("Hel\\lo", "Hel\\lo")]
        [TestCase("_Hel\\lo_", "<em>Hel\\lo</em>")]
        [TestCase(@"__asd \_a_ ads__", "<strong>asd _a_ ads</strong>")]
        [TestCase(@"__asd _a\_ ads__", "<strong>asd _a_ ads</strong>")]
        public void Render_SupportsEscaping(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("__Hello m_y world__", "<strong>Hello m_y world</strong>")]
        [TestCase("__a_a_a_a__a_", "<strong>a<em>a</em>a_a</strong>a_")]
        [TestCase("_a__a_a__a_", "<em>a__a</em>a__a_")]
        [TestCase("__a_a__a_a__", "<strong>a_a</strong>a_a__")]
        public void Render_ReturnsCorrectResult_OnComplexCases(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        //[TestCase("$ref!word$", "<a href=ref>word</a>")]
        //[TestCase("$!word$", "$!word$")]
        //[TestCase("$ref!_word_$", "<a href=ref><em>word</em></a>")]
        //[TestCase("# $ref!___word___$", "<h1><a href=ref><strong><em>word</em></strong></a></h1>")]
        //public void Render_SupportsReferences(string input, string expected)
        //{
        //    var result = md.Render(input);

        //    Assert.AreEqual(expected, result);
        //}
    }
}