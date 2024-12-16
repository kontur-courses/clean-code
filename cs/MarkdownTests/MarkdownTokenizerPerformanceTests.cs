using System.Diagnostics;
using Markdown.Tokenizer;
using FluentAssertions;

namespace MarkdownTests;

public class MarkdownTokenizerPerformanceTests
{
    private MarkdownTokenizer tokenizer;

    [SetUp]
    public void SetUp()
    {
        tokenizer = new MarkdownTokenizer();
    }

    [Test]
    public void Tokenize_HeaderPerformanceTest()
    {
        TokenizePerformanceTest("TestCases/MarkdownTokenizer/" + TestContext.CurrentContext.Test.MethodName + ".md", 300);
    }

    [Test]
    public void Tokenize_HeaderPerformanceTest_Large()
    {
        TokenizePerformanceTest("TestCases/MarkdownTokenizer/" + TestContext.CurrentContext.Test.MethodName + ".md", 2000);
    }

    private void TokenizePerformanceTest(string filePath, int maxMilliseconds)
    {
        var content = File.ReadAllText(filePath);
        var sw = new Stopwatch();
        sw.Start();

        var memoryBefore = GC.GetTotalMemory(true);
        var result = tokenizer.Tokenize(content);
        var memoryAfter = GC.GetTotalMemory(false);

        sw.Stop();

        Console.WriteLine($"Tokenization took {sw.ElapsedMilliseconds} ms");
        Console.WriteLine($"Token count: {result.Count}");

        var memoryUsed = memoryAfter - memoryBefore;
        Console.WriteLine($"Memory used during tokenization: {memoryUsed / 1024} KB");
        sw.ElapsedMilliseconds.Should().BeLessThan(maxMilliseconds);
    }
}