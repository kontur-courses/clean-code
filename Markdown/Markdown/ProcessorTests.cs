using System;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    [TestFixture]
    internal class ProcessorTests
    {
        private Processor processor;

        [OneTimeSetUp]
        public void SetUp()
        {
            processor = new Processor(Syntax.InitializeDefaultSyntax());
        }

        [Test]
        public void Render_ThrowsArgumentException_IfInputIsNull()
        {
            Action act = () => processor.Render(null);
            act.Should().Throw<ArgumentException>();
        }

        [TestCase("","", TestName = "Empty string")]
        [TestCase("It Was A Good Day!", "It Was A Good Day!", TestName = "Non-empty string without attributes")]
        [TestCase(@"Hello, \World", @"Hello, \World", TestName = "Escape character with non-special character is shown")]
        [TestCase(@"Hi\! How are you\?", "Hi! How are you?", TestName = "Escape character with special character is not shown")]
        [TestCase(@"My nick is \\prokiller\\", @"My nick is \prokiller\", TestName = "Escape character is escaped")]
        [TestCase(@"\\\Foo\\\\Bar", @"\\Foo\\Bar", TestName = "Many escape characters in a row")]

        [TestCase("this is one _underscore", "this is one _underscore", TestName = "Singe underscore doesn't count as tag")]
        [TestCase("some nice tag _pair_", "some nice tag <em>pair</em>", TestName = "Underscored word counts as emphasis")]
        [TestCase("_choose _wisely_", "_choose <em>wisely</em>", TestName = "Pair underscores after non-pair underscore")]
        [TestCase("_choose_ wisely_", "<em>choose</em> wisely_", TestName = "Non-pair underscore after pair underscore")]
        [TestCase("_don't _look_ inside!_", "<em>don't <em>look</em> inside!</em>", TestName = "Underscores inside underscores")]
        [TestCase("watch _ carefuly_", "watch _ carefuly_", TestName = "Underscore between spaces can't be opening")]
        [TestCase("_watch _ carefuly", "_watch _ carefuly", TestName = "Underscore between spaces can't be closing")]
        [TestCase("super_pro_ programmer", "super_pro_ programmer", TestName = "Underscore between non-spases can't be opening")]
        [TestCase("super _pro_programmer", "super _pro_programmer", TestName = "Underscore between non-spases can't be closing")]

        [TestCase(@"\_hey, ma_","_hey, ma_", TestName = "Escaping opening underscore doesn't create pair tags")]
        [TestCase(@"_hey, ma\_", "_hey, ma_", TestName = "Escaping closing underscore doesn't create pair tags")]
        [TestCase(@"_\_what a story_", "<em>_what a story</em>", TestName = "Escaping underscore inside pair of underscores")]

        [TestCase("this __song", "this __song", TestName = "Singe double underscore doesn't count as tag")]
        [TestCase("__boom__", "<strong>boom</strong>", TestName = "Double underscored word has strong tags")]
        [TestCase("__powerful tags__", "<strong>powerful tags</strong>", TestName = "Double underscored phrase has strong tags")]
        [TestCase("miss __ space__", "miss __ space__", TestName = "Double underscore between spaces can't be opening")]
        [TestCase("__miss __ space", "__miss __ space", TestName = "Double underscore between pases can't be closing")]
        [TestCase("weak__text__", "weak__text__", TestName = "Double underscore between non-spases can't be opening")]
        [TestCase("__weak__text", "__weak__text", TestName = "Double underscore between non-spases can't be closing")]
        [TestCase("__bow __wow__ bow__", "<strong>bow <strong>wow</strong> bow</strong>",
            TestName = "double underscores inside double underscores")]

        [TestCase("_very __cute__ cat_", "<em>very <strong>cute</strong> cat</em>",
            TestName = "Double underscore inside single underscore")]
        [TestCase("__very _sad_ dog__", "<strong>very <em>sad</em> dog</strong>",
            TestName = "Single underscores inside double underscores")]
        [TestCase("__very happy_ frog_", "<em><em>very happy</em> frog</em>", 
            TestName = "Underscores inside  underscores with an adjacent opening delimiters")]
        [TestCase("_very _nice fox__", "<em>very <em>nice fox</em></em>",
            TestName = "Underscores inside underscores with an adjacent closing delimiters")]
        [TestCase("__mad banana_", "_<em>mad banana</em>", TestName = "Double underscore and single underscore don't make pair")]
        public void Render_ReturnsCorrectString_When(string input, string expected)
        {
            processor.Render(input).Should().Be(expected);
        }
    }
}
