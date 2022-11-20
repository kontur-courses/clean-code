using FluentAssertions;
using Markdown.Tests;
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
    
    [TestCase("Hello, _world_!", @"Hello, \<em>world\</em>!", TestName = TestNames.ReplaceItalicTag)]
    [TestCase("Hello, __world__!", @"Hello, \<strong>world\</strong>!", TestName = TestNames.ReplaceBoldTag)]
    [TestCase("Some __text for _test_ logic__", @"Some \<strong>text for \<em>test\</em> logic\</strong>", TestName = TestNames.ReplaceItalicTagInsideBoldTag)]
    [TestCase("Some _text for __test__ logic_", @"Some \<em>text for __test__ logic\</em>", TestName = TestNames.ReplaceBoldTagInsideItalicTag)]
    [TestCase("Some _tex1_2t", @"Some _tex1_2t", TestName = TestNames.ReplaceTagInsideWordWithNumbersIsNotWorks)]
    [TestCase("Some _tex_t", @"Some \<em>tex\</em>t", TestName = TestNames.ReplaceTagInsideWordIsWorks)]
    [TestCase("So_me tex_t", @"So_me tex_t", TestName = TestNames.ReplaceTagInDifferentWordsIsNotWorks)]
    [TestCase("Some_ text_ for test", @"Some_ text_ for test", TestName = TestNames.ReplaceOpenTagSymbolMustBeNotFree)]
    [TestCase("Some _text for _test", @"Some text_ for _test", TestName = TestNames.ReplaceCloseTagSymbolMustBeNotFree)]
    [TestCase("Some __text _for__ test_", @"Some __text _for__ test_", TestName = TestNames.ReplaceIntersectsSymbolsIsNotWorks)]
    [TestCase("Some text ____ for __ test", @"Some text ____ for __ test", TestName = TestNames.ReplaceSymbolsOnTheSidesOfEmptyStringIsNotWorks)]
    public void Render_ValidParams_ShouldBe(string sourceText, string expectationText)
    {
        _markdown
            .Render(sourceText)
            .Should()
            .Be(expectationText);
    }
}
