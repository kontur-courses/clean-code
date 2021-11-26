using System;
using System.Text;
using FluentAssertions;
using FluentAssertions.Extensions;
using Markdown.Converters;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class MdTests
    {
        [Test]
        public void Render_ReturnsEmptyString_WhenTextIsNull()
        {
            Md.Render(null).Should().Be("");
        }

        [TestCase("", TestName = "text is empty")]
        [TestCase("    ", TestName = "text is whitespace")]
        [TestCase("ahahahah", TestName = "text not contains tags")]
        [TestCase("_", TestName = "text contains not closed tag")]
        public void Render_ReturnsString_WhenItNotContainsTags(string text)
        {
            Md.Render(text).Should().BeEquivalentTo(text);
        }

        [TestCase("_a_", "<em>a</em>", TestName = "italics single tag")]
        [TestCase("#a\n", "<h1>a</h1>\n", TestName = "title closed single tag")]
        [TestCase("#a", "<h1>a</h1>", TestName = "title not closed single tag")]
        [TestCase("__a__", "<strong>a</strong>", TestName = "strong text single tag")]
        [TestCase("*a*", "<ul>a</ul>", TestName = "unnumbered list single tag")]
        [TestCase("+a+", "+a+", TestName = "list element single tag")]
        [TestCase("_a_ _a_ _a_", "<em>a</em> <em>a</em> <em>a</em>", TestName = "many tags of the same type")]
        [TestCase("#a\n _a_ __a__", "<h1>a</h1>\n <em>a</em> <strong>a</strong>",
            TestName = "many tags of different type")]
        [TestCase("#__b_a_b__", "<h1><strong>b<em>a</em>b</strong></h1>", TestName = "intersected tags")]
        [TestCase("\\_a\\_", "_a_", TestName = "escaped italic tag")]
        [TestCase("\\__a\\__", "__a__", TestName = "escaped strong text tag")]
        [TestCase("\\+a\\+", "+a+", TestName = "escaped list element tag")]
        [TestCase("\\*a\\*", "*a*", TestName = "escaped unnumbered list tag")]
        [TestCase("\\", "\\", TestName = "escaped escape symbol")]
        [TestCase("\\\\\\\\\\\\\\\\\\\\\\\\", "\\", TestName = "many slashes")]
        [TestCase("\\\\_a_", "\\<em>a</em>", TestName = "escaped escaped symbol before tag")]
        [TestCase("_b__a__b_", "<em>b__a__b</em>", TestName = "strong text tag cant be inside italics tag")]
        [TestCase("#*a*", "<h1>*a*</h1>", TestName = "unnumbered list cant be inside title tag")]
        [TestCase("#+a+", "<h1>+a+</h1>", TestName = "list element cant be inside title tag")]
        [TestCase("#*+a+*", "<h1>*+a+*</h1>",
            TestName = "list element cant be inside unnumbered list when it is not correct tag")]
        [TestCase("*__a__ _a_*", "<ul>__a__ _a_</ul>",
            TestName = "tags except list element cant be inside unnumbered list")]
        [TestCase("a_1_2_3", "a_1_2_3", TestName = "text with digits is not tag")]
        [TestCase("_d_og", "<em>d</em>og", TestName = "tag can be at the beginning of the word")]
        [TestCase("d_o_g", "d<em>o</em>g", TestName = "tag can be in the middle of the word")]
        [TestCase("do_g_", "do<em>g</em>", TestName = "tag can be at the end of the word")]
        [TestCase("do_g c_at", "do_g c_at", TestName = "tag cant be in different words")]
        [TestCase("_dog \n cat_", "_dog \n cat_", TestName = "tag cant be in different paragraphs")]
        [TestCase("_dog cat_", "<em>dog cat</em>", TestName = "many words can be inside tag")]
        [TestCase("_dog", "_dog", TestName = "recognizes unclosed tag")]
        [TestCase("dog_", "dog_", TestName = "recognizes unopened tag")]
        [TestCase("_ dog_", "_ dog_", TestName = "space after opening tag")]
        [TestCase("_dog _ cat_", "<em>dog _ cat</em>", TestName = "space before closing tag")]
        [TestCase("a__1__2__3", "a__1__2__3", TestName = "text with digits is not strong text tag")]
        [TestCase("__d__og", "<strong>d</strong>og", TestName = "strong text tag can be at the beginning of the word")]
        [TestCase("d__o__g", "d<strong>o</strong>g", TestName = "strong text tag can be in the middle of the word")]
        [TestCase("do__g__", "do<strong>g</strong>", TestName = "strong text tag can be at the end of the word")]
        [TestCase("do__g c__at", "do__g c__at", TestName = "strong text tag cant be in different words")]
        [TestCase("__dog cat__", "<strong>dog cat</strong>", TestName = "many words can be inside strong text tag")]
        [TestCase("__dog", "__dog", TestName = "recognizes unclosed strong text tag")]
        [TestCase("dog__", "dog__", TestName = "recognizes unopened strong text tag")]
        [TestCase("__ dog__", "__ dog__", TestName = "space after opening strong text tag")]
        [TestCase("__dog __ cat__", "<strong>dog __ cat</strong>", TestName = "space before closing strong text tag")]
        [TestCase("_", "_", TestName = "1 underscore symbol")]
        [TestCase("__", "__", TestName = "2 underscore symbols")]
        [TestCase("___", "___", TestName = "3 underscore symbols")]
        [TestCase("____", "____", TestName = "4 underscore symbols")]
        [TestCase("_____", "_____", TestName = "5 underscore symbols")]
        public void Render_HandlesTagsCorrectly(string text, string expectedResult)
        {
            Md.Render(text).Should().BeEquivalentTo(expectedResult);
        }
        
        [TestCase(1000, 100, TestName = "4000 tags in 100 milliseconds")]
        [TestCase(10000, 1000, TestName = "40000 tags in 1000 milliseconds")]
        public void Render_HandlesTagsFastly(int iterationsNumber, int timeInMilliseconds)
        {
            var text = "__x_*+a+*_x__\n";
            var fullText = new StringBuilder();
            for (var i = 0; i < iterationsNumber; i++)
            {
                fullText.Append(text);
            }

            Action act = () => Md.Render(fullText.ToString());
            act.ExecutionTime().Should().BeLessThan(timeInMilliseconds.Milliseconds());
        }
    }
}