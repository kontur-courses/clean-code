using System.Text;
using BenchmarkDotNet.Attributes;
using Markdown;

namespace BenchmarkMarkdown;

[MemoryDiagnoser]
public class MarkdownParserBenchmark
{
    private IParser parser;
    private string fileContent;

    [Params(1, 10, 100, 1000)] public int TextSizeMultiplier { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        parser = new MarkdownParser();

        var baseDir = AppContext.BaseDirectory;
        var filePath = Path.Combine(baseDir, "InputData");
        var baseFileContent = File.ReadAllText(filePath, Encoding.UTF8);
        fileContent = string.Concat(Enumerable.Repeat(baseFileContent, TextSizeMultiplier));
    }

    [Benchmark]
    public void Parse()
    {
        parser.Parse(fileContent);
    }
}