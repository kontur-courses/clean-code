using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace MarkdownTests;

public class MdTests
{
    private static readonly Random Random = new();

    [TestCase("_abc_", "<em>abc</em>", TestName = "работа тега _")]
    [TestCase("__abc__", "<strong>abc</strong>", TestName = "работа тега __")]
    [TestCase("# abc", "<h1>abc</h1>", TestName ="работа тега #")]
    [TestCase("* abc", "<ul><li>abc</li></ul>", TestName = "работа тега *")]
    [TestCase("* abc\n* abc", "<ul><li>abc</li>\n<li>abc</li></ul>", TestName = "подряд идующие теги * объеденены в один контейнер")]
    [TestCase("* abc\nстрока помешала\n* abc\n* abc", "<ul><li>abc</li></ul>\nстрока помешала\n<ul><li>abc</li>\n<li>abc</li></ul>", TestName = "тег * отделённый от других строкой без * не объеденяется с другими в один контейнер")]
    [TestCase(@"\_abc\_", @"_abc_", TestName = "экранирование знаков разметки оставляет знаки нетронутыми, а сами символы экранирования исчезают")]
    [TestCase(@"abc\abc\ \abc.\", @"abc\abc\ \abc.\", TestName = "знак экранирования исчезает из разметки только если экранирует что-либо")]
    [TestCase(@"\\_abc abc abc_", @"\<em>abc abc abc</em>", TestName = "экранирование знака экранирования")]
    [TestCase(@"abc __abc abc _abc_ abc__ abc.", @"abc <strong>abc abc <em>abc</em> abc</strong> abc.", TestName = "внутри тега __ работает тег _")] 
    [TestCase(@"abc _abc __abc__ abc_ abc.", @"abc <em>abc __abc__ abc</em> abc.", TestName = "внутри тега _ не работает тег __")]
    [TestCase(@"abc_12_3 abc", @"abc_12_3 abc", TestName = "подчерки внутри текста с цифрами не работают")]
    [TestCase(@"_abc_defg abc_def_g abcde_fg._", @"<em>abc</em>defg abc<em>def</em>g abcde<em>fg.</em>", TestName = "подчерки работают внутри слов")]
    [TestCase(@"ab_cde ab_cde", @"ab_cde ab_cde", TestName = "подчерки внутри слов не работают, если находятся в разных словах")]
    [TestCase(@"__abc_ abc", @"__abc_ abc", TestName = "непарные парные теги не работают как выделение")]
    [TestCase(@"abc_ abc_", @"abc_ abc_", TestName = "после открывающего парного тега должен быть непробельный символ")]
    [TestCase(@"_abc _abc", @"_abc _abc", TestName = "перед закрывающим парным тегом должены быть непробельный символ")]
    [TestCase(@"abc __abc _abc__ abc_ abc", @"abc __abc _abc__ abc_ abc", TestName = "при пересечениии разных парных тегов не работают обе пары")]
    [TestCase(@"____", @"____", TestName = "парные теги должны содержать в себе непустую строку")]
    [TestCase(@"# abc __abc _abc_ abc__", @"<h1>abc <strong>abc <em>abc</em> abc</strong></h1>", TestName = "работа тега # с разными тегами внутри")]
    public void Render_ReturnsCorrectString_OnDifferentInput(string input, string expected)
    {
        Md.Render(input).Should().Be(expected);
    }
    
    [Test]
    public void Render_WorksCloseToLinearTime()
    {
        const int baseTextLength = 100;
        const int increasedTextLength = baseTextLength * 10;
        const int iterations = 100;

        var text = GenerateInput(iterations);
        var bigText = GenerateInput(increasedTextLength);

        var expected = Measure(text, iterations);
        var actual = Measure(bigText, iterations);
        var precision = expected * 0.1;
        
        actual.Should().BeCloseTo(expected, precision);
    }

    private static TimeSpan Measure(string text, int count)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();

        var sw = Stopwatch.StartNew();
        for (var i = 0; i < count; i++)
            Md.Render(text);

        return sw.Elapsed / count / text.Length;
    }

    private static string GenerateInput(int length)
    {
        var builder = new StringBuilder(length);
        while (builder.Length < length)
        {
            var text = GetRandomString(8);
            builder.Append(text);
        }

        return builder.ToString();
    }

    private static string GetRandomString(int length)
    {
        var words = new []
        {
            "plaintext",
            "_italictext_",
            "__boldtext__",
            "__dasda_dasd_bvb__",
            "# dasdd",
            "* fasfa",
            "\n",
            " ",
            "\\"
        };
        return words[Random.Next(words.Length)];
    }
}