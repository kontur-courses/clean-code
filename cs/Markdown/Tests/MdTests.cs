using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;


namespace Markdown.Tests
{
    [TestFixture]
    public class MdTests
    {
        private Md mdRender;

        [SetUp]
        public void SetUp()
        {
            mdRender = new Md();
        }

        [TestCase("__abcd__", ExpectedResult = "<strong>abcd</strong>",
            TestName = "Should_Parse_Bold_Formatting_In_Text")]
        [TestCase("_abcdef_", ExpectedResult = "<em>abcdef</em>",
            TestName = "Should_Parse_Italic_Formatting_In_Text")]
        [TestCase("#abcdef", ExpectedResult = "<h1>abcdef</h1>",
            TestName = "Should_Parse_Header_Formatting_In_Text")]
        public string MdRenderOnSimpleTokens(string line)
        {
            return mdRender.Render(line);
        }

        [TestCase("_ab_cd_ef_", ExpectedResult = "<em>ab</em>cd<em>ef</em>",
            TestName = "Should_Parse_Some_Italic_Formatting_In_Text")]
        [TestCase("__ab__cd__ef__", ExpectedResult = "<strong>ab</strong>cd<strong>ef</strong>",
            TestName = "Should_Parse_Some_Bold_Formatting_In_Text")]
        [TestCase("_ab_c__d_e_f__", ExpectedResult = "<em>ab</em>c<strong>d<em>e</em>f</strong>",
            TestName = "Should_Stop_Parse_Formatting_Text_If_Text_Finish")]
        [TestCase("__ab_cd_ef__", ExpectedResult = "<strong>ab<em>cd</em>ef</strong>",
            TestName = "Should_Parse_Italic_Text_Inside_Bold")]
        [TestCase("_ab__c__de_", ExpectedResult = "<em>ab__c__de</em>",
            TestName = "Should_Not_Parse_Neseted_Bold_Text_In_Italic")]
        [TestCase("#__ab_cd_ef__", ExpectedResult = "<h1><strong>ab<em>cd</em>ef</strong></h1>",
            TestName = "Should_Parse_Some_Nested_Tokens")]
        public string MdRenderOnNestedTokens(string line)
        {
            return mdRender.Render(line);
        }

        [TestCase(@"\__abcd\__", ExpectedResult = "__abcd__",
            TestName = "Should_Not_Parse_Escaped_Bold_Formatting_Char")]
        [TestCase(@"\_abcd\_", ExpectedResult = "_abcd_",
            TestName = "Should_Not_Parse_Escaped_Italic_Formatting_Char")]
        [TestCase(@"\#abcd", ExpectedResult = "#abcd",
            TestName = "Should_Not_Parse_Escaped_Header_Formatting_Char")]
        [TestCase(@"\\\#abcd", ExpectedResult = @"\#abcd",
            TestName = "Sheilding_Characters_Сan_Shield_Themselves")]
        [TestCase(@"ab\cd", ExpectedResult = @"ab\cd",
            TestName = "Sheilding_Characters_Should_Sheild_Only_Formatting_Characters")]
        [TestCase(@"__c\_e_t_f\_f__", ExpectedResult = "<strong>c_e<em>t</em>f_f</strong>",
            TestName = "Escaped_Characters_Can_Be_Nested")]
        [TestCase(@"__c\a__", ExpectedResult = @"<strong>c\a</strong>",
            TestName = "Escaped_Characters_Should_Not_Parse_If_They_Dont't_Sheild_Anything")]
        public string MdRenderOnShieldingText(string line)
        {
            return mdRender.Render(line);
        }

        [TestCase("__abc", ExpectedResult = "__abc",
            TestName = "Should_Not_Parse_Unpaired_Bold_Formatting_Char")]
        [TestCase("__ab_cd__", ExpectedResult = "__ab_cd__",
            TestName = "Should_Not_Parse_Intersect_Formatting_Char")]
        [TestCase("__ab_cd", ExpectedResult = "__ab_cd",
            TestName = "Should_Not_Parse_Unpaired_Italic_Formatting_Char_In_Bold")]
        [TestCase("__ abcd__", ExpectedResult = "__ abcd__",
            TestName = "Should_Not_Parse_Bold_Text_If_It_Starts_With_Non_Whitespace_Char")]
        [TestCase("__abcd __", ExpectedResult = "__abcd __",
            TestName = "Should_Not_Parse_Bold_Text_If_It_End_With_Non_Whitespace_Char")]
        [TestCase("__123__", ExpectedResult = "<strong>123</strong>",
            TestName = "Should_Pars_Bold_Text_With_Digit_Value")]
        [TestCase("_12_3", ExpectedResult = "_12_3",
            TestName = "Formatting_Bold_Char_Inside_Digit_Text_Should_Not_Parse")]
        [TestCase("____", ExpectedResult = "____",
            TestName = "Should_Not_Parse_Bold_Text_If_Empty_String_Inside")]
        [TestCase("ab__cb ef__g", ExpectedResult = "ab__cb ef__g",
            TestName = "Should_Not_Parse_Bold_Text_Inside_Different_Parts_Words")]
        [TestCase("__#__aa", ExpectedResult = "<strong>#</strong>aa",
            TestName = "Nested_Formatting_Header_Char_Inside_Bold_Text_Should_Not_Parse")]
        public string MdRenderOnCorruptedBoldText(string line)
        {
            return mdRender.Render(line);
        }

