using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace Markdown.UnitTests;

[TestFixture]
public class MdTests
{
    [SetUp]
    public void Setup()
    {
        traceFileName = Path.GetTempFileName();
        streamWriter = new(traceFileName);
        tracer = new(streamWriter);
        mdHtml = Md.Html(tracer);
    }

    [TearDown]
    public void TearDown()
    {
        var currentContext = TestContext.CurrentContext;
        
        streamWriter.Dispose();
        var traceFileInfo = new FileInfo(traceFileName);
        if (currentContext.Result.Outcome.Status == TestStatus.Failed && traceFileInfo.Length < 1_000_000_000)
        {

            var failedTestDirectory = Path.Combine(currentContext.TestDirectory, "failed-tests-md");
            var failedTestFileName = Path.Combine(failedTestDirectory,
                $"{currentContext.Test.MethodName}-{currentContext.Test.ID}-{Random.Shared.Next(100, 1000)}.log");

            Directory.CreateDirectory(failedTestDirectory);
            File.Copy(traceFileName, failedTestFileName);
            Console.Error.WriteLine($"Log for this failed test located at {failedTestFileName}");
        }
        traceFileInfo.Delete();
    }

    private Md mdHtml = null!;
    private string traceFileName = null!;
    private StreamWriter streamWriter = null!;
    private DebugTracer tracer = null!;

    [TestCase("", "")]
    [TestCase("\n", "")]
    [TestCase("\r\n", "")]
    [TestCase("\r", "")]

