using System.Collections.Generic;
using FluentAssertions;
using Markdown.ConverterTokens;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests.ConverterInTokenTest
{
    [TestFixture]
    public class DoubleEmphasisTests
    {
        [TestCase("", 0, TestName = "строка пустая")]
        [TestCase(" ", 0, TestName = "строка - пробел")]
        [TestCase("____", 0, TestName = "строка - это четыре подчерка")]
        [TestCase("__sd", 0, TestName = "в строке два подчерка")]
        [TestCase("______qwert", 0, TestName = "в строке четыре подчерка подряд")]
        [TestCase("qw__ert__", 0, TestName = "токен начинается не с указанного индекса")]
        [TestCase("qw__1r3__", 2, TestName = "цифры в слове с выделением")]
        [TestCase("qw__rt йцу__", 2, TestName = "токен начинается в одном слове, а закакнчивается в другом")]
        [TestCase("__ qwer__", 0, TestName = "пробельный символ после первой пары подчерков")]
        [TestCase("__qwer __", 0, TestName = "пробельный символ перед окончанием токена")]
        [TestCase("__qwer\\__", 0, TestName = "третий подчерк заэкранирован")]
        public void MakeConverter_ReturnNull(string str, int startIndex)
        {
            var doubleEmphasis = new DoubleEmphasis();
            doubleEmphasis.MakeConverter(str, startIndex).Should().BeNull();
        }

        [Test, TestCaseSource(nameof(DifferentInputs))]
        public void MakeConverter_ReturnToken(string str, int startIndex, IToken result)
        {
            var doubleEmphasis = new DoubleEmphasis();
            doubleEmphasis.MakeConverter(str, startIndex).Should().Be(result);
        }

        public static IEnumerable<TestCaseData> DifferentInputs
        {
            get
            {
                yield return new TestCaseData("q__wer__t", 1, new DoubleEmphasisToken("wer", 1, new IToken[0]))
                    .SetName("токен находиться в слове");
                yield return new TestCaseData("__qwert__", 0, new DoubleEmphasisToken("qwert", 0, new IToken[0]))
                    .SetName("слово окруженное двумя подчерками");
                yield return new TestCaseData("__qwert йцук__", 0,
                        new DoubleEmphasisToken("qwert йцук", 0, new IToken[0]))
                    .SetName("два слова окруженное двумя парами подчерков");
                yield return new TestCaseData("Привет __qwert йцук__ пока", 7,
                        new DoubleEmphasisToken("qwert йцук", 7, new IToken[0]))
                    .SetName("токен в середине строки");
                yield return new TestCaseData("Привет.__qwert йцук__", 7,
                        new DoubleEmphasisToken("qwert йцук", 7, new IToken[0]))
                    .SetName("токен начинается в начале предложения");
                yield return new TestCaseData("__qwert 123 йцук__", 0,
                        new DoubleEmphasisToken("qwert 123 йцук", 0, new IToken[0]))
                    .SetName("токен с цифрами в не граничных словах");
                yield return new TestCaseData("__qwert __ йцук__ пока", 0,
                        new DoubleEmphasisToken("qwert __ йцук", 0, new IToken[0]))
                    .SetName("подчерк отделен пробелами в центре токена");
                yield return new TestCaseData("__qwert\\\\__", 0,
                        new DoubleEmphasisToken("qwert\\\\", 0, new IToken[0]))
                    .SetName("заэкранирован символ экранирования второго подчерка");
                yield return new TestCaseData("__\\___", 0, new DoubleEmphasisToken("\\_", 0, new IToken[0]))
                    .SetName(@"строка вида __\___");
            }
        }
    }
}