using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkDownRendererTests
    {
        private MarkDownRenderer renderer = new MarkDownRenderer();

        private static (string mdText, string expectedHtml)[] mdTextWithOnlyHeader =
        {
            ("#простой случай"
                , "<h1>простой случай</h1>"),

            (@"\#c отражением",
                "#c отражением"),

            ("вну#три"
                , "вну#три"),

            ("в середине #предложения"
                , "в середине #предложения"),

            ("#в разных\n#абзацах",
                "<h1>в разных</h1>\n<h1>абзацах</h1>")
        };

        private static (string mdText, string expectedHtml)[] mdTextWithOnlyBoldTag =
        {
            ("__простой случай__"
                , "<strong>простой случай</strong>"),

            (@"\__c отражением__"
                , "__c отражением__"),
            
            ("____ пустые подчеркивания"
                ,"____ пустые подчеркивания"),

            ("вну__три__ слова"
                , "вну<strong>три</strong> слова"),

            ("не парный __тег"
                , "не парный __тег"),

            ("__ пробел после открывающего подчеркивания__",
                "__ пробел после открывающего подчеркивания__"),

            ("с цифрой __34лл_",
                "с цифрой __34лл_"),

            ("вну__три слова__"
                , "вну__три слова__")
        };

        private static (string mdText, string expectedHtml)[] mdTextWithBoldAndItalicTags =
        {
            ("__простой случай__"
                , "<strong>простой случай</strong>"),

            ("вну__три__ слова"
                , "вну<strong>три</strong> слова"),

            ("не парный __тег"
                , "не парный __тег"),

            ("__ пробел после открывающего подчеркивания__",
                "__ пробел после открывающего подчеркивания__"),

            ("с цифрой __34лл_",
                "с цифрой __34лл_"),

            ("__Пересечение _тегов__ нельзя_",
                "__Пересечение _тегов__ нельзя_"),

            ("__италик _внутри_ болд__",
                "<strong>италик <em>внутри</em> болд</strong>"),

            ("_болд __внутри__ италик_",
                "<em>болд __внутри__ италик</em>"),

            ("вну__три слова__"
                , "вну__три слова__")
        };

        private static (string mdText, string expectedHtml)[] mdTextWithOnlyItalicTag =
        {
            ("_простой случай_"
                , "<em>простой случай</em>"),
            
            (@"\_с отражением_"
                ,"_с отражением_"),

            ("вну_три_ слова"
                , "вну<em>три</em> слова"),

            ("не парный _тег"
                , "не парный _тег"),

            ("_ пробел после открывающего подчеркивания_",
                "_ пробел после открывающего подчеркивания_"),

            ("с цифрой _34лл_",
                "с цифрой _34лл_"),


            ("вну_три слова_"
                , "вну_три слова_")
        };

        private static (string mdText, string expectedHtml)[] mdTextWithDifferentCases =
        {
            ("#Заголовок __с _разными_ символами__",
                "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>"),
            
            (@"\\_должна_ отражать себя"
                ,@"\<em>должна</em> отражать себя"),
            
            (@"не должн\а экранираовать что-то иначе, не исчезает",
                @"не должн\а экранираовать что-то иначе, не исчезает")
        };

        [TestCaseSource(nameof(mdTextWithOnlyHeader))]
        public void Render_onlyHeader_shouldBeExpected((string mdText, string expectedHtml) pair)
        {
            var actual = renderer.Render(pair.mdText);

            actual.Should().Be(pair.expectedHtml);
        }

        [TestCaseSource(nameof(mdTextWithOnlyBoldTag))]
        public void Render_onlyBoldTag_shouldBeExpected((string mdText, string expectedHtml) pair)
        {
            var actual = renderer.Render(pair.mdText);

            actual.Should().Be(pair.expectedHtml);
        }

        [TestCaseSource(nameof(mdTextWithOnlyItalicTag))]
        public void Render_onlyItalicTag_shouldBeExpected((string mdText, string expectedHtml) pair)
        {
            var actual = renderer.Render(pair.mdText);

            actual.Should().Be(pair.expectedHtml);
        }

        [TestCaseSource(nameof(mdTextWithBoldAndItalicTags))]
        public void Render_withBoldAndItalicTags_shouldBeExpected((string mdText, string expectedHtml) pair)
        {
            var actual = renderer.Render(pair.mdText);

            actual.Should().Be(pair.expectedHtml);
        }

        [TestCaseSource(nameof(mdTextWithDifferentCases))]
        public void Render_withDifferentTags_shouldBeExpected((string mdText, string expectedHtml) pair)
        {
            var mdText = "#Заголовок __с _разными_ символами__";
            var expected = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>";

            var actual = renderer.Render(mdText);

            actual.Should().Be(expected);
        }
    }
}