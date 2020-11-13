using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Markdown
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
        public void Test1(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("_hello_", "<em>hello<\\em>")]
        [TestCase("_Hello_ _world_", "<em>Hello<\\em> <em>world<\\em>")]
        [TestCase("_Hello world_", "<em>Hello world<\\em>")]
        [TestCase("_Italic_ NotItalic", "<em>Italic<\\em> NotItalic")]
        [TestCase("_Italic_ NotItalic _Italic_", "<em>Italic<\\em> NotItalic <em>Italic<\\em>")]
        [TestCase("nn_III_nn", "nn<em>III<\\em>nn")]
        [TestCase("nn_III_", "nn<em>III<\\em>")]
        [TestCase("_III_nn", "<em>III<\\em>nn")]
        public void Test_Italics(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("__hello__", "<strong>hello<\\strong>")]
        [TestCase("__Hello__ __world__", "<strong>Hello<\\strong> <strong>world<\\strong>")]
        [TestCase("__Hello world__", "<strong>Hello world<\\strong>")]
        [TestCase("__Strong__ NotStrong", "<strong>Strong<\\strong> NotStrong")]
        [TestCase("__Strong__ NotStrong __Strong__", "<strong>Strong<\\strong> NotStrong <strong>Strong<\\strong>")]
        [TestCase("nn__SSS__nn", "nn<strong>SSS<\\strong>nn")]
        [TestCase("nn__SSS__", "nn<strong>SSS<\\strong>")]
        [TestCase("__SSS__nn", "<strong>SSS<\\strong>nn")]
        public void Test_Strong(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("__hello")]
        [TestCase("_hello")]
        [TestCase("hello__")]
        [TestCase("hello_")]
        [TestCase("hel__lo")]
        [TestCase("hel_lo")]
        [TestCase("hel__12__lo")]
        [TestCase("hel_12_lo")]
        [TestCase("__Strong _Intersection__ Italic_")]
        [TestCase("__")]
        [TestCase("____")]
        [TestCase("hel__lo wo__rld")]
        [TestCase("hel_lo wo_rld")]
        [TestCase("__hello \n world__")]
        [TestCase("_hello \n world_")]
        [TestCase("__hello_ \n __world_")]
        [TestCase("__hello __")]
        [TestCase("__ hello__")]
        [TestCase("_hello _")]
        [TestCase("_ hello_")]
        public void Render_ReturnsInputValue_OnIncorrectUsingTags(string input)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("__Hello _my_ world__", "<strong>Hello <em>my<\\em> world<\\strong>")]
        [TestCase("_Hello __my__ world_", "_Hello __my__ world_")]
        [TestCase("___Hello my_ world__", "<strong><em>Hello my<\\em> world<\\strong>")]
        [TestCase("__Hello _my world___", "<strong>Hello <em>my world<\\em><\\strong>")]
        [TestCase("___Hello my world___", "<strong><em>Hello my world<\\em><\\strong>")]
        [TestCase("___Hello my__ world_", "___Hello my__ world_")]
        public void Test2(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }

        [TestCase("#Hello", "<h1>Hello<\\h1>")]
        [TestCase("#_Hello_", "<h1><em>Hello<\\em><\\h1>")]
        [TestCase("#__Hello__", "<h1><strong>Hello<\\strong><\\h1>")]
        [TestCase("#__Hello _my_ world__", "<h1><strong>Hello <em>my<\\em> world<\\strong><\\h1>")]
        [TestCase("#Hello\n world", "<h1>Hello<\\h1> world")]
        [TestCase("_#Hello_", "_#Hello_")]
        [TestCase("__#Hello__", "__#Hello__")]
        public void Test3(string input, string expected)
        {
            var result = md.Render(input);

            Assert.AreEqual(input, result);
        }
    }
}
