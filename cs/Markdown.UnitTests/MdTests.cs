using FluentAssertions;

namespace Markdown.UnitTests;

[TestFixture]
public class MdTests
{
    [SetUp]
    public void Setup()
    {
        mdHtml = Md.Html();
    }

    private Md mdHtml = null!;

    [TestCase("", "")]
    [TestCase("\n", "")]
    [TestCase("\r\n", "")]
    [TestCase("\r", "")]
    [TestCase("abc", "<p>abc</p>")]
    [TestCase(@"abc
abc", "<p>abc</p><p>abc</p>")]
    [TestCase(@"_abc_
abc", "<p><em>abc</em></p><p>abc</p>")]
    [TestCase(@"_abc_
abc
abc_12_3", "<p><em>abc</em></p><p>abc</p><p>abc_12_3</p>")]
    [TestCase(@"_abc
abc", "<p>_abc</p><p>abc</p>")]
    [TestCase(@"_a_b_c
a_bc", "<p><em>a</em>b_c</p><p>a_bc</p>")]
    [TestCase("_abc_", "<p><em>abc</em></p>")]
    [TestCase("_abc_ abc", "<p><em>abc</em> abc</p>")]
    [TestCase("a_b_c abc", "<p>a<em>b</em>c abc</p>")]
    [TestCase("ab_c_ abc", "<p>ab<em>c</em> abc</p>")]
    [TestCase("abc_12_3 abc", "<p>abc_12_3 abc</p>")]
    [TestCase("ab_c a_bc", "<p>ab_c a_bc</p>")]
    [TestCase("__abc__", "<p><strong>abc</strong></p>")]
    [TestCase("__abc__ abc", "<p><strong>abc</strong> abc</p>")]
    [TestCase("__abc__\nabc", "<p><strong>abc</strong></p><p>abc</p>")]
    [TestCase("__abc__ _abc_ abc", "<p><strong>abc</strong> <em>abc</em> abc</p>")]
    [TestCase("a__b__c abc", "<p>a<strong>b</strong>c abc</p>")]
    [TestCase("ab__c__ abc", "<p>ab<strong>c</strong> abc</p>")]
    [TestCase("__a__bc abc", "<p><strong>a</strong>bc abc</p>")]
    [TestCase("abc__12__3 abc", "<p>abc__12__3 abc</p>")]
    [TestCase("ab__c a__bc", "<p>ab__c a__bc</p>")]
    [TestCase("__abc__ __a_b_c abc__", "<p><strong>abc</strong> <strong>a<em>b</em>c abc</strong></p>")]
    [TestCase("__abc_", "<p>__abc_</p>")]
    [TestCase("# abc", "<h1>abc</h1>")]
    [TestCase("# __abc__", "<h1><strong>abc</strong></h1>")]
    [TestCase("# __abc__ abc", "<h1><strong>abc</strong> abc</h1>")]
    [TestCase(@"# __abc__
abc", "<h1><strong>abc</strong></h1><p>abc</p>")]
    [TestCase("# __abc__ _abc_ abc", "<h1><strong>abc</strong> <em>abc</em> abc</h1>")]
    [TestCase(@"# __abc_
abc", "<h1>__abc_</h1><p>abc</p>")]
    [TestCase(@"# __abc__
abc", "<h1><strong>abc</strong></h1><p>abc</p>")]
    [TestCase(@"# __abc__ _abc_
abc", "<h1><strong>abc</strong> <em>abc</em></h1><p>abc</p>")]
    [TestCase(@"# __abc__
_abc_", "<h1><strong>abc</strong></h1><p><em>abc</em></p>")]
    [TestCase(@"# __abc__
_abc_ abc", "<h1><strong>abc</strong></h1><p><em>abc</em> abc</p>")]
    public void Render_ExpectedHtml_InputMd(string inputMd, string expectedHtml)
    {
        var actualHtml = mdHtml.Render(inputMd);

        actualHtml.Should().BeEquivalentTo(expectedHtml);
    }
}