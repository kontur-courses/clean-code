using System.Collections.Generic;
using FluentAssertions;
using Markdown.CoreParser.ConverterInTokens;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests.ConverterInTokenTest
{
    [TestFixture]
    public class SingleEmphasisTests
    {
        [TestCase("", 0, TestName = "строка пустая")]
        [TestCase(" ", 0, TestName = "строка - пробел")]
        [TestCase("__", 0, TestName = "строка - это два подчерка")]
        [TestCase("_sd", 0, TestName = "в строке один подчерка")]
        [TestCase("__qwert", 0, TestName = "в строке два подчерка подряд")]
        [TestCase("qw_ert_", 0, TestName = "токен начинается не с указанного индекса")]
        [TestCase("qw_1r3_", 2, TestName = "цифры в слове с выделением")]
        [TestCase("qw_rt йцу_", 2, TestName = "токен начинается в одном, а закакнчивается в другом")]
        [TestCase("_ qwer_", 0, TestName = "пробельный символ после первого подчерка")]
        [TestCase("_qwer _ ", 0, TestName = "пробельный символ перед окончанием токена")]
        [TestCase("_qwer\\_", 0, TestName = "второй подчерк заэкранирована")]
        public void MakeConverter_ReturnNull(string str, int startIndex)
        {
            var singleEmphasis = new SingleEmphasis();
            singleEmphasis.SelectTokenInString(str, startIndex).Should().BeNull();
        }
        
        [Test, TestCaseSource(nameof(DifferentInputs))]
        public void MakeConverter_ReturnToken(string str, int startIndex, IToken result)
        {
            var singleEmphasis = new SingleEmphasis();
            singleEmphasis.SelectTokenInString(str, startIndex).Should().Be(result);
        }
        
        public static IEnumerable<TestCaseData>  DifferentInputs 
        {
            get
            {
                yield return new TestCaseData("q_e_t", 1, new SingleEmphasisToken("e", 1, new IToken[0]))
                    .SetName("токен находиться в слове");
                yield return new TestCaseData("вы_делен_ием", 2, new SingleEmphasisToken("делен", 2, new IToken[0]))
                    .SetName("подчерки находиться внутри слове");
                yield return new TestCaseData("_qwert_", 0, new SingleEmphasisToken("qwert", 0, new IToken[0]))
                    .SetName("слово окруженное двумя землями");
                yield return new TestCaseData("_qwert йцук_", 0, new SingleEmphasisToken("qwert йцук", 0, new IToken[0]))
                    .SetName("два слова окруженное двумя подчерками");
                yield return new TestCaseData("Привет _qwert йцук_ пока", 7, new SingleEmphasisToken("qwert йцук", 7, new IToken[0]))
                    .SetName("токен в середине строки");
                yield return new TestCaseData("Привет._qwert йцук_", 7, new SingleEmphasisToken("qwert йцук", 7, new IToken[0]))
                    .SetName("токен начинается в начале предложения");
                yield return new TestCaseData("_qwert 123 йцук_", 0, new SingleEmphasisToken("qwert 123 йцук", 0, new IToken[0]))
                    .SetName("токен с цифрами в не граничных словах");
                yield return new TestCaseData("_qwert _ йцук_ пока", 0, new SingleEmphasisToken("qwert _ йцук", 0, new IToken[0]))
                    .SetName("подчерк отделенн пробелами в центре");
                yield return new TestCaseData("_qwert\\\\_", 0, new SingleEmphasisToken("qwert\\\\", 0, new IToken[0]))
                    .SetName("заэкранирован символ экранирования второго подчерка");
                yield return new TestCaseData("_\\__", 0, new SingleEmphasisToken("\\_", 0, new IToken[0]))
                    .SetName(@"строка вида _\__");
            }
        }
        
    }
}