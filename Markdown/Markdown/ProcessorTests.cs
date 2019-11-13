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

        [TestCase("this is one _underscore", "this is one _underscore", TestName = "singe underscore doesn't count as tag")]
        [TestCase("some nice tag _pair_", "some nice tag <em>pair</em>", TestName = "underscored word counts as emphasis")]
        [TestCase("_choose _wisely_", "_choose <em>wisely</em>", TestName = "pair underscores after non-pair underscore")]
        [TestCase("_choose_ wisely_", "<em>choose</em> wisely_", TestName = "non-pair underscore after pair underscore")]
        [TestCase("_don't _look_ inside!_", "<em>don't <em>look</em> inside!</em>", TestName = "underscores inside underscores")]
        [TestCase("watch _ carefuly_", "watch _ carefuly_", TestName = "underscore between spaces can't be opening")]
        [TestCase("_watch _ carefuly", "_watch _ carefuly", TestName = "underscore between spaces can't be closing")]
        [TestCase("super_pro_ programmer", "super_pro_ programmer", TestName = "underscore between non-spases can't be opening")]
        [TestCase("super _pro_programmer", "super _pro_programmer", TestName = "underscore between non-spases can't be closing")]

        [TestCase(@"\_hey, ma_","_hey, ma_", TestName = "escaping opening underscore doesn't create pair tags")]
        [TestCase(@"_hey, ma\_", "_hey, ma_", TestName = "escaping closing underscore doesn't create pair tags")]
        [TestCase(@"_\_what a story_", "<em>_what a story</em>", TestName = "escaping underscore inside pair of underscores")]
        public void Render_ReturnsCorrectString_When(string input, string expected)
        {
            processor.Render(input).Should().Be(expected);
        }
    }
}
