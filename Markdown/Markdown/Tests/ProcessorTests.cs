using System;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    internal class ProcessorTests
    {
        private Processor processor;

        [OneTimeSetUp]
        public void SetUp()
        {
            processor = new Processor(Syntax.InitializeDefaultSyntax(), new HtmlConverter());
        }

        [Test]
        public void Render_ThrowsArgumentException_IfInputIsNull()
        {
            Action act = () => processor.Render(null);
            act.Should().Throw<ArgumentException>();
        }

        [TestCase("", "", TestName = "Empty string")]
        [TestCase("It Was A Good Day!", "It Was A Good Day!", TestName = "Non-empty string without attributes")]
        public void Render_ReturnsCorrectString_WhenNoAttributes(string input, string expected)
        {
            processor.Render(input).Should().Be(expected);
        }

        [TestCase(@"Hello, \World", @"Hello, \World", TestName = "Escape character with non-special character is shown")]
        [TestCase(@"Sup, big boy\", @"Sup, big boy\", TestName = "Escape at the end of string")]
        [TestCase(@"Hi\! How are you\?", "Hi! How are you?", TestName = "Escape character with special character is not shown")]
        [TestCase(@"My nick is \\prokiller\\", @"My nick is \prokiller\", TestName = "Escape character is escaped")]
        [TestCase(@"\\\Foo\\\\Bar", @"\\Foo\\Bar", TestName = "Many escape characters in a row")]
        public void Render_ReturnsCorrectString_WithEscapeSymbol(string input, string expected)
        {
            processor.Render(input).Should().Be(expected);
        }

        [TestCase("this is one _underscore", "this is one _underscore", TestName = "Singe underscore doesn't count as tag")]
        [TestCase("some nice tag _pair_", "some nice tag <em>pair</em>", TestName = "Underscored word counts as emphasis")]
        [TestCase("_choose _wisely_", "_choose <em>wisely</em>", TestName = "Pair underscores after non-pair underscore")]
        [TestCase("_choose_ wisely_", "<em>choose</em> wisely_", TestName = "Non-pair underscore after pair underscore")]
        [TestCase("watch _ carefuly_", "watch _ carefuly_", TestName = "Underscore between spaces can't be opening")]
        [TestCase("_watch _ carefuly", "_watch _ carefuly", TestName = "Underscore between spaces can't be closing")]
        [TestCase("super_pro_ programmer", "super_pro_ programmer", TestName = "Underscore between non-spases can't be opening")]
        [TestCase("super _pro_programmer", "super _pro_programmer", TestName = "Underscore between non-spases can't be closing")]
        public void Render_ReturnsCorrectString_WithSingleUnderscores(string input, string expected)
        {
            processor.Render(input).Should().Be(expected);
        }

        [TestCase("this __song", "this __song", TestName = "Singe double underscore doesn't count as tag")]
        [TestCase("__boom__", "<strong>boom</strong>", TestName = "Double underscored word has strong tags")]
        [TestCase("__powerful tags__", "<strong>powerful tags</strong>", TestName = "Double underscored phrase has strong tags")]
        [TestCase("miss __ space__", "miss __ space__", TestName = "Double underscore between spaces can't be opening")]
        [TestCase("__miss __ space", "__miss __ space", TestName = "Double underscore between spaces can't be closing")]
        [TestCase("weak__text__", "weak__text__", TestName = "Double underscore between non-spases can't be opening")]
        [TestCase("__weak__text", "__weak__text", TestName = "Double underscore between non-spases can't be closing")]
        public void Render_ReturnsCorrectString_WithDoubleUnderscores(string input, string expected)
        {
            processor.Render(input).Should().Be(expected);
        }

        [TestCase("__mad banana_", "_<em>mad banana</em>", 
            TestName = "Double underscore and single underscore don't make pair")]
        [TestCase("____snake___", "_<strong><em>snake</em></strong>", 
            TestName = "Word with 4 and 3 underscores make 1 extra underscore, and strong and emphasis pair")]
        [TestCase("hey_______bye", "hey_______bye", 
            TestName = "Many underscores between words with no spaces make no tags")]
        [TestCase("_____ Call 911 _____ John", "_____ Call 911 _____ John", 
            TestName = "Many underscores between spaces make no tags")]
        [TestCase("_very __cute__ cat_", "<em>very <strong>cute</strong> cat</em>",
            TestName = "Double underscore inside single underscore")]
        [TestCase("__very _sad_ dog__", "<strong>very <em>sad</em> dog</strong>",
            TestName = "Single underscores inside double underscores")]
        [TestCase("___happy sunshine___", "<strong><em>happy sunshine</em></strong>",
            TestName = "Adjacent strong and emphasis tags are created in right order")]
        [TestCase("_very _nice fox__", "<em>very nice fox</em>",
            TestName = "No extra pair of emphasis tags inside pair of emphasis tags")]
        [TestCase("_you _should_ _eat_ healthy_", "<em>you should eat healthy</em>",
            TestName = "many nested emphasis pairs create only one pair of tags")]
        [TestCase("__very __evil__ wolf__", "<strong>very evil wolf</strong>",
            TestName = "No extra pair of strong tags inside pair of strong tags")]
        [TestCase("__you __should __eat__ junk____", "<strong>you should eat junk</strong>",
            TestName = "Many nested strong pairs create only one pair of tags")]
        [TestCase("___that ___is___ hard___", "<strong><em>that is hard</em></strong>",
            TestName = "Nested emphasis and strong tags inside strong and emphasis tags at the same time don't make tags")]
        public void Render_ReturnsCorrectString_WithManyUnderscores(string input, string expected)
        {
            processor.Render(input).Should().Be(expected);
        }

        [TestCase(@"\_hey, ma_", "_hey, ma_", TestName = "Escaping opening underscore doesn't create pair tags")]
        [TestCase(@"Sup, big boy\", @"Sup, big boy\", TestName = "Escape at the end of string")]
        [TestCase(@"_hey, ma\_", "_hey, ma_", TestName = "Escaping closing underscore doesn't create pair tags")]
        [TestCase(@"_\_what a story_", "<em>_what a story</em>", TestName = "Escaping underscore inside pair of underscores")]
        public void Render_ReturnsCorrectString_WithEscapedAttributes(string input, string expected)
        {
            processor.Render(input).Should().Be(expected);
        }

        [TestCase("[]()", "<a href=\"\"></a>", TestName = "Empty link header and description create empty tag")]
        [TestCase("[Google It](http://google.com)", "<a href=\"http://google.com\">Google It</a>", 
            TestName = "Circle brackets after round brackets create a link")]
        [TestCase("[_Yandex It_](http://yandex.ru)", "<a href=\"http://yandex.ru\"><em>Yandex It</em></a>", 
            TestName = "Emphasis text inside link header name")]
        [TestCase("[__Bing It__](http://bing.com)", "<a href=\"http://bing.com\"><strong>Bing It</strong></a>", 
            TestName = "Strong text inside link header name")]
        [TestCase("[[(Find It)]](http://mail.ru)", "<a href=\"http://mail.ru\">[(Find It)]</a>", 
            TestName = "Brackets inside link header don't make a tag")]
        [TestCase("_[Get Lucky_](https://www.wikipedia.org)", "_<a href=\"https://www.wikipedia.org\">Get Lucky_</a>", 
            TestName = "Pair tags are not valid when one of them is out of link header")]
        [TestCase("[Check One](_pavel)_", "<a href=\"_pavel\">Check One</a>_", 
            TestName = "Pair tags are not valid when one of them is out of link description")]
        [TestCase("[dog](_cat_)", "<a href=\"_cat_\">dog</a>", 
            TestName = "Tags are not created inside link description")]
        [TestCase("[bad] (space)", "[bad] (space)", 
            TestName = "Link tag is not created if symbol between header and description")]
        [TestCase("[one][two](http://two.com)", "[one]<a href=\"http://two.com\">two</a>", 
            TestName = "Two link headers in a row, second creates a link")]
        [TestCase("[hey [bye](empty)](http://google.com)", "[hey <a href=\"empty\">bye</a>](http://google.com)", 
            TestName = "Inside link tags create a link, external tags shown as tags")]
        [TestCase("[one](http://one.com)(http://two.com)", "<a href=\"http://one.com\">one</a>(http://two.com)", 
            TestName = "Link description after another doesn't create a link")]
        [TestCase("[_dog](cat_)", "<a href=\"cat_\">_dog</a>", 
            TestName = "Opening underscore in link header and closing underscore in link description don't create pair of tags")]
        [TestCase("[[Find It](http://mail.ru)]", "[<a href=\"http://mail.ru\">Find It</a>]",
            TestName = "Link inside header link brackets")]
        [TestCase(@"[Hello](World\!)", "<a href=\"World!\">Hello</a>", 
            TestName = "Special symbol is escaped inside link description")]
        [TestCase("[Google It](http://google.com) text [Google It](http://google.com)",
            "<a href=\"http://google.com\">Google It</a> text <a href=\"http://google.com\">Google It</a>",
            TestName = "Two links in text")]
        public void Render_ReturnsCorrectString_WithLinkAttributes(string input, string expected)
        {
            processor.Render(input).Should().Be(expected);
        }
    }
}