using System.Diagnostics;
using FluentAssertions;
using Markdown;
using Markdown.Maker;
using MarkdownTest.TestData;

namespace MarkdownTest;

public class MdTest
{
    [TestCaseSource(typeof(MdTestData), nameof(MdTestData.SpecExamples))]
    public void Test(string md, string html)
    {
        var result = new Md(new MarkdownParser(), new HtmlMaker(), new HtmlRenderer())
            .Render(md);
        result.Should()
            .Be(html);
    }

    [Test]
    public void LinearTimeComlexityTest()
    {
        var mdExpression =
            "#Заголовок с _курсивом_ и __жирным выделением__\n";
        var md = new Md(new MarkdownParser(), new HtmlMaker(), new HtmlRenderer());
    
        var stopwatch = new Stopwatch();
        GC.Collect();
        stopwatch.Start();
        md.Render(mdExpression);
        stopwatch.Stop();
        GC.Collect();
    
        var previous = stopwatch.ElapsedTicks;
        long current;
        for (int i = 0; i < 10; i++)
        {
            mdExpression += mdExpression;
            stopwatch.Restart();
            md.Render(mdExpression);
            stopwatch.Stop();
            GC.Collect();
            current = stopwatch.ElapsedTicks;
            Assert.That(current / previous <= 2);
            previous = current;
        }
    }
}