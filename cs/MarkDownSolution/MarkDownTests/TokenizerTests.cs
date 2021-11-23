using FluentAssertions;
using MarkDown;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownTests
{
    public class TokenizerTests
    {
        [Test]
        public void GetToken_OnTextWithoutSpecialSymbols_ShouldWorkCorrectly()
        {
            var text = "А роза упала на лапу Азора";
            var expectedToken = new Token(0, text.Length);
            Tokenizer.GetToken(text).Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }
        
        [Test]
        public void GetToken_OnSimpleTextWithItalic_ShouldWorkCorrectly()
        {
            var text = "Ну и _что_ дальше?";
            var expectedToken = new Token(0, text.Length);
            expectedToken.AddNestedToken(new ItalicToken(5, 5));
            Tokenizer.GetToken(text).Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnSimpleTextWithTwoItalics_ShouldWorkCorrectly()
        {
            var text = "Ну и _что_ _дальше_?";
            var expectedToken = new Token(0, text.Length);
            expectedToken.AddNestedToken(new ItalicToken(5, 5));
            expectedToken.AddNestedToken(new ItalicToken(11, 8));
            Tokenizer.GetToken(text).Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnSimpleTextWithBold_ShouldWorkCorrectly()
        {
            var text = "Ну и __что__ дальше?";
            var expectedToken = new Token(0, text.Length);
            expectedToken.AddNestedToken(new BoldToken(5, 7));
            Tokenizer.GetToken(text).Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnSimpleTextWithBoldAndItalic_ShouldWorkCorrectly()
        {
            var text = "Ну и __что__ _дальше_?";
            var expectedToken = new Token(0, text.Length);
            expectedToken.AddNestedToken(new BoldToken(5, 7));
            expectedToken.AddNestedToken(new ItalicToken(13, 8));
            var actualToken = Tokenizer.GetToken(text);
            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnTextWithOnlyOneGround_ShouldWorkCorrectly()
        {
            var text = "_А как тебе такое, Илон Маск?";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);
            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }
        [Test]
        public void GetToken_OnTextWithTwoGroundsInRow_ShouldWorkCorrectly()
        {
            var text = "__А как тебе такое, Илон Маск?";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);
            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }
        
        [Test]
        public void GetToken_OnTextWithEscapeAndGround_ShouldWorkCorrectly()
        {
            var text = "Всякое \\_иногда бывает\\_";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);
            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }
        [Test]
        public void GetToken_OnTextWithEscapeAndDoubleGround_ShouldWorkCorrectly()
        {
            var text = "Всякое \\__иногда бывает\\__";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);
            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnTextWithEscapeWithoutGround_ShouldWorkCorrectly()
        {
            var text = "\\Но иногда это \\\\ просто иллюзия\\";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);
            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnTextWithHeader_ShouldWorkCorrectly()
        {
            var text = "# Самый главный заголовок.";
            var expectedToken = new Token(0, text.Length);
            expectedToken.AddNestedToken(new HeaderToken(0, text.Length));
            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnTextWithHashTag_ShouldWorkCorrectly()
        {
            var text = "Иногда это просто #хештег";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_InsideTextWithDigits_ShouldWorkCorrectly()
        {
            var text = "Текст _с цифрами 123_ не выделяется.";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OutsideTextWithDigits_ShouldWorkCorrectly()
        {
            var text = "_Текст_ с цифрами 123 __выделяется__.";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);
            expectedToken.AddNestedToken(new ItalicToken(0, 7));
            expectedToken.AddNestedToken(new BoldToken(22, 14));

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }
        
        [Test]
        public void GetToken_OnPartOfWord_ShouldTokenize()
        {
            var text = "_Ино_гда";
            var expectedToken = new Token(0, text.Length);
            expectedToken.AddNestedToken(new ItalicToken(0, 5));
            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnPartsOfDifferentWords_ShouldNotTokenize()
        {
            var text = "пло_хая по_года";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnWhiteSpaceAfterGround_ShouldNotTokenize()
        {
            var text = "эти_ подчерки_ не считаются выделением";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnWhiteSpaceBeforeGround_ShouldNotTokenize()
        {
            var text = "эти _подчерки _не считаются_ окончанием выделения";
            var expectedToken = new Token(0, text.Length);
            expectedToken.AddNestedToken(new ItalicToken(4, 24));
            var actualToken = Tokenizer.GetToken(text);       

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnFourGroundsInRow_ShouldNotTokenize()
        {
            var text = "____";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }
        
        [Test]
        public void GetToken_OnTwoGroundsInRow_ShouldNotTokenize()
        {
            var text = "__";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnBoldInsideItalic_ShouldTokenize()
        {
            var text = "_итальянец и __жирный__ текст_";
            var expectedToken = new Token(0, text.Length);
            var firstNested = new ItalicToken(0, text.Length);
            var secondNested = new BoldToken(13, 10);
            secondNested.fatherToken = firstNested;
            firstNested.fatherToken = expectedToken;
            expectedToken.GetNestedTokens().Add(firstNested);
            firstNested.GetNestedTokens().Add(secondNested);

            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnItalicInsideBold_ShouldTokenize()
        {
            var text = "__жирный итальянец _и_ курсив__";
            var expectedToken = new Token(0, text.Length);
            var firstNested = new BoldToken(0, text.Length);
            var secondNested = new ItalicToken(19, 3);
            secondNested.fatherToken = firstNested;
            firstNested.fatherToken = expectedToken;
            expectedToken.GetNestedTokens().Add(firstNested);
            firstNested.GetNestedTokens().Add(secondNested);

            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_OnInvalidSituation_ShouldNotTokenize()
        {
            var text = "__Мне кажется это будет сложно_";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }

        [Test]
        public void GetToken_ItalicInsideHeader_ShouldTokenize()
        {
            var text = "# Всё _просто_";
            var innerInnerToken = new ItalicToken(6, 8);
            var innerToken = new HeaderToken(0, text.Length);
            var expectedToken = new Token(0, text.Length);

            innerToken.AddNestedToken(innerInnerToken);
            expectedToken.AddNestedToken(innerToken);

            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken, p => p.IgnoringCyclicReferences());
        }
    }
}
