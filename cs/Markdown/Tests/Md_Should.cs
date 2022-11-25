using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class Md_Should
{
    private Md _markdown;
    
    [SetUp]
    public void CreateMdInstance()
    {
        _markdown = new Md();
    }
    
    [TestCase("_a_", @"\<em>a\</em>", TestName = "Italic")]
    [TestCase("__a__", @"\<strong>a\</strong>", TestName = "Bold")]
    [TestCase("__a_b_c__", @"\<strong>a\<em>b\</em>c\</strong>", TestName = "Italic in Bold")]
    [TestCase("_a__b__c_", @"\<em>a__b__c\</em>", TestName = "Bold In Italic")]
    [TestCase("_a1_2b", @"_a1_2b", TestName = "Numeric word")]
    [TestCase("_a_b", @"\<em>a\</em>b", TestName = "Half of word")]
    [TestCase("a_a b_b", @"a_a b_b", TestName = "Different words")]
    [TestCase("a_ b_", @"a_ b_", TestName = "Invalid open symbol")]
    [TestCase("a _b _c", @"a _b _c", TestName = "Invalid close symbol")]
    [TestCase("__a _b__ c_", @"__a _b__ c_", TestName = "Intersects symbols")]
    [TestCase("____ a __", @"____ a __", TestName = "Symbols in sides of empty string")]
    [TestCase("_a", "_a", TestName = "Lonely open symbol")]
    [TestCase("a_", "a_", TestName = "Lonely close symbol")]
    [TestCase("_a b_c", "_a b_c", TestName = "Different words with open symbol in start")]
    [TestCase("a_b c_", "a_b c_", TestName = "Different words with close symbol in end")]
    [TestCase("__a _b_ _c_ d__", @"\<strong>a \<em>b\</em> \<em>c\</em> d\</strong>", TestName = "Multiply italic in bold")]
    [TestCase("___a___", @"\<strong>\<em>a\</em>\</strong>", TestName = "Triple symbol")]
    [TestCase("____a____", @"____a____", TestName = "Quad symbol")]
    [TestCase("__a __ b__", @"\<strong>a __ b\</strong>", TestName = "Bold with lonely tag in middle")]
    [TestCase("_a _ b_", @"\<em>a _ b\</em>", TestName = "Italic with lonely tag in middle")]
    [TestCase(@"\_a\_", @"_a_", TestName = "Screen italic")]
    [TestCase(@"\__a\__", @"__a__", TestName = "Screen bold")]
    [TestCase(@"\\_a\\_", @"\<em>a\</em>", TestName = "Double screen italic")]
    [TestCase(@"\\__a\\__", @"\<strong>a\</strong>", TestName = "Double screen bold")]
    [TestCase(@"#a", @"\<h1>a\</h1>", TestName = "Title")]
    [TestCase("a\n#a", "a\n" + @"\<h1>a\</h1>", TestName = "Title in not a first line")]
    [TestCase(@"#_a_ __b__", @"\<h1>\<em>a\</em> \<strong>b\</strong>\</h1>", TestName = "Title with other symbols inside")]
    [TestCase("#_a_ \n__b__", @"\<h1>\<em>a\</em>\</h1>" + "\n" + @"\<strong>b\</strong>", TestName = "Title with other symbols inside in not a first line")]
    [TestCase("#_a_\n__b__", @"\<h1>\<em>a\</em>\</h1>" + "\n" + @"\<strong>b\</strong>", TestName = "Title with other symbols inside in not a first line")]
    [TestCase(@"\#a", @"#a", TestName = "Screen title")]
    [TestCase(@"\\#a", @"\<h1>a\</h1>", TestName = "Double screen title")]
    public void Render_ValidParams_ShouldBe(string sourceText, string expectationText)
    {
        _markdown
            .Render(sourceText, TagConfigurations.TagsConfigurations)
            .Should()
            .Be(expectationText);
    }
    
    [TestCase(null, TestName = "Null string should throw exception")]
    [TestCase("", TestName = "Empty string should throw exception")]
    public void Render_InvalidParams_ShouldBeThrowArgumentException(string sourceText)
    {
        var action = () => _markdown
            .Render(sourceText, TagConfigurations.TagsConfigurations);
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Invalid string for parse");
    }
}
