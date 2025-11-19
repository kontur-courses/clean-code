using Markdown.Entities.Builders;
using NUnit.Framework;
using Markdown.Entities.Converters;
using Markdown.Entities.Parsers;

namespace Markdown.MarkdownTests.ConverterTests
{
    [TestFixture]
    public class ConverterTests
    {
        [TestFixture]
        public class BasicConverterTests
        {
            private Converter converter;
            [SetUp]
            public void Setup()
            {
                converter = new Converter(new HtmlBuilder(), new MarkdownTokenizer());
            }

            [Test]
            public void Convert_ThrowsNullReferenceException_WithNullBuilder()
            {
                var converter = new Converter(null, new MarkdownTokenizer());

                var exception = Assert.Throws<NullReferenceException>(() => converter.Convert("test"));
                Assert.That(exception.Message, Is.EqualTo("Builder can't be null"));
            }

            [Test]
            public void Convert_ThrowsNullReferenceException_WithNullTokenizer()
            {
                var converter = new Converter(new HtmlBuilder(), null);

                var exception = Assert.Throws<NullReferenceException>(() => converter.Convert("test"));
                Assert.That(exception.Message, Is.EqualTo("Tokenizer can't be null"));
            }

            [Test]
            public void Convert_ReturnsEmptyString_WithEmptyInput()
            {
                var result = converter.Convert("");

                Assert.That(result, Is.EqualTo(""));
            }

            [Test]
            public void Convert_ReturnsSimpleText_WithPlainText()
            {
                var result = converter.Convert("Hello World");

                Assert.That(result, Is.EqualTo("<p>Hello World</p>"));
            }

            [Test]
            public void Convert_ReturnsBoldText_WithBoldMarkers()
            {
                var result = converter.Convert("__bold text__");

                Assert.That(result, Is.EqualTo("<p><strong>bold text</strong></p>"));
            }

            [Test]
            public void Convert_ReturnsItalicText_WithItalicMarkers()
            {
                var result = converter.Convert("_italic text_");

                Assert.That(result, Is.EqualTo("<p><em>italic text</em></p>"));
            }

            [Test]
            public void Convert_ReturnsHeader_WithHeaderMarker()
            {
                var result = converter.Convert("# Header Text");

                Assert.That(result, Is.EqualTo("<h1>Header Text</h1>"));
            }

            [Test]
            public void Convert_ReturnsComplexHtml_WithMixedMarkup()
            {
                var result = converter.Convert("# Header with __bold__ and _italic_");

                Assert.That(result, Is.EqualTo("<h1>Header with <strong>bold</strong> and <em>italic</em></h1>"));
            }

            [Test]
            public void Convert_HandlesMultipleLines_WithLineBreaks()
            {
                var result = converter.Convert("First line\n\nSecond line");

                Assert.That(result, Is.EqualTo("<p>First line</p><p>Second line</p>"));
            }

            [Test]
            public void Convert_ReturnsComplexHtml_WithNestedMarkup()
            {
                var result = converter.Convert("# Header __with _bold_ and italic__");

                Assert.That(result, Is.EqualTo("<h1>Header <strong>with <em>bold</em> and italic</strong></h1>"));
            }
        }
        

        [TestFixture]
        public class NumbersAndUnderscoresTests
        {
            private Converter converter;

            [SetUp]
            public void Setup()
            {
                converter = new Converter(new HtmlBuilder(), new MarkdownTokenizer());
            }

            [Test]
            public void Convert_ShouldNotCreateEmphasis_WithUnderscoresAmongNumbers()
            {
                var markdown = "Цифры_12_3 не должны выделяться";

                var result = converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>Цифры_12_3 не должны выделяться</p>"));
            }

            [Test]
            public void Convert_ShouldNotCreateEmphasis_WithBoldAmongNumbers()
            {
                var markdown = "Цифры__12__3 не должны выделяться";

                var result = converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>Цифры__12__3 не должны выделяться</p>"));
            }

            [Test]
            public void Convert_ShouldNotCreateEmphasis_WithUnderscoresInMixedAlphanumeric()
            {
                var markdown = "Текст123_45_678 не выделяется";

                var result = converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>Текст123_45_678 не выделяется</p>"));
            }

