using System.Diagnostics;
using FluentAssertions;
using Markdown;
using MarkdownTest.TestData;

namespace MarkdownTest;

public class MdTest
{
    [TestCaseSource(typeof(MdTestData), nameof(MdTestData.SpecExamples))]
    [TestCaseSource(typeof(MdTestData), nameof(MdTestData.Examples))]
    public void Test(string md, string html)
    {
        new Md()
            .Render(md)
            .Should()
            .Be(html);
    }

    [Test]
    public void LinearTimeComlexityTest()
    {
        var mdExpression =
            "#Заголовок с _курсивом_ и __жирным выделением__\n[Ссылка на сайт www.example.com](www.example.com)\n";
        var md = new Md();

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        md.Render(mdExpression);
        stopwatch.Stop();

        var previous = stopwatch.ElapsedTicks;
        long current;
        for (int i = 0; i < 13; i++)
        {
            mdExpression += mdExpression;
            stopwatch.Restart();
            md.Render(mdExpression);
            stopwatch.Stop();
            GC.Collect();
            current = stopwatch.ElapsedTicks;
            Assert.LessOrEqual(current / previous, 2);
            previous = current;
        }
    }
}