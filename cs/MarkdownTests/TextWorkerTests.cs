using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class TextWorkerTests
    {
        private static string newLine = Environment.NewLine;
        
        [TestCase("some text", "some text", new char[0],
            TestName = "RemoveShieldsBeforeKeyChars_SimpleText_EqualSimpleText")]
        [TestCase("", "", new char[0])]
        [TestCase(@"\", @"\", new char[0], TestName = "RemoveShieldsBeforeKeyChars_OnlyOneShield_OneShield")]
        [TestCase(@"\", @"\", new[] {'_', '#'},
            TestName = "RemoveShieldsBeforeKeyChars_OnlyOneShieldWithKeyChars_OneShield")]
        [TestCase(@"\\", @"\", new char[0], TestName = "RemoveShieldsBeforeKeyChars_TwoShield_OneShield")]
        [TestCase(@"\\\", @"\\", new char[0], TestName = "RemoveShieldsBeforeKeyChars_ThreeShield_TwoShield")]
        [TestCase(@"\\\\", @"\\", new char[0], TestName = "RemoveShieldsBeforeKeyChars_ThreeShield_TwoShield")]
        [TestCase(@"\\\\", @"\\", new[] {'_', '#'},
            TestName = "RemoveShieldsBeforeKeyChars_ThreeShieldWithKeyChars_TwoShield")]
        [TestCase(@"\_", "_", new[] {'_'}, TestName = "RemoveShieldsBeforeKeyChars_ShieldBeforeKeyChar_KeyChar")]
        [TestCase(@"\\_", @"\_", new[] {'_'},
            TestName = "RemoveShieldsBeforeKeyChars_TwoShieldBeforeKeyChar_OneShieldAndKeyChar")]
        [TestCase(@"\d", @"\d", new char[0],
            TestName = "RemoveShieldsBeforeKeyChars_ShieldBeforeNotKeyChar_EqualString")]
        [TestCase(@"\ _ \ # \ /", @"\ _ \ # \ /", new[] {'_', '#', '/'},
            TestName = "RemoveShieldsBeforeKeyChars_SpaceBetweenKeyCharsAndShield_EqualString")]
        [TestCase("__ ## //", "__ ## //", new[] {'_', '#', '/'},
            TestName = "RemoveShieldsBeforeKeyChars_DoubleKeyChars_EqualString")]
        [TestCase(@"\__ \## \//", "__ ## //", new[] {'_', '#', '/'},
            TestName = "RemoveShieldsBeforeKeyChars_ShieldBeforeDoubleKeyChars_StringWithoutShields")]
        [TestCase(@"#\", @"#\", new[] {'#'}, TestName = "RemoveShieldsBeforeKeyChars_ShieldAfterKeyChar_EqualString")]
        [TestCase(@"|| \\", @"| \\", new char[0], '|',
            TestName = "RemoveShieldsBeforeKeyChars_NotStandardShield_StringWithoutOneShield")]
        public void RemoveShieldsChecker(string input, string expected, IEnumerable<char> keyChars, char shield = '\\')
        {
            TextWorker.RemoveShieldsBeforeKeyChars(input, keyChars, shield).Should().Be(expected);
        }


        [TestCase("text", new[] {"text"}, TestName = "SplitOnParagraphs_StringWithoutParagraphs_InputString")]
        [TestCase("te\r\nxt", new[] {"te", "xt"},
            TestName = "SplitOnParagraphs_StringWithOneParagraph_TwoStringInRightOrder")]
        [TestCase("te\r\nxt\r\n", new[] {"te", "xt", ""},
            TestName = "SplitOnParagraphs_ParagraphCharsLastInLine_LastStringEmpty")]
        [TestCase("", new[] {""}, TestName = "SplitOnParagraphs_EmptyString_OneEmptyString")]
        [TestCase("\r\n", new[] {"", ""}, TestName = "SplitOnParagraphs_OnlyOneParagraphCharsPair_TwoEmptyString")]
        public void SplitOnParagraphsChecker(string input, IEnumerable<string> expected)
        {
            TextWorker.SplitOnParagraphs(input).Should().Equal(expected);
        }
    }
}