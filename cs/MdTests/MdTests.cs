using Markdown;
using FluentAssertions;

namespace MdTests
{
    public class MDTests
    {
        private readonly Md _md;

        public MDTests()
        {
            _md = new Md();
        }

        [Test]
        public void Parse_EmptyInput_ReturnsEmptyString()
        {
            _md.Parse("").Should().BeEmpty();
        }


        [Test]
        public void Parse_PlainText_ReturnsAsIs()
        {
            _md.Parse("Просто текст").Should().Be("Просто текст");
        }


        [Test]
        public void Parse_BoldText_ReturnsStrongTag()
        {
            _md.Parse("__жирный__").Should().Be("<strong>жирный</strong>");
        }

        [Test]
        public void Parse_ItalicText_ReturnsEmTag()
        {
            _md.Parse("_курсив_").Should().Be("<em>курсив</em>");
        }

        [Test]
        public void Parse_CombinedBoldAndItalic_ReturnsNestedTags()
        {
            _md.Parse("__жирный и _курсив_ и еще текст__").Should().Be("<strong>жирный и <em>курсив</em> и еще текст</strong>");
        }

        [Test]
        public void Parse_CombinedItalicAndBold_ReturnsNestedTags()
        {
            _md.Parse("_курсив и __жирный__ и еще текст_").Should().Be("<em>курсив и __жирный__ и еще текст</em>");
        }

        [Test]
        public void Parse_ItalicStartInsideWord_ReturnsAsIs()
        {
            _md.Parse("Текст с подч_еркиванием_ внутри слова").Should().Be("Текст с подч<em>еркиванием</em> внутри слова");
        }

        [Test]
        public void Parse_ItalicClosedInsedeWord_ReturnsAsIs()
        {
            _md.Parse("Текст с _подчерк_иванием внутри слова").Should().Be("Текст с <em>подчерк</em>иванием внутри слова");
        }

        [Test]
        public void Parse_ItalicStartAndClosedInsideWord_ReturnsAsIs()
        {
            _md.Parse("Текст с подч_еркива_нием внутри слова").Should().Be("Текст с подч<em>еркива</em>нием внутри слова");
        }

        [Test]
        public void Parse_StrongCloseInsedeItalic_ReturnsAsIs()
        {
            _md.Parse("__пересечения _двойных__ и одинарных_").Should().Be("__пересечения _двойных__ и одинарных_");
        }

        [Test]
        public void Parse_TripleUnderscore_ReturnsStrongTag()
        {
            _md.Parse("___текст___").Should().Be("<strong><em>текст</em></strong>");
        }

        [TestCase("# Заголовок", "<h1>Заголовок</h1>")]
        [TestCase("# Заголовок с __жирным__ текстом", "<h1>Заголовок с <strong>жирным</strong> текстом</h1>")]
        [TestCase("# Загаловк с _курсивом_ текстом", "<h1>Загаловк с <em>курсивом</em> текстом</h1>")]
        [TestCase("# Заголовок с __жирным__ и _курсивом_ текстом", "<h1>Заголовок с <strong>жирным</strong> и <em>курсивом</em> текстом</h1>")]
        public void Parse_Header_ReturnsH1Tag(string input, string expected)
        {
            _md.Parse(input).Should().Be(expected);
        }

        [Test]
        public void Parse_DoubleHeader_ReturnThisString()
        {
            _md.Parse("## Подзаголовок").Should().Be("## Подзаголовок");
        }

        [Test]
        public void Parse_UnmutchedUnderscores_ReturnsOnlyMatchingUnderscores()
        {
            _md.Parse("Текст с __непарными_ подчеркиваниями").Should().Be("Текст с __непарными_ подчеркиваниями");

        }

        [TestCase("#")]
        [TestCase("__")]
        [TestCase("____")]

        public void Parse_EmptyTags_ReturnsAsIs(string input)
        {
            _md.Parse(input).Should().Be(input);
        }

        [Test]
        public void Parse_UnderscoreInDigits_ReturnsAsIs()
        {
            _md.Parse("Текст с 1_23_456").Should().Be("Текст с 1_23_456");
        }
        [Test]
        public void Parse_StrongInDigits_ReturnsAsIs()
        {
            _md.Parse("Текст с 1__23__456").Should().Be("Текст с 1__23__456");
        }

        [Test]
        public void Parse_WordsWithDigits_ReturnsAsIs()
        {
            _md.Parse("цифрами_12_3").Should().Be("цифрами_12_3");
        }


        [Test]
        public void Parse_EscapedWithStrongTag_ReturnsAsIs()
        {
            _md.Parse("Текст с \\__экранированным__ подчеркиванием").Should().Be("Текст с __экранированным__ подчеркиванием");
        }

        [Test]
        public void Parse_EscapedClosedTag_ReturnsAsIs()
        {
            _md.Parse("Текст с _Подчеркиваниями\\_ подчеркиванием").Should().Be("Текст с _Подчеркиваниями_ подчеркиванием");
        }

        [Test]
        public void Parse_EscapedHeader_ReturnsAsIs()
        {
            _md.Parse("\\# Заголовок").Should().Be("# Заголовок");
        }

        [Test]
        public void Parse_EscapedInsideWord_ReturnsAsIs()
        {
            _md.Parse(@"Текст с эскейпом внутри слова: подчеркив\ание").Should().Be(@"Текст с эскейпом внутри слова: подчеркив\ание");
        }

