using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownShould
{
    [TestFixture]
    public class Md_Should
    {
        private Md parser;

        [SetUp]
        public void SetUp()
        {
            parser = new Md();
        }

        [TestCase(), Timeout(2000)]
        public void PotentiallyLongRunningTest()
        {
            // TODO
        }

        [TestCase("_some __bike_ is__", ExpectedResult = "<p><em>some <em><em>bike</em> is</em></em></p>")]
        [TestCase("_(__foo__)_", ExpectedResult = "<p><em>(<strong>foo</strong>)</em></p>")]
        [TestCase("*foo**bar**baz*", ExpectedResult = "<p><em>foo<strong>bar</strong>baz</em></p>")]
        [TestCase("**Gomphocarpus (*Gomphocarpus physocarpus*, syn.\r\n*Asclepias physocarpa*)**", 
            ExpectedResult = "<p><strong>Gomphocarpus (<em>Gomphocarpus physocarpus</em>, syn.\r\n<em>Asclepias physocarpa</em>)</strong></p>")]
        public string ComplexTest(string input)
        {
            return parser.Render(input);
        }
    }
}