        [TestCase("_abc", ExpectedResult = "_abc",
            TestName = "Should_Not_Parse_Unpaired_Italic_Formatting_Char")]
        [TestCase("_ abcd_", ExpectedResult = "_ abcd_",
            TestName = "Should_Not_Parse_Italic_Text_If_It_Starts_With_Non_Whitespace_Char")]
        [TestCase("_abcd _", ExpectedResult = "_abcd _",
            TestName = "Should_Not_Parse_Italic_Text_If_It_End_With_Non_Whitespace_Char")]
        [TestCase("_12_3", ExpectedResult = "_12_3",
            TestName = "Formatting_Italic_Char_Inside_Digit_Text_Should_Not_Parse")]
        [TestCase("ab_cb ef_g", ExpectedResult = "ab_cb ef_g",
            TestName = "Should_Not_Parse_Italic_Text_Inside_Different_Parts_Words")]
        [TestCase("_#_aa", ExpectedResult = "<em>#</em>aa",
            TestName = "Nested_Formatting_Header_Char_Inside_Italic_Text_Should_Not_Parse")]
        public string MdRenderOnCorruptedItalicText(string line)
        {
            return mdRender.Render(line);
        }

        [TestCase("abc#av", ExpectedResult = "abc#av",
            TestName = "Header_Text_Should_Start_At_Beginning_Paragraph")]
        [TestCase("abcav#", ExpectedResult = "abcav#",
            TestName = "Header_Text_Should_Not_Parse_If_It_Does_Not_Contain_Value")]
        [TestCase("#_#aa", ExpectedResult = "<h1>_#aa</h1>",
            TestName = "Nested_Formatting_Header_Char_Inside_Text_Should_Not_Parse")]
        [TestCase("#__#aa", ExpectedResult = "<h1>__#aa</h1>",
            TestName = "Nested_Formatting_Header_Char_Inside_Text_Should_Not_Parse")]
        [TestCase("__#v_faf__abc", ExpectedResult = "__#v_faf__abc",
            TestName = "Nested_Corrupted_Header_Char_Inside_Text_Should_Not_Affect_Parsing")]
        public string MdRenderOnCorruptedHeaderText(string line)
        {
            return mdRender.Render(line);
        }

        [TestCase("#_", ExpectedResult = "<h1>_</h1>")]
        [TestCase("#__", ExpectedResult = "<h1>__</h1>")]
        [TestCase("##", ExpectedResult = "<h1>#</h1>")]
        [TestCase("_#", ExpectedResult = "_#")]
        [TestCase("__#", ExpectedResult = "__#")]
        [TestCase("_#__", ExpectedResult = "_#__")]
        [TestCase("_#___", ExpectedResult = "<em>#__</em>")]
        [TestCase(@"#\#\#\#\#", ExpectedResult = "<h1>####</h1>")]
        [TestCase(@"_i\_\_\_____", ExpectedResult = "<em>i______</em>")]
        [TestCase(@"__\\#_aa", ExpectedResult = @"__\#_aa")]
        [TestCase(@"\######__", ExpectedResult = @"######__")]
        [TestCase(@"\a__", ExpectedResult = @"\a__")]
        [TestCase(@"\_\_\_\_\_# _a_", ExpectedResult = @"_____# <em>a</em>")]
        [TestCase(@"as#_a s", ExpectedResult = @"as#_a s")]
        [TestCase(@"#_\ a_", ExpectedResult = @"<h1><em>\ a</em></h1>")]
        [TestCase(@"#_\\ a_", ExpectedResult = @"<h1><em>\ a</em></h1>")]
        public string MdRenderOnSpecialCase(string line)
        {
            return mdRender.Render(line);
        }
    }
}
