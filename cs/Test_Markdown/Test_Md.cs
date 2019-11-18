using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Test_Markdown
{
    [TestFixture]
    public class Test_Md
    {
        [TestCase("Just a string", "Just a string",
            TestName = "InputString_IfStringWithoutAnyControlChar")]
        [TestCase("_Pair of Underscore_", "<em>Pair of Underscore</em>",
            TestName = "StringWithTagEM_IfStringContainPairOfUnderscore")]
        [TestCase("\\_Screen control symbol\\_", "_Screen control symbol_",
            TestName = "InputString_IfControlSymbolAreScreened")]
        [TestCase("__Pair of Double Underscore__", "<strong>Pair of Double Underscore</strong>",
            TestName = "StringWithTagStrong_IfStringContainsPairOfDoubleUnderscore")]
        [TestCase("_Unpaired underscore is not tag__", "_Unpaired underscore is not tag__", TestName =
            "StringWithoutTag_IfStringContainsUnpairedUnderscore")]
        [TestCase("_Underscore after space is not close of tag _((", "_Underscore after space is not close of tag _((",
            TestName = "StringWithoutTag_IfEndUnderscoreStayAfterSpace")]
        [TestCase("__Underscore _work_ in double underscore__",
            "<strong>Underscore <em>work</em> in double underscore</strong>",
            TestName = "StringWithTag_IfDoubleUnderscoreContainsOneUnderscorePair")]
        [TestCase("_Double underscore __dont work__ inside one_", "<em>Double underscore __dont work__ inside one</em>",
            TestName = "StringInOneTag_IfOneUnderscoreContainsDoubleUnderscore")]
        [TestCase("_Same underscore _hide in their analog_", "<em>Same underscore hide in their analog</em>", 
            TestName = "StringWithoutInnerUnderscore_IfUnderscoreContainsInUnderScore")]
        [TestCase("_First priority _have first_ underscore_", "<em>First priority have first</em> underscore_", 
            TestName = "StringWithTagOnBegin_IfHaveSeveralPairOfUnderscore")]
        [TestCase("_Skip screened\\_ underscore_", "<em>Skip screened_ underscore</em>", 
            TestName = "StringInTagWithUnderscore_IfUnderscoreWasScreenedInAnotherUnderscore")]
        public void RenderShouldReturn(string input, string excepted)
        {
            var actual = Md.Render(input);

            actual.Should().Be(excepted);
        }
    }
}