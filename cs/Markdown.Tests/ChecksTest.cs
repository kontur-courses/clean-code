using FluentAssertions;
using Markdown.CandidateInfo;
using Markdown.HtmlTools;

namespace Markdown.Tests;

[TestFixture]
public class ChecksTest
{
    [TestCase(-1)]
    [TestCase(4)]
    [TestCase(5)]
    public void IsEscaped_WithUnCorrectPosition(int position)
    {
        var result = Checks.IsEscaped("test", position);
        result.Should().BeFalse();
    }

    [TestCase(@"\\a", 2)]
    [TestCase(@"\\", 0)]
    [TestCase(@"\\\\a", 4)]
    public void IsEscaped_WithEvenNumberEscape(string text, int position)
    {
        var result = Checks.IsEscaped(text, position);
        result.Should().BeFalse();
    }

    [TestCase(@"\\a", 1)]
    [TestCase(@"\\", 1)]
    [TestCase(@"\\\\a", 3)]
    public void IsEscaped_WithOddNumberEscape(string text, int position)
    {
        var result = Checks.IsEscaped(text, position);
        result.Should().BeTrue();
    }

    [TestCase(-1)]
    [TestCase(4)]
    [TestCase(5)]
    public void IsAfterSpace_WithUnCorrectPosition(int position)
    {
        var result = Checks.IsAfterSpace("test", position);
        result.Should().BeFalse();
    }

    [TestCase("test", 0)]
    [TestCase("te t", 3)]
    [TestCase("t  t", 2)]
    public void IsAfterSpace_WithPositionZeroOrWhiteSpace(string text, int position)
    {
        var result = Checks.IsAfterSpace(text, position);
        result.Should().BeTrue();
    }

    [TestCase(-1)]
    [TestCase(4)]
    [TestCase(5)]
    public void IsBeforeSpace_WithUnCorrectPosition(int position)
    {
        var result = Checks.IsBeforeSpace("test", position);
        result.Should().BeFalse();
    }

    [TestCase("test", 3)]
    [TestCase("tes ", 2)]
    [TestCase("t  t", 1)]
    public void IsBeforeSpace_WithPositionMaxIndexTextOrWhiteSpace(string text, int position)
    {
        var result = Checks.IsBeforeSpace(text, position);
        result.Should().BeTrue();
    }

    [Test]
    public void CanSelect_WithCandidateOneAfterOther()
    {
        var open = new Candidate { Position = 1, EdgeType = EdgeType.NotSuitable };
        var close = new Candidate { Position = 2, EdgeType = EdgeType.NotSuitable };
        var result = Checks.CanSelect("test", open, close);

        result.Should().BeTrue();
    }

    [TestCase("123")]
    [TestCase("123 123")]
    [TestCase("123_123")]
    [TestCase(" 123 ")]
    [TestCase("abc123")]
    public void CanSelect_WithCandidateWithDigits(string text)
    {
        var open = new Candidate { Position = 0, EdgeType = EdgeType.NotSuitable };
        var close = new Candidate { Position = text.Length, EdgeType = EdgeType.NotSuitable };
        var result = Checks.CanSelect(text, open, close);

        result.Should().BeFalse();
    }

    [Test]
    public void CanSelect_WithCandidatesOnEdge()
    {
        var open = new Candidate { Position = 1, EdgeType = EdgeType.Edge };
        var close = new Candidate { Position = 3, EdgeType = EdgeType.Edge };
        var result = Checks.CanSelect("test", open, close);

        result.Should().BeTrue();
    }

    [TestCase("test")]
    public void CanSelect_WithCandidatesWithOneWord(string text)
    {
        var open = new Candidate { Position = 0, EdgeType = EdgeType.NotSuitable };
        var close = new Candidate { Position = text.Length, EdgeType = EdgeType.NotSuitable };
        var result = Checks.CanSelect(text, open, close);

        result.Should().BeTrue();
    }

    [TestCase("test ")]
    [TestCase(" test ")]
    [TestCase(" test")]
    [TestCase("hello world")]
    [TestCase("hello world !")]
    [TestCase(" hello world ")]
    [TestCase("hello world hello world")]
    public void CanSelect_WithCandidatesWithFewWords(string text)
    {
        var open = new Candidate { Position = 0, EdgeType = EdgeType.NotSuitable };
        var close = new Candidate { Position = text.Length, EdgeType = EdgeType.NotSuitable };
        var result = Checks.CanSelect(text, open, close);

        result.Should().BeFalse();
    }
}