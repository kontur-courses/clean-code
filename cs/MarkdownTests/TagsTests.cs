using FluentAssertions;
using Markdown.LinkTags;
using Markdown.PairTags;
using NUnit.Framework;

namespace MarkdownTests
{
    public class TagsTests
    {
        [TestCase("_a", 0, true)]
        [TestCase("_a", 1, false)]
        [TestCase("a_", 1, false)]
        [TestCase("__aa", 0, false)]
        [TestCase("__aa", 1, true)]
        [TestCase("_a_", 2, false)]
        [TestCase("_a_", 0, true)]
        [TestCase("1_123", 1, false)]
        [TestCase("a_123", 1, false)]
        [TestCase("1_a23", 1, false)]
        [TestCase("_", 0, false)]
        [TestCase("_ ads", 0, false)]
        [TestCase("ds_da", 2, true)]
        [TestCase("___a___", 0, false)]
        [TestCase("___a___", 1, false)]
        [TestCase("___a___", 2, true)]
        public void OpeningItalic(string text, int position, bool expected)
        {
            var prefixItalic = new OpeningItalic();
            var result = prefixItalic.CheckForCompliance(text, position);
            result.Should().Be(expected);
        }


        [TestCase("_a", 0, false)]
        [TestCase("_a", 1, false)]
        [TestCase("a_", 1, true)]
        [TestCase("__aa", 0, false)]
        [TestCase("__aa", 1, false)]
        [TestCase("_a_", 2, true)]
        [TestCase("_a_", 0, false)]
        [TestCase("1_123", 1, false)]
        [TestCase("a_123", 1, false)]
        [TestCase("1_a23", 1, false)]
        [TestCase("_", 0, false)]
        [TestCase("_ ads", 0, false)]
        [TestCase("abc _", 4, false)]
        [TestCase("ds_da", 2, true)]
        [TestCase("___a___", 6, false)]
        [TestCase("___a___", 5, false)]
        [TestCase("___a___", 4, true)]
        public void ClosingItalic(string text, int position, bool expected)
        {
            var postfixItalic = new ClosingItalic();
            var result = postfixItalic.CheckForCompliance(text, position);
            result.Should().Be(expected);
        }


        [TestCase("__ab", 0, true)]
        [TestCase("__ab", 1, false)]
        [TestCase("_ab", 0, false)]
        [TestCase("__", 0, false)]
        [TestCase("ab__", 2, false)]
        [TestCase("ab__", 3, false)]
        [TestCase("__ ", 0, false)]
        [TestCase("asd__sad", 3, true)]
        [TestCase("asd__sad", 4, false)]
        [TestCase("___a", 0, true)]
        [TestCase("___a", 1, true)]
        [TestCase("___a", 2, false)]
        [TestCase("a___", 2, false)]
        [TestCase("a___", 3, false)]
        [TestCase("a___", 4, false)]
        public void OpeningBold(string text, int position, bool expected)
        {
            var prefixBold = new OpeningBold();
            var result = prefixBold.CheckForCompliance(text, position);
            result.Should().Be(expected);
        }


        [TestCase("b__", 2, false)]
        [TestCase("b__", 1, true)]
        [TestCase("a_", 1, false)]
        [TestCase("_ab", 0, false)]
        [TestCase("_ab", 0, false)]
        [TestCase("__ab", 0, false)]
        [TestCase("__ab", 1, false)]
        [TestCase("__", 0, false)]
        [TestCase("__", 1, false)]
        [TestCase(" __", 2, false)]
        [TestCase("asd__sad", 3, true)]
        [TestCase("a___", 1, true)]
        [TestCase("a___", 2, true)]
        [TestCase("a___", 3, false)]
        public void ClosingBold(string text, int position, bool expected)
        {
            var postfixBold = new ClosingBold();
            var result = postfixBold.CheckForCompliance(text, position);
            result.Should().Be(expected);
        }


        [TestCase("[1234](abc)", true, TestName = "valid link")]
        [TestCase("![1234](abc)", false, TestName = "is not a image")]
        [TestCase("[[1234]]((abc))", false, TestName = "does not support brackets nesting")]
        [TestCase("[1234(abc)", false, TestName = "bracket not closed")]
        [TestCase("[1234](abc", false, TestName = "square bracket not closed")]
        [TestCase("a[1234](abc)", false, TestName = "characters before the tag")]
        [TestCase("[1234](abc)b", false, TestName = "characters after the tag")]
        [TestCase("[1234] (abc)", false, TestName = "characters between brackets")]
        [TestCase("![]()", false, TestName = "empty padding")]
        public void LinkTryParseShould_Correspond(string text, bool expected)
        {
            var l = new Link();

            var result = l.TryParse(text, text.IndexOf('['), out _);

            result.Should().Be(expected);
        }


        [TestCase("![1234](abc)", true, TestName = "valid image")]
        [TestCase("[1234](abc)", false, TestName = "is not a link")]
        [TestCase("![[1234]](abc)", false, TestName = "does not support brackets nesting1")]
        [TestCase("![1234]](abc)", false, TestName = "does not support brackets nesting2")]
        [TestCase("!{1234}(abc)", false, TestName = "curly instead of square")]
        [TestCase("![1234] (abc)", false, TestName = "string between brackets")]
        [TestCase("! [1234](abc)", false, TestName = "space between ! and ]")]
        [TestCase("![123(4a]bc)", false, TestName = "brackets closed incorrectly")]
        [TestCase("![]()", false, TestName = "empty padding")]
        public void ImageTryParseShould_Correspond(string text, bool expected)
        {
            var img = new Image();

            var result = img.TryParse(text, text.IndexOf('!'), out _);

            result.Should().Be(expected);
        }
    }
}