        [Test]
        public void Parse_SimpleHeaderNextLine_ReturnsH1AndNextLine()
        {
            _md.Parse("# Крутой текст\nПродолжение текста").Should().Be("<h1>Крутой текст</h1>\nПродолжение текста");
        }

        [Test]
        public void Parse_ComplexMarkdown_ReturnsCorrectHtml()
        {
            _md.Parse("# Заголовок с __жирным__ и _курсивом_ текстом" +
                "\nПродолжение текста с __жирным__ и _курсивом_.").Should()
                .Be("<h1>Заголовок с <strong>жирным</strong> и <em>курсивом</em> текстом</h1>" +
                "\nПродолжение текста с <strong>жирным</strong> и <em>курсивом</em>.");
        }

        [Test]
        public void Parse_HtmlTagsInText_EscapesThem()
        {
            _md.Parse("Текст с <div>тегом</div>")
               .Should().Be("Текст с &lt;div&gt;тегом&lt;/div&gt;");
        }

        [Test]
        public void Parse_StrongInsideItalic_ReturnsAsIs()
        {
            _md.Parse("_текст __жирный_ текст__").Should().Be("_текст __жирный_ текст__");
        }

        [Test]
        public void Parse_UnderscoreInStrong_ReturnsCorrectHtml()
        {
            _md.Parse("__текст _ хвост__").Should().Be("<strong>текст _ хвост</strong>");
        }

        [Test]
        public void Parse_LonelyUnderscoreWithSpacesInsideItalic_StaysUnderscore()
        {
            _md.Parse("_текст _ ещё_").Should().Be("<em>текст _ ещё</em>");
        }

        [TestCase("_ текст_", "_ текст_")]
        [TestCase("_текст _", "_текст _")]
        [TestCase("_ текст _", "_ текст _")]
        [TestCase("__ текст__", "__ текст__")]
        [TestCase("__текст __", "__текст __")]
        [TestCase("__ текст __", "__ текст __")]
        [TestCase("эти_ подчерки_ не", "эти_ подчерки_ не")]
        [TestCase("текст _ текст_", "текст _ текст_")]
        public void Parse_SpacesNextToDelimiters_DoesNotEmphasize(string input, string expected)
        {
            _md.Parse(input).Should().Be(expected);
        }


        [Test]
        public void Parse_SingleListItem_ReturnsUlWithLi()
        {
            _md.Parse("- пункт").Should().Be("<ul><li>пункт</li></ul>");
        }

        [Test]
        public void Parse_ConsecutiveListItems_AreGroupedIntoOneList()
        {
            _md.Parse("- один\n- два").Should().Be("<ul><li>один</li><li>два</li></ul>");
        }

        [Test]
        public void Parse_ListItemWithFormatting_RendersInlineTags()
        {
            _md.Parse("- _курсив_ и __жирный__")
                .Should().Be("<ul><li><em>курсив</em> и <strong>жирный</strong></li></ul>");
        }

        [Test]
        public void Parse_ParagraphBeforeList_SeparatesBlocks()
        {
            _md.Parse("Текст\n- пункт").Should().Be("Текст\n<ul><li>пункт</li></ul>");
        }

        [Test]
        public void Parse_ParagraphAfterList_EndsList()
        {
            _md.Parse("- пункт\nТекст").Should().Be("<ul><li>пункт</li></ul>\nТекст");
        }

        [Test]
        public void Parse_ParagraphBetweenItems_SplitsIntoTwoLists()
        {
            _md.Parse("- a\nТекст\n- b")
                .Should().Be("<ul><li>a</li></ul>\nТекст\n<ul><li>b</li></ul>");
        }

        [Test]
        public void Parse_HeaderBetweenItems_SplitsIntoTwoLists()
        {
            _md.Parse("- a\n# Заголовок\n- b")
                .Should().Be("<ul><li>a</li></ul>\n<h1>Заголовок</h1>\n<ul><li>b</li></ul>");
        }

        [Test]
        public void Parse_LeadingSpacesBeforeMarker_AreIgnored()
        {
            _md.Parse("  - пункт").Should().Be("<ul><li>пункт</li></ul>");
        }

        [TestCase("-пункт", "-пункт")]
        [TestCase("-", "-")]
        [TestCase("- ", "-")]
        public void Parse_NotAListItem_ReturnsAsText(string input, string expected)
        {
            _md.Parse(input).Should().Be(expected);
        }

        [Test]
        public void Parse_EscapedMarker_IsNotListItem()
        {
            _md.Parse("\\- не пункт").Should().Be("- не пункт");
        }

        [Test]
        public void Parse_MarkerInsideItem_StaysText()
        {
            _md.Parse("- пункт - не вложенный")
                .Should().Be("<ul><li>пункт - не вложенный</li></ul>");
        }

        [Test]
        public void Parse_HeaderMarkerInsideItem_StaysText()
        {
            _md.Parse("- # не заголовок").Should().Be("<ul><li># не заголовок</li></ul>");
        }

        [Test]
        public void Parse_ListMarkerInsideHeader_StaysText()
        {
            _md.Parse("# - заголовок").Should().Be("<h1>- заголовок</h1>");
        }

        [Test]
        public void Parse_UnpairedUnderscoresInDifferentItems_DoNotPair()
        {
            _md.Parse("- _a\n- b_").Should().Be("<ul><li>_a</li><li>b_</li></ul>");
        }


    }
}