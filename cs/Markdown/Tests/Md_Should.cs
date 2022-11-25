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
    
    [TestCase("_a_", @"\<em>a\</em>", TestName = TestNames.ReplaceItalicTag)]
    [TestCase("__a__", @"\<strong>a\</strong>", TestName = TestNames.ReplaceBoldTag)]
    [TestCase("__a_b_c__", @"\<strong>a\<em>b\</em>c\</strong>", TestName = TestNames.ReplaceItalicTagInsideBoldTag)]
    [TestCase("_a__b__c_", @"\<em>a__b__c\</em>", TestName = TestNames.ReplaceBoldTagInsideItalicTag)]
    [TestCase("_a1_2b", @"_a1_2b", TestName = TestNames.ReplaceTagInsideWordWithNumbersIsNotWorks)]
    [TestCase("_a_b", @"\<em>a\</em>b", TestName = TestNames.ReplaceTagInsideWordIsWorks)]
    [TestCase("a_a b_b", @"a_a b_b", TestName = TestNames.ReplaceTagInDifferentWordsIsNotWorks)]
    [TestCase("a_ b_", @"a_ b_", TestName = TestNames.ReplaceOpenTagSymbolMustBeNotFree)]
    [TestCase("a _b _c", @"a _b _c", TestName = TestNames.ReplaceCloseTagSymbolMustBeNotFree)]
    [TestCase("__a _b__ c_", @"__a _b__ c_", TestName = TestNames.ReplaceIntersectsSymbolsIsNotWorks)]
    [TestCase("____ a __", @"____ a __", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase("_a", "_a", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase("a_", "a_", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase("_a b_c", "_a b_c", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase("a_b c_", "a_b c_", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase("__a _b_ _c_ d__", @"\<strong>a \<em>b\</em> \<em>c\</em> d\</strong>", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase("___a___", @"\<strong>\<em>a\</em>\</strong>", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase("____a____", @"____a____", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase("__a __ b__", @"\<strong>a __ b\</strong>", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase("_a _ b_", @"\<em>a _ b\</em>", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase(@"\_a\_", @"_a_", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase(@"\__a\__", @"__a__", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase(@"\\_a\\_", @"\<em>a\</em>", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase(@"\\__a\\__", @"\<strong>a\</strong>", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase(@"#a", @"\<h1>a\</h1>", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase("a\n#a", "a\n" + @"\<h1>a\</h1>", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase(@"#_a_ __b__", @"\<h1>\<em>a\</em> \<strong>b\</strong>\</h1>", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase("#_a_ \n__b__", @"\<h1>\<em>a\</em>\</h1>" + "\n" + @"\<strong>b\</strong>", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase("#_a_\n__b__", @"\<h1>\<em>a\</em>\</h1>" + "\n" + @"\<strong>b\</strong>", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    [TestCase("$a$", @"\<a>a\</a>", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    public void Render_ValidParams_ShouldBe(string sourceText, string expectationText)
    {
        _markdown
            .Render(sourceText)
            .Should()
            .Be(expectationText);
    }
}
