using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown;

namespace MarkDownTests;

[TestFixture]
[TestOf(typeof(IMarkDown))]
public class MdTests
{
    private IMarkDown sut = new Md();
    private static Random random = new();
    private const string Chars = " ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789\n";

    [Test]
    public void Should_Throw_WhenInputNull()
    {
        Action action = () => sut.Render(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestCase(@"\# abc", ExpectedResult = "# abc")]
    [TestCase(@"\_abc_", ExpectedResult = "_abc_")]
    [TestCase(@"_abc\_", ExpectedResult = "_abc_")]
    public string Should_IgnoreEscapedTags(string text) => sut.Render(text);

    [TestCase("# abc", ExpectedResult = "<h1>abc</h1>")]
    [TestCase("_abc_", ExpectedResult = "<em>abc</em>")]
    [TestCase("__abc__", ExpectedResult = "<strong>abc</strong>")]
    [TestCase("[abc](https://abc.abc)", ExpectedResult = "<a href=\"https://abc.abc\">abc</a>")]
    public string Should_SupportDifferentTags(string text) => sut.Render(text);

    [TestCase("# abc _abc_", ExpectedResult = "<h1>abc <em>abc</em></h1>")]
    public string Should_RenderNestedTags(string text) => sut.Render(text);

    [TestCase("")]
    [TestCase("__")]
    [TestCase("__abc")]
    [TestCase("_how are you _")]
    [TestCase("_ how are you_")]
    [TestCase("_ how are you _")]
    [TestCase("#how are you")]
    [TestCase("__ how are _you __ i am_")]
    [TestCase("__ how are _you __ i am_")]
    public void Should_IgnoreInvalidTags(string text)
    {
        var rendered = sut.Render(text);

        rendered.Should().Be(text);
    }

    [TestCase("# how are you", ExpectedResult = "<h1>how are you</h1>")]
    [TestCase("asd\n# how are you", ExpectedResult = "asd\n<h1>how are you</h1>")]
    [TestCase("asd\n\\# how are you", ExpectedResult = "asd\n# how are you")]
    [TestCase("asd\n# how are you \n abc", ExpectedResult = "asd\n<h1>how are you</h1>\n abc")]
    [TestCase("__how _are_ you__", ExpectedResult = "<strong>how <em>are</em> you</strong>")]
    public string Should_RenderCorrect(string text) => sut.Render(text);

    [Test]
    public void Should_WorkLinearly()
    {
        const int baseTextLength = 100;
        const int increasedTextLength = baseTextLength * 10;
        const int iterations = 100;

        var text = GenerateInput(iterations);
        var bigText = GenerateInput(increasedTextLength);

        var expected = Measure(text, iterations);
        var actual = Measure(bigText, iterations);

        var allowedDeviation = expected * 0.4;
        Assert.That(actual, Is.EqualTo(expected).Within(allowedDeviation));
    }

    private TimeSpan Measure(string text, int count)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();

        var sw = Stopwatch.StartNew();
        for (var i = 0; i < count; i++)
            sut.Render(text);

        return sw.Elapsed / count / text.Length;
    }

    private static string GenerateInput(int length)
    {
        const int wordsLength = 6;
        var tags = new[] { "_", "__", "#", "[", "]", "(", ")" };
        var builder = new StringBuilder(length);
        var index = 0;
        while (index < length)
        {
            AddRandomTag(builder, tags, ref index);

            var part = GetRandomString(wordsLength);
            builder.Append(part);
            index += part.Length;

            AddRandomTag(builder, tags, ref index);
        }

        return builder.ToString();
    }

    private static void AddRandomTag(StringBuilder builder, string[] tags, ref int index)
    {
        if (random.Next(2) == 0)
        {
            builder.Append(tags[random.Next(tags.Length)]);
            index++;
        }
    }

    private static string GetRandomString(int length) =>
        new(Enumerable.Repeat(Chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
}