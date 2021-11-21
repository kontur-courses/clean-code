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
            Tokenizer.GetToken(text).Should().BeEquivalentTo(expectedToken);
        }
        
        [Test]
        public void GetToken_OnSimpleTextWithItalic_ShouldWorkCorrectly()
        {
            var text = "Ну и _что_ дальше?";
            var expectedToken = new Token(0, text.Length);
            expectedToken.nestedTokens.Add(new ItalicToken(5, 5));
            Tokenizer.GetToken(text).Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void GetToken_OnSimpleTextWithTwoItalics_ShouldWorkCorrectly()
        {
            var text = "Ну и _что_ _дальше_?";
            var expectedToken = new Token(0, text.Length);
            expectedToken.nestedTokens.Add(new ItalicToken(5, 5));
            expectedToken.nestedTokens.Add(new ItalicToken(11, 8));
            Tokenizer.GetToken(text).Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void GetToken_OnSimpleTextWithBold_ShouldWorkCorrectly()
        {
            var text = "Ну и __что__ дальше?";
            var expectedToken = new Token(0, text.Length);
            expectedToken.nestedTokens.Add(new BoldToken(5, 7));
            Tokenizer.GetToken(text).Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void GetToken_OnSimpleTextWithBoldAndItalic_ShouldWorkCorrectly()
        {
            var text = "Ну и __что__ _дальше_?";
            var expectedToken = new Token(0, text.Length);
            expectedToken.nestedTokens.Add(new BoldToken(5, 7));
            expectedToken.nestedTokens.Add(new ItalicToken(13, 8));
            var actualToken = Tokenizer.GetToken(text);
            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void GetToken_OnTextWithOnlyOneGround_ShouldWorkCorrectly()
        {
            var text = "_А как тебе такое, Илон Маск?";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);
            actualToken.Should().BeEquivalentTo(expectedToken);
        }
        [Test]
        public void GetToken_OnTextWithTwoGroundsInRow_ShouldWorkCorrectly()
        {
            var text = "__А как тебе такое, Илон Маск?";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);
            actualToken.Should().BeEquivalentTo(expectedToken);
        }
        
        [Test]
        public void GetToken_OnTextWithEscapeAndGround_ShouldWorkCorrectly()
        {
            var text = "Всякое \\_иногда бывает\\_";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);
            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void GetToken_OnTextWithEscapeWithoutGround_ShouldWorkCorrectly()
        {
            var text = "\\Но иногда это \\\\ просто иллюзия\\";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);
            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void GetToken_OnTextWithHeader_ShouldWorkCorrectly()
        {
            var text = "# Самый главный заголовок.";
            var expectedToken = new Token(0, text.Length);
            expectedToken.nestedTokens.Add(new HeaderToken(0, text.Length));
            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void GetToken_OnTextWithHashTag_ShouldWorkCorrectly()
        {
            var text = "Иногда это просто #хештег";
            var expectedToken = new Token(0, text.Length);
            var actualToken = Tokenizer.GetToken(text);

            actualToken.Should().BeEquivalentTo(expectedToken);
        }
    }
}
