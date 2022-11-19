using System.Globalization;
using MarkdownRenderer;

// ReSharper disable StringLiteralTypo

namespace MarkdownRenderer_Tests;

public class MdRender_Should
{
    private Md _markdown = null!;

    [SetUp]
    public void Setup()
    {
        _markdown = new Md();
    }

    [TestCase("", TestName = "Empty line")]
    [TestCase("\n\n\n", TestName = "Empty multiline")]
    [TestCase("abcd", TestName = "Simple text")]
    [TestCase("ab\ncd\nef", TestName = "Simple multiline text")]
    public void ReturnTheSame_OnSourceWithoutTags(string source)
    {
        _markdown.Render(source).Should().Be(source);
    }

    [TestCaseSource(nameof(BuildDefaultInlineTestCaseDataSource), new object[] {"_", "em", "italic"})]
    public void ReturnCorrectRenderResult_OnItalicTags(string source, string expected
    )
    {
        _markdown.Render(source).Should().Be(expected);
    }

    [TestCaseSource(nameof(BuildDefaultInlineTestCaseDataSource), new object[] {"__", "strong", "strong"})]
    public void ReturnCorrectRenderResult_OnStrongTags(string source, string expected)
    {
        _markdown.Render(source).Should().Be(expected);
    }

    [TestCase("<ab.cd>", @"<a href=""ab.cd"">ab.cd</a>", TestName = "Simple link")]
    [TestCase("<ab.cd/ef>", @"<a href=""ab.cd/ef"">ab.cd/ef</a>", TestName = "Two level link")]
    [TestCase("<a b.cd>", "<a b.cd>", TestName = "Incorrect destination")]
    public void ReturnCorrectResult_OnSimpleLinkTags(string source, string expected)
    {
        _markdown.Render(source).Should().Be(expected);
    }

    [TestCase("[abc](ab.cd)", @"<a href=""ab.cd"">abc</a>", TestName = "Titled link")]
    [TestCase("[abc](ab.cd/ef)", @"<a href=""ab.cd/ef"">abc</a>", TestName = "Two level titled link")]
    [TestCase("[a b c](ab.cd)", @"<a href=""ab.cd"">a b c</a>", TestName = "Title with spaces")]
    [TestCase("[](ab.cd)", @"<a href=""ab.cd""></a>", TestName = "Empty title")]
    [TestCase("[abc]]()", "[abc]]()", TestName = "Empty destination")]
    [TestCase("[abc](a b.cd)", "[abc](a b.cd)", TestName = "Incorrect destination")]
    [TestCase("[abc] (ab.cd)", "[abc] (ab.cd)", TestName = "Space between detination and title")]
    [TestCase("[abc]](ab.cd)", "[abc]](ab.cd)", TestName = "Bad braces placing")]
    public void ReturnCorrectResult_OnTitledLinkTags(string source, string expected)
    {
        _markdown.Render(source).Should().Be(expected);
    }

    [TestCase("__ab _cd_ ef__", "<strong>ab <em>cd</em> ef</strong>", TestName = "Italic inside strong")]
    [TestCase("_ab __cd__ ef_", "<em>ab __cd__ ef</em>", TestName = "Strong inside italic")]
    [TestCase("_<ab.cd>_", @"<em><a href=""ab.cd"">ab.cd</a></em>", TestName = "Simple link inside italic")]
    [TestCase("__<ab.cd>__", @"<strong><a href=""ab.cd"">ab.cd</a></strong>", TestName = "Simple link inside strong")]
    [TestCase("_[abc](ab.cd)_", @"<em><a href=""ab.cd"">abc</a></em>", TestName = "Titled link inside italic")]
    [TestCase("__[a](ab.cd)__", @"<strong><a href=""ab.cd"">a</a></strong>", TestName = "Titled link inside strong")]
    [TestCase("__abc_", "__abc_", TestName = "Not paired tags")]
    [TestCase("__ab _cd__ ef_", "__ab _cd__ ef_", TestName = "Italic strong intersections")]
    public void ReturnCorrectRenderResult_OnCombinedInlineTags(string source, string expected)
    {
        _markdown.Render(source).Should().Be(expected);
    }

