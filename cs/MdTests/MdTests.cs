using NUnit;
using Markdown;
using Xunit;
using FluentAssertions;
using Markdown.Parsing;

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

        [Test]
        public void Parse_Header_ReturnsH1Tag()
        {
            _md.Parse("# Заголовок").Should().Be("<h1>Заголовок</h1>");
        }

        [Test]
        public void Parse_HeaderWithBoldText_ReturnsH1WithStrongTag()
        {
            _md.Parse("# Заголовок с __жирным__ текстом").Should().Be("<h1>Заголовок с <strong>жирным</strong> текстом</h1>");
        }

        [Test]
        public void Parse_HeaderWithItalicText_ReturnsH1WithEmTag()
        {
            _md.Parse("# Заголовок с _курсивом_ текстом").Should().Be("<h1>Заголовок с <em>курсивом</em> текстом</h1>");
        }

        [Test]
        public void Parse_HeaderWithBoldAndItalicText_ReturnsH1WithNestedTags()
        {
            _md.Parse("# Заголовок с __жирным__ и _курсивом_ текстом").Should().Be("<h1>Заголовок с <strong>жирным</strong> и <em>курсивом</em> текстом</h1>");
        }


        [Test]
        public void Parse_DoubleHeader_ReturnThisString()
        {
            _md.Parse("## Подзаголовок").Should().Be("## Подзаголовок");
        }

        [Test]
        public void Parse_UnmutchedUnderscores_ReturnsOnlyMatchingUnderscores()
        {
            _md.Parse("Текст с __непарными_ подчеркиваниями").Should().Be("Текст с <em>_непарными</em> подчеркиваниями");

        }

        [Test]
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
            _md.Parse("Текст с 1__23_45__6").Should().Be("Текст с 1__23_45__6");
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


    }
}