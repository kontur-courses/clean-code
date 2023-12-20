namespace MarkDownTests;

public class MarkDownTests
{
    private MarkDownEnvironment markDownEnv;
    
    [SetUp]
    public void Setup()
    {
        markDownEnv = new MarkDownEnvironment();
    }

    [TestCase("\n# a", "\n<h1>a</h1>", TestName = "IsCreatedWhenNoNewLineAtTheEnd")]
    [TestCase("\n# a \n", "\n<h1>a </h1>", TestName = "IsCreatedWhenNewLineAtTheEnd")]
    [TestCase("\n# a \n", "\n<h1>a </h1>", TestName = "IsCreatedWhenNewLineAtTheEnd")]
    [TestCase("# a \n", "<h1>a </h1>", TestName = "IsCreatedWhenNoNewLineAtTheStart")]
    [TestCase(" # a \n", " # a \n", TestName = "IsCreatedWhenIncorrectStart")]
    [TestCase("# __a__\n", "<h1><strong>a</strong></h1>", TestName = "SupportsAnotherTagsInsideWhenNewLineAtTheEnd")]
    [TestCase("# __a__", "<h1><strong>a</strong></h1>", TestName = "SupportsAnotherTagsInsideWhenNoNewLineAtTheEnd")]
    public void GenerateHtml_Header(string markdown, string expected)
    {
        MarkDown.MarkDown.GenerateHtml(markdown, markDownEnv)
            .Should()
            .Be(expected);
    }
    
    [TestCase("__a__", "<strong>a</strong>", TestName = "CreatesTag")]
    [TestCase("a__a__ ", "a<strong>a</strong> ", TestName = "CreatesTagInWord")]
    [TestCase("__a", "__a", TestName = "DoesNotCreateTagWithoutCloseSymbol")]
    [TestCase("a __bcd__ e", "a <strong>bcd</strong> e", TestName = "CreatesTagInPhrase")]
    [TestCase("__a __", "__a __", TestName = "DoesNotCreateTagWhenSpaceBeforeClosing")]
    [TestCase("a__b c__", "a__b c__", TestName = "DoesNotCreateTagWhenInDifferentWords")]
    [TestCase("__ a__", "__ a__", TestName = "DoesNotCreateTagWhenSpaceAfterOpening")]
    [TestCase("__a1__", "__a1__", TestName = "CannotCreateWithDigitsNearTag")]
    [TestCase("__a1b__", "__a1b__", TestName = "CannotCreateWithDigitsInsideTag")]
    [TestCase("__a _b_ c__", "<strong>a <em>b</em> c</strong>", TestName = "SupportsEmInside")]
    [TestCase("__a _b c__", "<strong>a _b c</strong>", TestName = "SupportsNotClosedEm")]
    public void GenerateHtml_Strong(string markdown, string expected)
    {
        MarkDown.MarkDown.GenerateHtml(markdown, markDownEnv)
            .Should()
            .Be(expected);
    }
    
    [TestCase("_a_", "<em>a</em>", TestName = "CreatesTag")]
    [TestCase("a_a_ ", "a<em>a</em> ", TestName = "CreatesTagInWord")]
    [TestCase("_a", "_a", TestName = "DoesNotCreateTagWithoutCloseSymbol")]
    [TestCase("a _bcd_ e", "a <em>bcd</em> e", TestName = "CreatesTagInPhrase")]
    [TestCase("_a _", "_a _", TestName = "DoesNotCreateTagWhenSpaceBeforeClosing")]
    [TestCase("a_b c_", "a_b c_", TestName = "DoesNotCreateTagWhenInDifferentWords")]
    [TestCase("_ a_", "_ a_", TestName = "DoesNotCreateTagWhenSpaceAfterOpening")]
    [TestCase("_a1_", "_a1_", TestName = "CannotCreateWithDigitsNearTag")]
    [TestCase("_a1b_", "_a1b_", TestName = "CannotCreateWithDigitsInsideTag")]
    [TestCase("_a __b__ c_", "<em>a __b__ c</em>", TestName = "DoesNotSupportStrongInside")]
    public void GenerateHtml_Em(string markdown, string expected)
    {
        MarkDown.MarkDown.GenerateHtml(markdown, markDownEnv)
            .Should()
            .Be(expected);
    }
    
    [TestCase("__", "__", TestName = "CreateEmptyEmTag")]
    [TestCase("____", "____", TestName = "CreateEmptyStrongTag")]
    [TestCase("__a _b c__ d_", "__a _b c__ d_", TestName = "CreateIntersectingTags")]
    [TestCase("_a __b c_ d__", "_a __b c_ d__", TestName = "CreateIntersectingTagsInAnotherOrder")]
    public void GenerateHtml_DoesNot(string markdown, string expected)
    {
        MarkDown.MarkDown.GenerateHtml(markdown, markDownEnv)
            .Should()
            .Be(expected);
    }
    
    [TestCase("\\_a_", "_a_", TestName = "ScreeningEmTag")]
    [TestCase("\\__a__", "__a__", TestName = "ScreeningStrongTag")]
    [TestCase("\\_a __b__ c_", "_a <strong>b</strong> c_", TestName = "ScreeningWithStrongInsideEm")]
    [TestCase("\\_a __b_ c__", "_a <strong>b_ c</strong>", TestName = "ScreeningIntersection")]
    [TestCase(@"\\__a__", "\\<strong>a</strong>", TestName = "ScreeningOfScreening")]
    [TestCase(@"__a \\_b_ c__", "<strong>a \\<em>b</em> c</strong>", TestName = "ScreeningOfScreeningInsideTags")]
    public void GenerateHtml_Supports(string markdown, string expected)
    {
        MarkDown.MarkDown.GenerateHtml(markdown, markDownEnv)
            .Should()
            .Be(expected);
    }
}