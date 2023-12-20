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
    [TestCase("# a \n", "<h1>a </h1>", TestName = "IsCreatedWhenNoNewLineAtTheStart")]
    [TestCase(" # a \n", " # a \n", TestName = "IsCreatedWhenIncorrectStart")]
    [TestCase("# __a__\n", "<h1><strong>a</strong></h1>", TestName = "SupportsAnotherTagsInsideWhenNewLineAtTheEnd")]
    [TestCase("# __a__", "<h1><strong>a</strong></h1>", TestName = "SupportsAnotherTagsInsideWhenNoNewLineAtTheEnd")]
    [TestCase("# __a _b_ c__", "<h1><strong>a <em>b</em> c</strong></h1>", TestName = "SupportsMultipleTagsInside")]
    public void GenerateHtml_Header(string markdown, string expected)
    {
        MarkDown.MarkDown.RenderHtml(markdown, markDownEnv)
            .Should()
            .Be(expected);
    }
    
    [TestCase("_a_", "<em>a</em>", TestName = "CreatesEmTag")]
    [TestCase("a_a_ ", "a<em>a</em> ", TestName = "CreatesEmTagInWord")]
    [TestCase("_a", "_a", TestName = "DoesNotCreateEmTagWithoutCloseSymbol")]
    [TestCase("a _bcd_ e", "a <em>bcd</em> e", TestName = "CreatesEmTagInPhrase")]
    [TestCase("_a _", "_a _", TestName = "DoesNotCreateEmTagWhenSpaceBeforeClosing")]
    [TestCase("a_b c_", "a_b c_", TestName = "DoesNotCreateEmTagWhenInDifferentWords")]
    [TestCase("_ a_", "_ a_", TestName = "DoesNotCreateEmTagWhenSpaceAfterOpening")]
    [TestCase("_a1_", "_a1_", TestName = "DoesNotCreateEmWithDigitsNearTag")]
    [TestCase("_a1b_", "_a1b_", TestName = "DoesNotCreateWithDigitsInsideEmTag")]
    [TestCase("_a __b__ c_", "<em>a __b__ c</em>", TestName = "EmDoesNotSupportStrongInside")]
    [TestCase("__a__", "<strong>a</strong>", TestName = "CreatesStrongTag")]
    [TestCase("a__a__ ", "a<strong>a</strong> ", TestName = "CreatesStrongTagInWord")]
    [TestCase("__a", "__a", TestName = "DoesNotCreateStrongTagWithoutCloseSymbol")]
    [TestCase("a __bcd__ e", "a <strong>bcd</strong> e", TestName = "CreatesStrongTagInPhrase")]
    [TestCase("__a __", "__a __", TestName = "DoesNotCreateStrongTagWhenSpaceBeforeClosing")]
    [TestCase("a__b c__", "a__b c__", TestName = "DoesNotCreateStrongTagWhenInDifferentWords")]
    [TestCase("__ a__", "__ a__", TestName = "DoesNotCreateStrongTagWhenSpaceAfterOpening")]
    [TestCase("__a1__", "__a1__", TestName = "DoesNotCreateStrongWithDigitsNearTag")]
    [TestCase("__a1b__", "__a1b__", TestName = "DoesNotCreateStrongWithDigitsInsideTag")]
    [TestCase("__a _b_ c__", "<strong>a <em>b</em> c</strong>", TestName = "StrongSupportsEmInside")]
    [TestCase("__a _b c__", "<strong>a _b c</strong>", TestName = "StrongSupportsNotClosedEm")]
    [TestCase("__", "__", TestName = "DoesNotCreateEmptyEmTag")]
    [TestCase("____", "____", TestName = "DoesNotCreateEmptyStrongTag")]
    [TestCase("__a _b c__ d_", "__a _b c__ d_", TestName = "DoesNotCreateIntersectingTags")]
    [TestCase("_a __b c_ d__", "_a __b c_ d__", TestName = "DoesNotCreateIntersectingTagsInAnotherOrder")]
    public void GenerateHtml(string markdown, string expected)
    {
        MarkDown.MarkDown.RenderHtml(markdown, markDownEnv)
            .Should()
            .Be(expected);
    }
    
    [TestCase("\\_a_", "_a_", TestName = "ScreeningEmTag")]
    [TestCase("\\__a__", "__a__", TestName = "ScreeningStrongTag")]
    [TestCase("\\_a __b__ c_", "_a <strong>b</strong> c_", TestName = "ScreeningEmWithStrongInside")]
    [TestCase("\\_a __b_ c__", "_a <strong>b_ c</strong>", TestName = "ScreeningIntersection")]
    [TestCase(@"\\__a__", "\\<strong>a</strong>", TestName = "ScreeningOfScreening")]
    [TestCase(@"__a \\_b_ c__", "<strong>a \\<em>b</em> c</strong>", TestName = "ScreeningOfScreeningInsideTags")]
    public void GenerateHtml_Supports(string markdown, string expected)
    {
        MarkDown.MarkDown.RenderHtml(markdown, markDownEnv)
            .Should()
            .Be(expected);
    }
    
    [TestCase("- a", "<ul><li>a</li></ul>", TestName = "LisIsCreated")]
    [TestCase("- a\n- b", "<ul><li>a</li><li>b</li></ul>", TestName = "CreatesMultipleLi's")]
    [TestCase("- a\n+ b\n* c ", "<ul><li>a</li><li>b</li><li>c </li></ul>", TestName = "SupportsMultipleLiOpeners")]
    [TestCase("- __a__", "<ul><li><strong>a</strong></li></ul>", TestName = "SupportsTagsInside")]
    [TestCase("- \\__a__", "<ul><li>__a__</li></ul>", TestName = "SupportsScreeningInside")]
    [TestCase("# - a", "<h1>- a</h1>", TestName = "IsNotCreatedInInsideAnotherTag")]
    public void GenerateHtml_Ul(string markdown, string expected)
    {
        MarkDown.MarkDown.RenderHtml(markdown, markDownEnv)
            .Should()
            .Be(expected);
    }
}