            [Test]
            public void Convert_ShouldWork_WithNumbersOutsideMarkers()
            {
                var markdown = "123 _выделенный_ текст 456";

                var result = converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>123 <em>выделенный</em> текст 456</p>"));
            }
        }

        [TestFixture]
        public class CrossWordMarkersTests
        {
            private Converter _converter;

            [SetUp]
            public void Setup()
            {
                _converter = new Converter(new HtmlBuilder(), new MarkdownTokenizer());
            }


            [Test]
            public void Convert_ShouldNotCreateEmphasis_WithItalicMarkersSpanningMultipleWords()
            {
                var markdown = "раз_ные слова_ не должны выделяться курсивом";

                var result = _converter.Convert(markdown);

                // Курсив не должен работать через разные слова
                Assert.That(result, Is.EqualTo("<p>раз_ные слова_ не должны выделяться курсивом</p>"));
            }
        }

        [TestFixture]
        public class WordPartMarkersTests
        {
            private Converter _converter;

            [SetUp]
            public void Setup()
            {
                _converter = new Converter(new HtmlBuilder(), new MarkdownTokenizer());
            }

            [Test]
            public void Convert_ShouldCreateEmphasis_WithItalicAtWordBeginning()
            {
                var markdown = "В _нач_але слова";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>В <em>нач</em>але слова</p>"));
            }

            [Test]
            public void Convert_ShouldCreateEmphasis_WithItalicInWordMiddle()
            {
                var markdown = "В сер_еди_не слова";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>В сер<em>еди</em>не слова</p>"));
            }

            [Test]
            public void Convert_ShouldCreateEmphasis_WithItalicAtWordEnd()
            {
                var markdown = "В кон_це_ слова";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>В кон<em>це</em> слова</p>"));
            }

            [Test]
            public void Convert_ShouldCreateBold_WithBoldAtWordBeginning()
            {
                var markdown = "В __нач__але слова";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>В <strong>нач</strong>але слова</p>"));
            }

            [Test]
            public void Convert_ShouldCreateBold_WithBoldInWordMiddle()
            {
                var markdown = "В сер__еди__не слова";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>В сер<strong>еди</strong>не слова</p>"));
            }

            [Test]
            public void Convert_ShouldCreateBold_WithBoldAtWordEnd()
            {
                var markdown = "В кон__це__ слова";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>В кон<strong>це</strong> слова</p>"));
            }
        }

        [TestFixture]
        public class EscapedMarkersTests
        {
            private Converter _converter;

            [SetUp]
            public void Setup()
            {
                _converter = new Converter(new HtmlBuilder(), new MarkdownTokenizer());
            }

            [Test]
            public void Convert_ShouldNotCreateEmphasis_WithEscapedItalicMarker()
            {
                var markdown = "\\_Вот это\\_, не должно выделиться";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>_Вот это_, не должно выделиться</p>"));
            }

            [Test]
            public void Convert_ShouldNotCreateBold_WithEscapedBoldMarker()
            {
                var markdown = "\\_\\_это не жирный\\_\\_ текст";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>__это не жирный__ текст</p>"));
            }

            [Test]
            public void Convert_ShouldCreateEmphasis_WithEscapedEscapeBeforeItalic()
            {
                var markdown = "\\\\_вот это будет выделено_";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>\\<em>вот это будет выделено</em></p>"));
            }

            [Test]
            public void Convert_ShouldKeepEscapeChars_WhenNotEscapingMarkers()
            {
                var markdown = "Сим\\волы экранирования\\ должны остаться";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>Сим\\волы экранирования\\ должны остаться</p>"));
            }

            [Test]
            public void Convert_ShouldHandleMixedEscapedAndNormalMarkers()
            {
                var markdown = "\\_не выделяется_ а _выделяется_";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>_не выделяется_ а <em>выделяется</em></p>"));
            }
        }

        [TestFixture]
        public class UnpairedMarkersTests
        {
            private Converter _converter;

            [SetUp]
            public void Setup()
            {
                _converter = new Converter(new HtmlBuilder(), new MarkdownTokenizer());
            }

            [Test]
            public void Convert_ShouldNotCreateEmphasis_WithSingleItalicMarker()
            {
                var markdown = "Текст с _одиночным маркером";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>Текст с _одиночным маркером</p>"));
            }