    [TestCase("# abc", "<h1>abc</h1>", TestName = "Only header")]
    [TestCase("#abc", "#abc", TestName = "Headers without space after tag")]
    [TestCase(" # abc", " # abc", TestName = "Header with space before tag")]
    [TestCase("# abc __def _ghi_ jkl__", "<h1>abc <strong>def <em>ghi</em> jkl</strong></h1>",
        TestName = "Header with nested inline elements")]
    [TestCase("# abc\ndef\n# ghi", "<h1>abc</h1>\ndef\n<h1>ghi</h1>", TestName = "Headers and paragraphs mix")]
    public void ReturnCorrectRenderResult_OnHeaderTags(string source, string expected)
    {
        _markdown.Render(source).Should().Be(expected);
    }

    [TestCase("\\_abc\\_", "_abc_", TestName = "Italic escaped")]
    [TestCase("\\__abc\\__", "__abc__", TestName = "Strong escaped")]
    [TestCase("\\<abc.ru>", "<abc.ru>", TestName = "Simple link escaped")]
    [TestCase("\\[abc](abc.ru)", "[abc](abc.ru)", TestName = "Titled link escaped")]
    [TestCase("\\\\_abc_", "\\<em>abc</em>", TestName = "Self escaped")]
    [TestCase("ab\\c\\d", "ab\\c\\d", TestName = "Not disapper if nothing escape")]
    [TestCase("\\# abc", "# abc", TestName = "Header escape")]
    [TestCase("\\#abc", "\\#abc", TestName = "Not disapper if header tag without space")]
    [TestCase("\\\\\\_abc_", "\\_abc_", TestName = "Tripple escape")]
    public void ReturnCorrectRenderResult_OnEscapeCharacters(string source, string expected)
    {
        _markdown.Render(source).Should().Be(expected);
    }

    private static IEnumerable<TestCaseData> BuildDefaultInlineTestCaseDataSource(string sourceTag, string resultTag,
        string tagName)
    {
        var lowerTagName = tagName.ToLower();
        var titleTagName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lowerTagName);

        yield return new TestCaseData($"{sourceTag}{sourceTag}", $"{sourceTag}{sourceTag}")
            .SetName($"Empty {lowerTagName} tags");

        yield return new TestCaseData($"{sourceTag}abcd{sourceTag}", $"<{resultTag}>abcd</{resultTag}>")
            .SetName($"{titleTagName} one word");

        yield return new TestCaseData($"{sourceTag}ab cd ef{sourceTag}", $"<{resultTag}>ab cd ef</{resultTag}>")
            .SetName($"Some words inside {lowerTagName}");

        yield return new TestCaseData($"ab {sourceTag}cd{sourceTag} ef", $"ab <{resultTag}>cd</{resultTag}> ef")
            .SetName($"{titleTagName} inside plain text");

        yield return new TestCaseData($"a{sourceTag}bc{sourceTag}d", $"a<{resultTag}>bc</{resultTag}>d")
            .SetName($"{titleTagName} inside word");

        yield return new TestCaseData($"{sourceTag}ab cd{sourceTag}ef", $"{sourceTag}ab cd{sourceTag}ef")
            .SetName($"{titleTagName} inside word with spaces");

        yield return new TestCaseData($"a{sourceTag}b2c{sourceTag}d", $"a{sourceTag}b2c{sourceTag}d")
            .SetName($"{titleTagName} inside word with digits");

        yield return new TestCaseData($" {sourceTag}x{sourceTag} ", $" <{resultTag}>x</{resultTag}> ")
            .SetName($"{titleTagName} with spaces before and after");

        yield return new TestCaseData(
                $"{sourceTag}a {sourceTag}b c{sourceTag} d{sourceTag}",
                $"<{resultTag}>a {sourceTag}b c</{resultTag}> d{sourceTag}")
            .SetName($"{titleTagName} tags intersections");

        yield return new TestCaseData($"{sourceTag}ab\ncd{sourceTag}", $"{sourceTag}ab\ncd{sourceTag}")
            .SetName($"{titleTagName} tag pair on other line");

        yield return new TestCaseData(
                $"{sourceTag}abcd{sourceTag}\n{sourceTag}efgh{sourceTag}",
                $"<{resultTag}>abcd</{resultTag}>\n<{resultTag}>efgh</{resultTag}>")
            .SetName($"{titleTagName} multiline");
    }
}