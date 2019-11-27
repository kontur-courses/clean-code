using NUnit.Framework;
using FluentAssertions;
using System;
using Programm;


namespace Tests
{
    [TestFixture]
    public class RenderShould
    {
        [Test]
        [TestCase("abc eqwe", ExpectedResult = "abc eqwe")]
        [TestCase("123", ExpectedResult = "123")]
        [TestCase("Hi, User! How are you?", ExpectedResult = "Hi, User! How are you?")]
        public string GiveTheSameString_IfParagraphWithoutUnderscores(string paragraph)
        {
            return Md.Render(paragraph);
        }

        [Test]
        [TestCase("abc_", ExpectedResult = "abc_")]
        [TestCase("vvv_vv", ExpectedResult = "vvv_vv")]
        [TestCase("123 _ 123", ExpectedResult = "123 _ 123")]
        public string GiveTheSameString_IfOnlyOneUnderScore(string paragraph)
        {
            return Md.Render(paragraph);
        }

        [Test]
        [TestCase("abc\\_", ExpectedResult = "abc_")]
        [TestCase("\\_abc\\_", ExpectedResult = "_abc_")]
        [TestCase("\\\\\\_abc\\\\\\_", ExpectedResult = "\\_abc\\_")]
        public string MuteUnderScore_IfSlashBeforeIt(string paragraph)
        {
            return Md.Render(paragraph);
        }

        [Test]
        [TestCase("_abc_", ExpectedResult = "<em>abc</em>")]
        [TestCase("bcd_qwe qwe_bcd", ExpectedResult = "bcd<em>qwe qwe</em>bcd")]
        [TestCase("_l   l_", ExpectedResult = "<em>l   l</em>")]
        public string MakeStringCurved_IfValidPairOfUnderScore(string paragraph)
        {
            return Md.Render(paragraph);
        }

        [Test]
        [TestCase("_abc _", ExpectedResult = "_abc _")]
        [TestCase("_l  l _", ExpectedResult = "_l  l _")]
        [TestCase("qwe_qweqwe _qwe", ExpectedResult = "qwe_qweqwe _qwe")]
        public string NotMakeStringCurved_IfClosingUnderScoreAfterSpace(string paragraph)
        {
            return Md.Render(paragraph);
        }

        [Test]
        [TestCase("_ abc_", ExpectedResult = "_ abc_")]
        [TestCase("_ l   l_", ExpectedResult = "_ l   l_")]
        [TestCase("qwe_ qweqwe_qwe", ExpectedResult = "qwe_ qweqwe_qwe")]
        public string NotMakeStringCurved_IfOpeningUnderScoreBeforeSpace(string paragraph)
        {
            return Md.Render(paragraph);
        }

        [Test]
        [TestCase("1_abc_", ExpectedResult = "1_abc_")]
        [TestCase("_abc1_", ExpectedResult = "_abc1_")]
        [TestCase("_abc_1", ExpectedResult = "_abc_1")]
        [TestCase("2_abc_1", ExpectedResult = "2_abc_1")]
        public string NotMakeStringCurved_IfUnderScoreAdjacentTosDigit(string paragraph)
        {
            return Md.Render(paragraph);
        }

        [Test]
        [TestCase("_abc_abc_", ExpectedResult = "<em>abc</em>abc_")]
        [TestCase("_l  l__", ExpectedResult = "<em>l  l</em>_")]
        [TestCase("__l  l_", ExpectedResult = "_<em>l  l</em>")]
        public string NotMakePartOfStringCurved_IfNoPairToUnderScore(string paragraph)
        {
            return Md.Render(paragraph);
        }

        [Test]
        [TestCase("_abc_abc_abc_", ExpectedResult = "<em>abc</em>abc<em>abc</em>")]
        [TestCase("_abc _abc_ abc_", ExpectedResult = "<em>abc <em>abc</em> abc</em>")]
        [TestCase("_abc _a_abc_a_ abc_", ExpectedResult = "<em>abc <em>a</em>abc</em>a_ abc_")]
        public string MakeStringCurved_IfNMutipleUnderScores(string paragraph)
        {
            return Md.Render(paragraph);
        }

        [Test]
        [TestCase("__abc__", ExpectedResult = "<strong>abc</strong>")]
        [TestCase("__abc__abc__abc__", ExpectedResult = "<strong>abc</strong>abc<strong>abc</strong>")]
        [TestCase("__a __b__ a__", ExpectedResult = "<strong>a <strong>b</strong> a</strong>")]
        public string MakeStringBold_IfDoubleUnderScores(string paragraph)
        {
            return Md.Render(paragraph);
        }

        [Test]
        [TestCase("__a _abc_ a__", ExpectedResult = "<strong>a <em>abc</em> a</strong>")]
        public string MakeStringBoldAndCurved_IfNestingUnderScores(string paragraph)
        {
            return Md.Render(paragraph);
        }


        [Test]
        [TestCase("_a __abc__ a_", ExpectedResult = "<em>a __abc__ a</em>")]
        public string MuteDoubleUnderScores_IfTheyInUnaryUnderScores(string paragraph)
        {
            return Md.Render(paragraph);
        }

        [Test]
        [TestCase("__abc _qwe qweqwe__ dase_", ExpectedResult = "<em><em>abc <em>qwe qweqwe</em></em> dase</em>")]
        public string MakeCurvedStringAndNotBold_IfUnaryUnderScoresIntersectBinaryOnes(string paragraph)
        {
            return Md.Render(paragraph);
        }
    }
}