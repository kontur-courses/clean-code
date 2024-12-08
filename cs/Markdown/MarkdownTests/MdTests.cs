using FluentAssertions;
using Markdown;
using System.Diagnostics;
using System.Text;

namespace MarkdownTests;

[TestFixture]
public partial class MdTests
{
    private Md md;

    [SetUp]
    public void SetUp()
    {
        var converters = new List<HtmlTokenConverter>()
        {
            new BoldTokenHtmlConverter(),
            new ItalicTokenHtmlConverter(),
            new HeaderTokenHtmlConverter(),
            new TextTokenHtmlConverter(),
            new LinkTokenHtmlConverter(),
        };

        var parsers = new List<TokenParser>()
        {
            new BoldTokenParser(),
            new ItalicTokenParser(),
            new HeaderTokenParser(),
            new LinkTokenParser(),
        };

        var converter = new MarkdownToHtmlConverter(converters);
        var parser = new MarkdownParser(parsers);

        md = new(converter, parser);
    }

    [Test]
    public void Md_ConvertOneTag()
    {
        var text = "_italic_";
        var expectedResult = "<em>italic</em>";

        var result = md.Render(text);

        result.Should().Be(expectedResult);
    }

    [TestCaseSource(nameof(conversionTestCases))]
    public void Md_ConvertCorrecty(string text, string expectedResult)
    {
        var result = md.Render(text);

        result.Should().Be(expectedResult);
    }

    [Test]
    public void Md_WorkInLinearComplexity()
    { 
        var smallText = GenerateMarkdownText(100);
        var bigText = GenerateMarkdownText(1000);
        var smallTextTime = GetTimeForOneSymbol(smallText, 200);
        var bigTextTime = GetTimeForOneSymbol(bigText, 200);
        var precision = smallTextTime * 0.5;

        bigTextTime.Should().BeCloseTo(smallTextTime, new(0, 0, 0, 0, 200));
    }

    private TimeSpan GetTimeForOneSymbol(string text, int count)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        var sw = new Stopwatch();
        sw.Start();
        for (var i = 0; i < count; i++) 
            md.Render(text);
        sw.Stop();

        return sw.Elapsed / count / text.Length;
    }

    private string GenerateMarkdownText(int size)
    {
        string[] baseTexts = ["__bold _italic_ bold__", "# header __b _i_ b__"];
        var result = new StringBuilder();
        var rnd = new Random();

        while (true)
        {
            var text = baseTexts[rnd.Next(baseTexts.Length)];
            if (result.Length + text.Length < size)
                result.Append(text);
            else
                break;
        }

        result.Append('a', size - result.Length);

        return result.ToString();
    }
}