            [Test]
            public void Convert_ShouldNotCreateBold_WithSingleBoldMarker()
            {
                var markdown = "Текст с __одиночным маркером";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>Текст с __одиночным маркером</p>"));
            }

            [Test]
            public void Convert_ShouldNotCreateEmphasis_WithMismatchedMarkers()
            {
                var markdown = "__Непарные_ символы";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>__Непарные_ символы</p>"));
            }

            [Test]
            public void Convert_ShouldNotCreateEmphasis_WithIntersectingMarkers()
            {
                var markdown = "__пересечение _двойных__ и одинарных_";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>__пересечение _двойных__ и одинарных_</p>"));
            }

            [Test]
            public void Convert_ShouldNotCreateEmphasis_WithEmptyMarkers()
            {
                var markdown = "Пустые ____ подчерки";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p>Пустые ____ подчерки</p>"));
            }

            [Test]
            public void Convert_ShouldHandleValidAndInvalidMarkersInSameText()
            {
                var markdown = "_валидный_ и _невалидный текст";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p><em>валидный</em> и _невалидный текст</p>"));
            }

            [Test]
            public void Convert_ShouldHandleBoldWithUnclosedItalicInside()
            {
                var markdown = "__жирный с _незакрытым курсивом__";

                var result = _converter.Convert(markdown);

                // В этом случае у нас пересечение маркеров - во всей строке обрабатываем как подчеркивания
                Assert.That(result, Is.EqualTo("<p>__жирный с _незакрытым курсивом__</p>"));
            }
        }

        [TestFixture]
        public class ComplexCombinationTests
        {
            private Converter _converter;

            [SetUp]
            public void Setup()
            {
                _converter = new Converter(new HtmlBuilder(), new MarkdownTokenizer());
            }

            [Test]
            public void Convert_ShouldHandleComplexRealWorldExample()
            {
                var markdown = "# Заголовок\n\nТекст с _курсивом_, __жирным__ и со_чета_ниями.\n\nЦифры_123_456 не выделяются, а _части_ слов - выделяются.";

                var result = _converter.Convert(markdown);

                Assert.That(result, Contains.Substring("<h1>Заголовок</h1>"));
                Assert.That(result, Contains.Substring("<em>курсивом</em>"));
                Assert.That(result, Contains.Substring("<strong>жирным</strong>"));
                Assert.That(result, Contains.Substring("со<em>чета</em>ниями"));
                Assert.That(result, Contains.Substring("Цифры_123_456"));
                Assert.That(result, Contains.Substring("<em>части</em>"));
            }

            [Test]
            public void Convert_ShouldHandleMultipleEdgeCasesInOneDocument()
            {
                var markdown = "# Тест\n\n_корректный_ и _не корректный между словами\\_экранированный_ и обычный текст.\n\n__жирный__ и __незакрытый жирный";

                var result = _converter.Convert(markdown);

                Assert.That(result, Contains.Substring("<em>корректный</em>"));
                Assert.That(result, Contains.Substring("<em>не корректный между словами"));
                Assert.That(result, Contains.Substring("_экранированный</em>"));
                Assert.That(result, Contains.Substring("<strong>жирный</strong>"));
                Assert.That(result, Contains.Substring("__незакрытый жирный"));
            }
        }

        [TestFixture]
        public class LinkTests
        {
            private Converter _converter;

            [SetUp]
            public void Setup()
            {
                _converter = new Converter(new HtmlBuilder(), new MarkdownTokenizer());
            }

            [Test]
            public void Convert_ReturnsAnchorTag_WithSimpleLink()
            {
                var markdown = "[пример ссылки](https://example.com)";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<p><a href=\"https://example.com\">пример ссылки</a></p>"));
            }

            [Test]
            public void Convert_ReturnsHeaderWithAnchor_WithLinkInHeader()
            {
                var markdown = "# Заголовок с [ссылкой](https://header.com)";

                var result = _converter.Convert(markdown);

                Assert.That(result, Is.EqualTo("<h1>Заголовок с <a href=\"https://header.com\">ссылкой</a></h1>"));
            }
        }
    }
}
