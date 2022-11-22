using System.Diagnostics;
using System.Text;
using MarkdownRenderer;

namespace MarkdownRenderer_Tests;

[TestFixture]
public class MdRender_PerformanceTests
{
    private Md _markdown = null!;

    [SetUp]
    public void Setup()
    {
        _markdown = new Md();
    }

    [TestCaseSource(nameof(LinearComplexityTestsData))]
    public void RenderWorksLinear_OnAnyContent(Func<int, string> contentGenerator)
    {
        var shortContent = contentGenerator(1000);
        var longContent = contentGenerator(10000);
        (GetRenderExecutionTimeInTicks(longContent) / (double) GetRenderExecutionTimeInTicks(shortContent))
            .Should().BeLessOrEqualTo(1.5d * longContent.Length / shortContent.Length);
    }

    private static readonly TestCaseData[] LinearComplexityTestsData =
    {
        new TestCaseData(CreatePlainTextContent).SetName("Plain text"),
        new TestCaseData(CreateContentWithNestedTags).SetName("Nested tags"),
        new TestCaseData(CreateRandomTagsPlacedContent).SetName("Random placed tags")
    };

    private static string CreatePlainTextContent(int length)
    {
        var content = new StringBuilder(length);
        for (var i = 0; i < 50; i++)
            content.Append(GetRandomLetters().Take(length / 50).ToArray()).Append('\n');

        return content.ToString();
    }

    private static string CreateContentWithNestedTags(int length)
    {
        return new StringBuilder(length + 50)
            .Append("# ")
            .Append(GetRandomLetters().Take(length / 8).ToArray())
            .Append("__")
            .Append(GetRandomLetters().Take(length / 8).ToArray())
            .Append('_')
            .Append(GetRandomLetters().Take(length / 8).ToArray())
            .Append('<')
            .Append(GetRandomLetters().Take(length / 8).ToArray())
            .Append('.')
            .Append(GetRandomLetters().Take(length / 8).ToArray())
            .Append('>')
            .Append(GetRandomLetters().Take(length / 8).ToArray())
            .Append('_')
            .Append(GetRandomLetters().Take(length / 8).ToArray())
            .Append("__")
            .Append(GetRandomLetters().Take(length / 8).ToArray())
            .ToString();
    }

    private static string CreateRandomTagsPlacedContent(int length)
    {
        var tags = new[] {"_", "__", "\\", "\\n", "<", ">", "[", "]", "(", ")"};
        var content = new StringBuilder(length);
        using (var randomLettersEnumerator = GetRandomLetters().GetEnumerator())
        {
            randomLettersEnumerator.MoveNext();
            for (var i = 0; i < length; i++)
            {
                if (Random.Shared.Next(7) == 0)
                    content.Append(tags[Random.Shared.Next(tags.Length)]);
                else
                    content.Append(randomLettersEnumerator.Current);
            }
        }

        return content.ToString();
    }

    private long GetRenderExecutionTimeInTicks(string content)
    {
        const int repetitionCount = 100;

        var stopwatch = new Stopwatch();
        GC.Collect();
        _markdown.Render(content);

        stopwatch.Start();
        for (var i = 0; i < repetitionCount; i++)
            _markdown.Render(content);

        stopwatch.Stop();

        return stopwatch.ElapsedTicks / repetitionCount;
    }

    private static IEnumerable<char> GetRandomLetters()
    {
        while (true)
            yield return (char) Random.Shared.Next('a', 'z' + 1);
    }
}