    //Paragraph
    [TestCase("abc", "<p>abc</p>")]
    [TestCase(@"abc
abc", "<p>abc</p><p>abc</p>")]
    [TestCase(@"_abc_
abc", "<p><em>abc</em></p><p>abc</p>")]
    [TestCase(@"_abc_
abc
abc_12_3", "<p><em>abc</em></p><p>abc</p><p>abc_12_3</p>")]
    [TestCase(@"_abc
abc", "<p>_abc</p><p>abc</p>")]
    [TestCase(@"_a_b_c
a_bc", "<p><em>a</em>b_c</p><p>a_bc</p>")]
    [TestCase("\\__abc_\\_", "<p>_<em>abc</em>_</p>")]
    [TestCase("__\\_abc\\___", "<p><strong>_abc_</strong></p>")]
    [TestCase("_abc_", "<p><em>abc</em></p>")]
    [TestCase("_abc_ abc", "<p><em>abc</em> abc</p>")]
    [TestCase("a_b_c abc", "<p>a<em>b</em>c abc</p>")]
    [TestCase("ab_c_ abc", "<p>ab<em>c</em> abc</p>")]
    [TestCase("abc_12_3 abc", "<p>abc_12_3 abc</p>")]
    [TestCase("ab_c a_bc", "<p>ab_c a_bc</p>")]
    [TestCase("__abc__", "<p><strong>abc</strong></p>")]
    [TestCase("__abc__ abc", "<p><strong>abc</strong> abc</p>")]
    [TestCase("__abc__\nabc", "<p><strong>abc</strong></p><p>abc</p>")]
    [TestCase("__abc__ _abc_ abc", "<p><strong>abc</strong> <em>abc</em> abc</p>")]
    [TestCase("a__b__c abc", "<p>a<strong>b</strong>c abc</p>")]
    [TestCase("ab__c__ abc", "<p>ab<strong>c</strong> abc</p>")]
    [TestCase("__a__bc abc", "<p><strong>a</strong>bc abc</p>")]
    [TestCase("abc__12__3 abc", "<p>abc__12__3 abc</p>")]
    [TestCase("ab__c a__bc", "<p>ab__c a__bc</p>")]
    [TestCase("__abc__ __a_b_c abc__", "<p><strong>abc</strong> <strong>a<em>b</em>c abc</strong></p>")]
    [TestCase("__abc_", "<p>__abc_</p>")]

    //Header
    [TestCase("# abc", "<h1>abc</h1>")]
    [TestCase("# __abc__", "<h1><strong>abc</strong></h1>")]
    [TestCase("# __abc__ abc", "<h1><strong>abc</strong> abc</h1>")]
    [TestCase(@"# __abc__
abc", "<h1><strong>abc</strong></h1><p>abc</p>")]
    [TestCase("# __abc__ _abc_ abc", "<h1><strong>abc</strong> <em>abc</em> abc</h1>")]
    [TestCase(@"# __abc_
abc", "<h1>__abc_</h1><p>abc</p>")]
    [TestCase(@"# __abc__
abc", "<h1><strong>abc</strong></h1><p>abc</p>")]
    [TestCase(@"# __abc__ _abc_
abc", "<h1><strong>abc</strong> <em>abc</em></h1><p>abc</p>")]
    [TestCase(@"# __abc__
_abc_", "<h1><strong>abc</strong></h1><p><em>abc</em></p>")]
    [TestCase(@"# __abc__
_abc_ abc", "<h1><strong>abc</strong></h1><p><em>abc</em> abc</p>")]

    // Unordered list
    [TestCase("- abc", "<ul><li>abc</li></ul>")]
    [TestCase(@"- abc
- _abc_
- __abc__", "<ul><li>abc</li><li><em>abc</em></li><li><strong>abc</strong></li></ul>")]
    [TestCase(@"# abc
- abc
- _abc_
- __abc__
abc", "<h1>abc</h1><ul><li>abc</li><li><em>abc</em></li><li><strong>abc</strong></li></ul><p>abc</p>")]
    [TestCase(@"- abc
\- abc", @"<ul><li>abc</li></ul><p>- abc</p>")]
    public void Render_ExpectedHtml_InputMd(string inputMd, string expectedHtml)
    {
        var actualHtml = mdHtml.Render(inputMd);

        actualHtml.Should().BeEquivalentTo(expectedHtml);
    }

    [Test]
    public void Render_OofNDifficulty_ShortAndLongMarkdown()
    {
        // Пояснение - мини бенчмарк, не AAA, может упасть - данные не всегда проходят хоть и входные данные детерминированны
        var shortMd = @"# Markdown
В fork-е этого репозитория создай проект __M__ark_down_ и реализуй метод __Render__ класса _Md_.
Он принимает в качестве аргумента текст в __markdown__-подобной разметке, и возвращает строку с _html-кодом_ этого текста согласно спецификации.
- Проведи начальное проектирование: зафиксируй классы и их методы в коде (а также связи между классами), но не пиши внутренности методов
- Покажи декомпозицию наставнику, получи обратную связь
- После этого приступай к реализации методов, используя TDD
- Помни, твой алгоритм должен работать быстро — линейно или почти линейно от размера входа. Не забудь написать такой тест!";
        const int longCount = 100;
        var longMd = string.Join("\n", Enumerable.Repeat(shortMd, longCount));
        var shortTimes = new List<TimeSpan>();
        var longTimes = new List<TimeSpan>();
        mdHtml = Md.Html(null);

        const int warmupCount = 100;
        for (var i = 0; i < warmupCount; i++)
        {
            _ = mdHtml.Render(shortMd);
            _ = mdHtml.Render(longMd);
        }

        const int actualCount = 1000;
        var stopwatch = new Stopwatch();
        for (var i = 0; i < actualCount; i++)
        {
            stopwatch.Restart();
            _ = mdHtml.Render(shortMd);
            stopwatch.Stop();
            var shortMdTime = stopwatch.Elapsed;
            shortTimes.Add(shortMdTime);
            stopwatch.Restart();
            _ = mdHtml.Render(longMd);
            stopwatch.Stop();
            var longMdTime = stopwatch.Elapsed;
            longTimes.Add(longMdTime);
        }

        var shortNanoseconds = shortTimes.Average(x => x.TotalNanoseconds);
        var longNanoseconds = longTimes.Average(x => x.TotalNanoseconds);

        var shortPartInLongMd = longNanoseconds / longCount;
        shortPartInLongMd.Should().BeLessOrEqualTo(shortNanoseconds);
    }
}