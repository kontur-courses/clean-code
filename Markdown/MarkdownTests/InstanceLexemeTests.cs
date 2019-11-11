using FluentAssertions;
using Markdown;
using Markdown.MarkdownDocument.Inline;
using NUnit.Framework;

namespace MarkdownTests
{
    public class InstanceLexemeTests
    {
        [Test, TestCaseSource(nameof(WhitespaceCharacters))]
        public void IsWhiteSpace_True_WhitespaceCharacters(char c)
        {
            Lexeme.CreateFromChar(c).IsWhitespace().Should().BeTrue();
        }

        [Test, TestCaseSource(nameof(NumericCharacters))]
        public void IsWhiteSpace_False_NumericCharacters(char c)
        {
            Lexeme.CreateFromChar(c).IsWhitespace().Should().BeFalse();
        }
        
        [Test, TestCaseSource(nameof(AlphabeticCharacters))]
        public void IsWhiteSpace_False_AlphabeticCharacters(char c)
        {
            Lexeme.CreateFromChar(c).IsWhitespace().Should().BeFalse();
        }

        [Test, TestCaseSource(nameof(PunctuationCharacters))]
        public void IsWhiteSpace_False_PunctuationCharacters(char c)
        {
            Lexeme.CreateFromChar(c).IsWhitespace().Should().BeFalse();
        }

        [Test, TestCaseSource(nameof(WhitespaceCharacters))]
        public void IsPunctuation_False_WhitespaceCharacters(char c)
        {
            Lexeme.CreateFromChar(c).IsPunctuation().Should().BeFalse();
        }

        [Test, TestCaseSource(nameof(NumericCharacters))]
        public void IsPunctuation_False_NumericCharacters(char c)
        {
            Lexeme.CreateFromChar(c).IsPunctuation().Should().BeFalse();
        }
        
        [Test, TestCaseSource(nameof(AlphabeticCharacters))]
        public void IsPunctuation_False_AlphabeticCharacters(char c)
        {
            Lexeme.CreateFromChar(c).IsPunctuation().Should().BeFalse();
        }

        [Test, TestCaseSource(nameof(PunctuationCharacters))]
        public void IsPunctuation_True_PunctuationCharacters(char c)
        {
            Lexeme.CreateFromChar(c).IsPunctuation().Should().BeTrue();
        }
        
        [Test, TestCaseSource(nameof(WhitespaceCharacters))]
        public void IsDigit_False_WhitespaceCharacters(char c)
        {
            Lexeme.CreateFromChar(c).IsDigit().Should().BeFalse();
        }

        [Test, TestCaseSource(nameof(NumericCharacters))]
        public void IsDigit_True_NumericCharacters(char c)
        {
            Lexeme.CreateFromChar(c).IsDigit().Should().BeTrue();
        }
        
        [Test, TestCaseSource(nameof(AlphabeticCharacters))]
        public void IsDigit_False_AlphabeticCharacters(char c)
        {
            Lexeme.CreateFromChar(c).IsDigit().Should().BeFalse();
        }

        [Test, TestCaseSource(nameof(PunctuationCharacters))]
        public void IsDigit_False_PunctuationCharacters(char c)
        {
            Lexeme.CreateFromChar(c).IsDigit().Should().BeFalse();
        }

        private static readonly char[] WhitespaceCharacters = {
            '\u0020', // Space
            '\u0009', // Tab
            '\u000a', // Newline
            '\u000b', // Line tabulation
            '\u000c', // Form feed
            '\u000d', // Carriage return
        };

        private static readonly char[] PunctuationCharacters = {
            '!', ',', '"', '#', '$', '%', '&', '\'',
            '(', ')', '*', '+', ',', '-', '.', '/',
            ':', ';', '<', '=', '>', '?', '@', '[',
            '\\', ']', '^', '_', '`', '{', '|', '}', '~'
        };

        private static readonly char[] NumericCharacters = {
            '1', '2', '3', '4', '5', '6', '7', '9', '0',
        };

        private static readonly char[] AlphabeticCharacters = {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
            'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
            's', 't', 'u', 'v', 'w', 'x', 'y', 'z',

            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
            'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R',
            'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',

            'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з',
            'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р',
            'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ',
            'ъ', 'ы', 'ь', 'э', 'ю', 'я',

            'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'Х',
            'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р',
            'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ',
            'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я',
        };
    }
}