using Markdown.Entities.Parsers;
using Markdown.Enums;
using NUnit.Framework;

namespace Markdown.MarkdownTests.TokenizersTests
{
    [TestFixture]
    public class MarkdownTokenizerTests
    {
        private MarkdownTokenizer _tokenizer;

        [SetUp]
        public void Setup()
        {
            _tokenizer = new MarkdownTokenizer();
        }

        [TestFixture]
        public class ItalicsOnlyTests
        {
            private MarkdownTokenizer _tokenizer;

            [SetUp]
            public void Setup()
            {
                _tokenizer = new MarkdownTokenizer();
            }

            [Test]
            public void TokenizeLine_ReturnsEmphasisTokens_WithItalicSurroundedByUnderscores()
            {
                var line = "Текст, _окруженный с двух сторон_ одинарными символами подчерка";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(5));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Текст, "));
                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.ItalicsStart));
                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[2].Value, Is.EqualTo("окруженный с двух сторон"));
                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.ItalicsEnd));
                Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[4].Value, Is.EqualTo(" одинарными символами подчерка"));
            }

            [Test]
            public void TokenizeLine_ShouldCreateEmphasisTokens_WithItalicInWordParts()
            {
                var line = "Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(12));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Однако выделять часть слова они могут: и в "));

                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.ItalicsStart));

                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[2].Value, Is.EqualTo("нач"));

                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.ItalicsEnd));

                Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[4].Value, Is.EqualTo("але, и в сер"));

                Assert.That(tokens[5].Type, Is.EqualTo(TokenType.ItalicsStart));

                Assert.That(tokens[6].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[6].Value, Is.EqualTo("еди"));

                Assert.That(tokens[7].Type, Is.EqualTo(TokenType.ItalicsEnd));

                Assert.That(tokens[8].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[8].Value, Is.EqualTo("не, и в кон"));

                Assert.That(tokens[9].Type, Is.EqualTo(TokenType.ItalicsStart));

                Assert.That(tokens[10].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[10].Value, Is.EqualTo("це."));

                Assert.That(tokens[11].Type, Is.EqualTo(TokenType.ItalicsEnd));

            }

            [Test]
            public void TokenizeLine_ShouldNotWork_WithItalicAcrossDifferentWords()
            {
                var line = "В то же время выделение в ра_зных сл_овах не работает.";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("В то же время выделение в ра_зных сл_овах не работает."));
            }

            [Test]
            public void TokenizeLine_ShouldNotEndEmphasis_WithUnderscorePrecededBySpace()
            {
                var line = "За подчерками, начинающими выделение, должен _ следовать_ непробельный символ.";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("За подчерками, начинающими выделение, должен _ следовать_ непробельный символ."));
            }

            [Test]
            public void TokenizeLine_ShouldNotStartEmphasis_WithClosingUnderscoreAfterSpace()
            {
                var line = "Иначе эти _подчерки _не считаются выделением";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Иначе эти _подчерки _не считаются выделением"));
            }
        }

        [TestFixture]
        public class BoldOnlyTests
        {
            private MarkdownTokenizer _tokenizer;

            [SetUp]
            public void Setup()
            {
                _tokenizer = new MarkdownTokenizer();
            }

            [Test]
            public void TokenizeLine_ReturnsStrongTokens_WithBoldSurroundedByDoubleUnderscores()
            {
                var line = "__Выделенный двумя символами текст__ должен становиться полужирным";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(4));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.BoldStart));
                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[1].Value, Is.EqualTo("Выделенный двумя символами текст"));
                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.BoldEnd));
                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[3].Value, Is.EqualTo(" должен становиться полужирным"));
            }

            [Test]
            public void TokenizeLine_ReturnsStrongTokens_WithWordSurroundedByDoubleUnderscores()
            {
                var line = "Выделенный двумя символами __текст__ должен становиться полужирным";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(5));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Выделенный двумя символами "));
                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.BoldStart));
                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[2].Value, Is.EqualTo("текст"));
                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.BoldEnd));
                Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[4].Value, Is.EqualTo(" должен становиться полужирным"));
            }


            [Test]
            public void TokenizeLine_ShouldNotStartEmphasis_WithUnderscoreFollowedBySpace()
            {
                var line = "Иначе эти__ подчерки__ не считаются выделением";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Иначе эти__ подчерки__ не считаются выделением"));
            }

            [Test]
            public void TokenizeLine_ShouldNotStartEmphasis_WithClosingUnderscoreAfterSpace()
            {
                var line = "Иначе эти __подчерки __не считаются выделением";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Иначе эти __подчерки __не считаются выделением"));
            }
        }

        [TestFixture]
        public class TagInteractionTests
        {
            private MarkdownTokenizer _tokenizer;

            [SetUp]
            public void Setup()
            {
                _tokenizer = new MarkdownTokenizer();
            }

            [Test]
            public void TokenizeLine_ShouldCreateNestedTokens_WithBoldContainingItalic()
            {
                var line = "Внутри __двойного выделения _одинарное_ тоже__ работает.";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(9));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Внутри "));
                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.BoldStart));
                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[2].Value, Is.EqualTo("двойного выделения "));
                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.ItalicsStart));
                Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[4].Value, Is.EqualTo("одинарное"));
                Assert.That(tokens[5].Type, Is.EqualTo(TokenType.ItalicsEnd));
                Assert.That(tokens[6].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[6].Value, Is.EqualTo(" тоже"));
                Assert.That(tokens[7].Type, Is.EqualTo(TokenType.BoldEnd));
                Assert.That(tokens[8].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[8].Value, Is.EqualTo(" работает."));
            }

            [Test]
            public void TokenizeLine_ShouldNotCreateBoldTokens_WithItalicContainingBold()
            {
                var line = "Но не наоборот — внутри _одинарного __двойное__ не_ работает.";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(5));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Но не наоборот — внутри "));
                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.ItalicsStart));
                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[2].Value, Is.EqualTo("одинарного __двойное__ не"));
                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.ItalicsEnd));
                Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[4].Value, Is.EqualTo(" работает."));
            }

            [Test]
            public void TokenizeLine_ShouldNotCreateEmphasis_WithUnderscoresInNumbers()
            {
                var line = "Подчерки внутри текста c цифрами_12_3 не считаются выделением";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Подчерки внутри текста c цифрами_12_3 не считаются выделением"));
            }

            [Test]
            public void TokenizeLine_ShouldNotCreateEmphasis_WithUnpairedUnderscores()
            {
                var line = "__Непарные_ символы в рамках одного абзаца не считаются выделением.";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("__Непарные_ символы в рамках одного абзаца не считаются выделением."));
            }

            [Test]
            public void TokenizeLine_ShouldNotCreateEmphasis_WithItalicsIntersectingUnderscores()
            {
                var line = "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением."));
            }

            [Test]
            public void TokenizeLine_ShouldNotCreateEmphasis_WithBoldIntersectingUnderscores()
            {
                var line = "В случае _пересечения __двойных_ и одинарных__ подчерков ни один из них не считается выделением.";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("В случае _пересечения __двойных_ и одинарных__ подчерков ни один из них не считается выделением."));
            }

            [Test]
            public void TokenizeLine_ShouldNotCreateEmphasis_WithEmptyUnderscores()
            {
                var line = "Если внутри подчерков пустая строка ____, то они остаются символами подчерка.";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Если внутри подчерков пустая строка ____, то они остаются символами подчерка."));
            }
        }

        [TestFixture]
        public class HeaderTests
        {
            private MarkdownTokenizer _tokenizer;

            [SetUp]
            public void Setup()
            {
                _tokenizer = new MarkdownTokenizer();
            }

            [Test]
            public void TokenizeLine_ReturnsHeaderToken_WithHeader()
            {
                var line = "# Заголовок";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(2));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Header));
                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[1].Value, Is.EqualTo("Заголовок"));
            }

            [Test]
            public void TokenizeLine_ReturnsCombinedTokens_WithHeaderContainingFormatting()
            {
                var line = "# Заголовок __с _разными_ символами__";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(9));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Header));
                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[1].Value, Is.EqualTo("Заголовок "));
                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.BoldStart));
                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[3].Value, Is.EqualTo("с "));
                Assert.That(tokens[4].Type, Is.EqualTo(TokenType.ItalicsStart));
                Assert.That(tokens[5].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[5].Value, Is.EqualTo("разными"));
                Assert.That(tokens[6].Type, Is.EqualTo(TokenType.ItalicsEnd));
                Assert.That(tokens[7].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[7].Value, Is.EqualTo(" символами"));
                Assert.That(tokens[8].Type, Is.EqualTo(TokenType.BoldEnd));
            }

            [Test]
            public void TokenizeLine_ShouldNotBeHeader_WithHeaderWithoutSpace()
            {
                var line = "#Заголовок без пробела";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("#Заголовок без пробела"));
            }

            [Test]
            public void TokenizeLine_ShouldNotBeHeader_WithHeaderInMiddleOfLine()
            {
                var line = "Текст # Заголовок в середине";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Текст # Заголовок в середине"));
            }

            [Test]
            public void Tokenize_ReturnsCorrectTokens_WithCompleteExampleFromSpecification()
            {
                var text = "# Заголовок __с _разными_ символами__";

                var tokens = _tokenizer.TokenizeLine(text);

                Assert.That(tokens, Has.Count.EqualTo(9));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Header));
                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[1].Value, Is.EqualTo("Заголовок "));
                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.BoldStart));
                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[3].Value, Is.EqualTo("с "));
                Assert.That(tokens[4].Type, Is.EqualTo(TokenType.ItalicsStart));
                Assert.That(tokens[5].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[5].Value, Is.EqualTo("разными"));
                Assert.That(tokens[6].Type, Is.EqualTo(TokenType.ItalicsEnd));
                Assert.That(tokens[7].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[7].Value, Is.EqualTo(" символами"));
                Assert.That(tokens[8].Type, Is.EqualTo(TokenType.BoldEnd));
            }
        }

        [TestFixture]
        public class EscapeTests
        {
            private MarkdownTokenizer _tokenizer;

            [SetUp]
            public void Setup()
            {
                _tokenizer = new MarkdownTokenizer();
            }

            [Test]
            public void TokenizeLine_ShouldNotCreateEmphasisTokens_WithEscapedItalic()
            {
                var line = "\\_Вот это\\_, не должно выделиться тегом";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("_Вот это_, не должно выделиться тегом"));
            }

            [Test]
            public void TokenizeLine_ShouldKeepEscapeCharacters_WithEscapeCharactersNotEscaping()
            {
                var line = "Здесь сим\\волы экранирования\\ \\должны остаться.\\";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Здесь сим\\волы экранирования\\ \\должны остаться.\\"));
            }

            [Test]
            public void TokenizeLine_ShouldCreateItalic_WithEscapedEscapeCharacter()
            {
                var line = "Символ экранирования тоже можно экранировать: \\\\_вот это будет выделено тегом_";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(4));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Символ экранирования тоже можно экранировать: \\"));
                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.ItalicsStart));
                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[2].Value, Is.EqualTo("вот это будет выделено тегом"));
                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.ItalicsEnd));
            }

            [Test]
            public void TokenizeLine_ReturnsTextToken_WithOnlyEscapeCharacters()
            {
                var line = "\\\\\\";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("\\\\"));
            }

            [Test]
            public void TokenizeLine_HandlesCorrectly_WithMixedEscapeAndFormatting()
            {
                var line = "\\__это не жирный\\__ а _\\это не курсив\\_";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(1));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("__это не жирный__ а _\\это не курсив_"));
            }

            [Test]
            public void TokenizeLine_HandlesCorrectly_WithMultipleEscapeSequences()
            {
                var line = "\\\\_одинарный экранирование_ и \\\\\\_двойной экранирование_";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(5));
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("\\"));
                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.ItalicsStart));
                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[2].Value, Is.EqualTo("одинарный экранирование"));
                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.ItalicsEnd));
                Assert.That(tokens[4].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[4].Value, Is.EqualTo(" и \\_двойной экранирование_"));
            }
        }

        [TestFixture]
        public class LinkTests
        {
            private MarkdownTokenizer _tokenizer;

            [SetUp]
            public void Setup()
            {
                _tokenizer = new MarkdownTokenizer();
            }

            [Test]
            public void TokenizeLine_ReturnsLinkTokens_WithValidLinkFormat()
            {
                var line = "Посетите [наш сайт](https://example.com) для деталей";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens, Has.Count.EqualTo(8));

                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[0].Value, Is.EqualTo("Посетите "));

                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.LinkStart));
                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.LinkText));
                Assert.That(tokens[2].Value, Is.EqualTo("наш сайт"));
                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.LinkEnd));

                Assert.That(tokens[4].Type, Is.EqualTo(TokenType.UrlStart));
                Assert.That(tokens[5].Type, Is.EqualTo(TokenType.Url));
                Assert.That(tokens[5].Value, Is.EqualTo("https://example.com"));
                Assert.That(tokens[6].Type, Is.EqualTo(TokenType.UrlEnd));

                Assert.That(tokens[7].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[7].Value, Is.EqualTo(" для деталей"));
            }

            [Test]
            public void TokenizeLine_ReturnsLinkTokens_WithNestedFormatting()
            {
                var line = "[текст с _курсивом_ и __жирным__](https://example.com)";

                var tokens = _tokenizer.TokenizeLine(line);

                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.LinkStart));
                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.LinkText));
                Assert.That(tokens[1].Value, Is.EqualTo("текст с "));

                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.ItalicsStart));
                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[3].Value, Is.EqualTo("курсивом"));
                Assert.That(tokens[4].Type, Is.EqualTo(TokenType.ItalicsEnd));

                Assert.That(tokens[5].Type, Is.EqualTo(TokenType.LinkText));
                Assert.That(tokens[5].Value, Is.EqualTo(" и "));

                Assert.That(tokens[6].Type, Is.EqualTo(TokenType.BoldStart));
                Assert.That(tokens[7].Type, Is.EqualTo(TokenType.Text));
                Assert.That(tokens[7].Value, Is.EqualTo("жирным"));
                Assert.That(tokens[8].Type, Is.EqualTo(TokenType.BoldEnd));

                Assert.That(tokens[9].Type, Is.EqualTo(TokenType.LinkEnd));
                Assert.That(tokens[10].Type, Is.EqualTo(TokenType.UrlStart));
                Assert.That(tokens[11].Type, Is.EqualTo(TokenType.Url));
                Assert.That(tokens[11].Value, Is.EqualTo("https://example.com"));
                Assert.That(tokens[12].Type, Is.EqualTo(TokenType.UrlEnd));
            }
        }

        [TestFixture]
        public class HelperMethodsTests
        {
            private MarkdownTokenizer _tokenizer;

            [SetUp]
            public void Setup()
            {
                _tokenizer = new MarkdownTokenizer();
            }

            [Test]
            public void TextToLines_ReturnsCorrectLines_WithMultipleLines()
            {
                var text = "line1\nline2\nline3";

                var lines = _tokenizer.TextToLines(text);

                Assert.That(lines, Has.Count.EqualTo(3));
                Assert.That(lines[0], Is.EqualTo("line1"));
                Assert.That(lines[1], Is.EqualTo("line2"));
                Assert.That(lines[2], Is.EqualTo("line3"));
            }

            [Test]
            public void TextToLines_ReturnsEmptyList_WithEmptyString()
            {
                var text = "";

                var lines = _tokenizer.TextToLines(text);

                Assert.That(lines, Has.Count.EqualTo(1));
            }

            [Test]
            public void TokenizeLines_CombinesAllTokens_WithMultipleLines()
            {
                var lines = new List<string> { "line1", "line2", "line3" };

                var tokens = _tokenizer.TokenizeLines(lines);

                Assert.That(tokens, Has.Count.EqualTo(6));
            }
        }
    }
}