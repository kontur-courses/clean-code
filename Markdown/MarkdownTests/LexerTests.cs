using System.Linq;
using FluentAssertions;
using Markdown;
using Markdown.MarkdownDocument.Inline;
using NUnit.Framework;

namespace MarkdownTests
{
    public class LexerTests
    {
        [Test]
        public void ExtractLexemes_Letters_Correct()
        {
            var expectedLexemes = new[]
            {
                Lexeme.CreateFromChar('h'),
                Lexeme.CreateFromChar('e'),
                Lexeme.CreateFromChar('l'),
                Lexeme.CreateFromChar('l'),
                Lexeme.CreateFromChar('o')
            };

            var lexemes = Lexer.ExtractLexemes("hello").ToArray();

            lexemes.Length.Should().Be(expectedLexemes.Length);
            lexemes.Should().BeEquivalentTo(expectedLexemes);
        }

        [Test]
        public void ExtractLexemes_Digits_Correct()
        {
            var expectedLexemes = new[]
            {
                Lexeme.CreateFromChar('1'),
                Lexeme.CreateFromChar('2'),
                Lexeme.CreateFromChar('3'),
                Lexeme.CreateFromChar('4'),
                Lexeme.CreateFromChar('5')
            };

            var lexemes = Lexer.ExtractLexemes("12345").ToArray();

            lexemes.Length.Should().Be(expectedLexemes.Length);
            lexemes.Should().BeEquivalentTo(expectedLexemes);
        }

        [Test]
        public void ExtractLexemes_Punctuation_Correct()
        {
            var expectedLexemes = new[]
            {
                Lexeme.CreateFromChar('!'),
                Lexeme.CreateFromChar(','),
                Lexeme.CreateFromChar('_'),
                Lexeme.CreateFromChar('='),
                Lexeme.CreateFromChar('_'),
                Lexeme.CreateFromChar('_')
            };

            var lexemes = Lexer.ExtractLexemes("!,_=__").ToArray();

            lexemes.Length.Should().Be(expectedLexemes.Length);
            lexemes.Should().BeEquivalentTo(expectedLexemes);
        }

        [Test]
        public void ExtractLexemes_EscapedPunctuation_Correct()
        {
            var expectedLexemes = new[]
            {
                Lexeme.CreateFromChar('!', true),
                Lexeme.CreateFromChar(',', true),
                Lexeme.CreateFromChar('_', true),
                Lexeme.CreateFromChar('=', true),
                Lexeme.CreateFromChar('_', true),
                Lexeme.CreateFromChar('_', true)
            };

            var lexemes = Lexer.ExtractLexemes("\\!\\,\\_\\=\\_\\_").ToArray();

            lexemes.Length.Should().Be(expectedLexemes.Length);
            lexemes.Should().BeEquivalentTo(expectedLexemes);
        }

        [Test]
        public void ExtractLexemes_UnderlinesWithNumbers_Escaped()
        {
            var lexemes = Lexer.ExtractLexemes("_abc1_ ab_c_d1").ToArray();
            var underlinesAreEscaped = lexemes.Where(lexeme => lexeme.Value == "_").All(lexeme => lexeme.IsEscaped());
            underlinesAreEscaped.Should().BeTrue("underlines in text with numbers should be escaped");
        }
    }
}