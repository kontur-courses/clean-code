using System.Text;
using FluentAssertions.Extensions;
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

    [Test]
    public void RenderWorksFast_OnLinesWithoutTags()
    {
        var content = new StringBuilder(10000);
        for (var i = 0; i < 100; i++)
        {
            content.Append(GetRandomLetters().Take(100).ToArray()).Append('\n');
        }

        TestExecutionTime(content.ToString());
    }

    [Test]
    public void RenderWorksFast_OnLineWithNestedTags()
    {
        var content = new StringBuilder(10050)
            .Append("# ")
            .Append(GetRandomLetters().Take(1500).ToArray())
            .Append("__")
            .Append(GetRandomLetters().Take(1000).ToArray())
            .Append('_')
            .Append(GetRandomLetters().Take(1000).ToArray())
            .Append('<')
            .Append(GetRandomLetters().Take(1500).ToArray())
            .Append('.')
            .Append(GetRandomLetters().Take(1500).ToArray())
            .Append('>')
            .Append(GetRandomLetters().Take(1000).ToArray())
            .Append('_')
            .Append(GetRandomLetters().Take(1000).ToArray())
            .Append("__")
            .Append(GetRandomLetters().Take(1500).ToArray());

        TestExecutionTime(content.ToString());
    }

    [Test]
    public void RenderWorksFast_OnLinesWithRandomPlacedTags()
    {
        var tags = new[] {"_", "__", "\\", "\\n", "<", ">", "[", "]", "(", ")"};
        var content = new StringBuilder(10000);
        using (var randomLettersEnumerator = GetRandomLetters().GetEnumerator())
        {
            randomLettersEnumerator.MoveNext();
            for (var i = 0; i < 10000; i++)
            {
                if (Random.Shared.Next(5) == 0)
                    content.Append(tags[Random.Shared.Next(tags.Length)]);
                else
                    content.Append(randomLettersEnumerator.Current);
            }
        }

        TestExecutionTime(content.ToString());
    }

    private void TestExecutionTime(string content, int maxDurationInMilliseconds = 100) =>
        _markdown.ExecutionTimeOf(md => md.Render(content)).Should()
            .BeLessOrEqualTo(maxDurationInMilliseconds.Milliseconds());

    private static IEnumerable<char> GetRandomLetters()
    {
        while (true)
            yield return (char) Random.Shared.Next('a', 'z' + 1);
    }
}