using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Markdown.Tests
{
    [TestFixture]
    public class ConvertTests
    {
        private TokenReader reader;

        [SetUp]
        public void SetUp()
        {
            reader = new TokenReader();
        }

        [TestCase("__abcd__", ExpectedResult = "<strong>abcd</strong>", TestName = "OnSimpleBoldText")]
        [TestCase("_abcdef_", ExpectedResult = "<em>abcdef</em>", TestName = "OnSimpleItalicText")]
        [TestCase("#abcdef", ExpectedResult = "<h1>abcdef</h1>", TestName = "OnSimpleHeaderText")]
        [TestCase("__ab_cd_ef__", ExpectedResult = "<strong>ab<em>cd</em>ef</strong>", TestName = "OnItalicInBoldText")]
        [TestCase("#__ab_cd_ef__", ExpectedResult = "<h1><strong>ab<em>cd</em>ef</strong></h1>", TestName = "OnItalicInBoldTextWithHeader")]
        public string ConvertTokenToHtml(string line)
        {
            var token = reader.ReadTokens(line).First();
            return MdConvert.TokenToHtml(token);
        }

        [TestCase("_ab_cd_ef_", ExpectedResult = "<em>ab</em>cd<em>ef</em>", TestName = "OnSomeItalicTokensInText")]
        [TestCase("__ab__cd__ef__", ExpectedResult = "<strong>ab</strong>cd<strong>ef</strong>", TestName = "OnSomeBoldTokensInText")]
        [TestCase("_ab_c__d_e_f__", ExpectedResult = "<em>ab</em>c<strong>d<em>e</em>f</strong>", TestName = "OnNestedTokensText")]
        public string ConvertTokensToHtml(string line)
        {
            var tokens = reader.ReadTokens(line);
            return MdConvert.ToHtml(tokens);
        }
    }
}
