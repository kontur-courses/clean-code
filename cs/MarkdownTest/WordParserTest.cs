using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    [TestFixture]
    public class WordParserTest_Should
    {
        [Test]
        public void ShouldReturnCorrect_WhenUnderscoreAtTheBeginning()
        {
            var word = "нач_але";
            var result = "<em>нач</em>але";
            HtmlMaker.FromTextInfo(WordParser.Parse(word)).Should().Be(result);
        }

        [Test]
        public void ShouldReturnCorrect_WhenUnderscoreAtTheEnd()
        {
            var word = "кон_це";
            var result = "кон<em>це</em>";
            HtmlMaker.FromTextInfo(WordParser.Parse(word)).Should().Be(result);
        }

        [Test]
        public void ShouldReturnCorrect_WhenUnderscoreInTheMiddle()
        {
            var word = "сер_еди_не";
            var result = "сер<em>еди</em>не";
            HtmlMaker.FromTextInfo(WordParser.Parse(word)).Should().Be(result);
        }

        [Test]
        public void ShouldntParse_WhenDigitsInWord()
        {
            var word = "цифрами_12_3";
            var result = "цифрами_12_3";
            HtmlMaker.FromTextInfo(WordParser.Parse(word)).Should().Be(result);
        }

        [Test]
        [TestCase("много_подче_ркива_ний", "много<em>подче</em>ркива_ний")]
        [TestCase("много_подче_рк_ива_ний", "много<em>подче</em>рк<em>ива</em>ний")]
        [TestCase("много_подче___рк_ива_ний", "много<em>подче___рк</em>ива_ний")]
        public void ShouldReturnCorrect_ThreeOrMoreUnderscoreInWord(string word, string result)
        {
            HtmlMaker.FromTextInfo(WordParser.Parse(word)).Should().Be(result);
        }
    }
}