using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MarkdownConverterTests
{
    MarkdownToHtmlConverter converter;

    [SetUp]
    public void Setup()
    {
        var lexer = new MarkdownLexer();
        var parser = new MarkdownParser();
        converter = new MarkdownToHtmlConverter(lexer, parser);
    }

    [Test]
    public void ConvertMarkdownToHtml()
    {
        var md = "# title";
        var expected = "<h1>title</h1>";
        var actual = converter.Convert(md);
        actual.Should().Be(expected);
    }

    [TestCase("# header", "<h1>header</h1>")]
    [TestCase("_italic_", "<em>italic</em>")]
    [TestCase("ita_lic_", "ita<em>lic</em>")]
    [TestCase("__strong__", "<strong>strong</strong>")]
    [TestCase("st__rong__", "st<strong>rong</strong>")]
    [TestCase("___text___", "<strong><em>text</em></strong>")]
    [TestCase("__text _text_ text__", "<strong>text <em>text</em> text</strong>")]
    [TestCase("# header\n new line", "<h1>header</h1>\n new line")]
    [TestCase(@"\n\_Вот это\_", @"\n_Вот это_")]
    [TestCase("line with _italic_ text", "line with <em>italic</em> text")]
    [TestCase("a _t_ b", "a <em>t</em> b")]
    [TestCase("line with __strong__ text", "line with <strong>strong</strong> text")]
    [TestCase("line with __text _text_ text__ abc", "line with <strong>text <em>text</em> text</strong> abc")]
    [TestCase("# Header 1\n ___Dear Diary___, today has been a _hard_ day",
        "<h1>Header 1</h1>\n <strong><em>Dear Diary</em></strong>, today has been a <em>hard</em> day")]
    [TestCase("# _Header_ 1\n ___Dear Diary___, today has been a _hard_ day",
        "<h1><em>Header</em> 1</h1>\n <strong><em>Dear Diary</em></strong>, today has been a <em>hard</em> day")]
    public void MdRender_ReturnsExpectedHtml(string md, string expected)
    {
        var actual = converter.Convert(md);
        actual.Should().Be(expected);
    }

    [Test]
    public void ConvertMarkdownToHtml_ConformsToSpecification()
    {
        var dir = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestsData");
        var expectedPath = Path.Combine(dir, "expected.txt");
        var md = File.ReadAllText(Path.Combine(dir, "test.txt"));
        
        var expected = File.ReadAllText(expectedPath);
        var actual = converter.Convert(md);

        actual.Should().Be(expected);
    }

    [Test]
    public void Convert_ShouldPerformInLinearTime()
    {
        const int smallInputSize = 1000;
        const int largeInputSize = 100000;

        var smallInput = GenerateMarkdownInput(smallInputSize);
        var largeInput = GenerateMarkdownInput(largeInputSize);

        var smallTime = MeasureExecutionTime(() => converter.Convert(smallInput));
        var largeTime = MeasureExecutionTime(() => converter.Convert(largeInput));

        var growthFactor = (double)largeTime / smallTime;
        growthFactor.Should().BeLessThan(largeInputSize / smallInputSize * 1.5, "execution time should grow linearly with the size of the input");
    }

    private string GenerateMarkdownInput(int size)
    {
        var mdBuilder = new StringBuilder(size);
        for (var i = 0; i < size; i++)
        {
            mdBuilder.Append("# Heading\n");
            mdBuilder.Append("**Bold text**\n");
            mdBuilder.Append("*Italic text*\n");
        }

        return mdBuilder.ToString();
    }

    private long MeasureExecutionTime(Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        return stopwatch.ElapsedMilliseconds;
    }
}