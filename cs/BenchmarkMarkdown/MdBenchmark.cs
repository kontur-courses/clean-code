using System.Text;
using BenchmarkDotNet.Attributes;
using Markdown;

namespace BenchmarkMarkdown;

[MemoryDiagnoser]
public class MdBenchmark
{
    private Md md;
    private string fileContent;

    [Params(1, 10, 100, 1000)] public int TextSizeMultiplier { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        md = new Md();

        var baseDir = AppContext.BaseDirectory;
        var filePath = Path.Combine(baseDir, "InputData");
        var baseFileContent = File.ReadAllText(filePath, Encoding.UTF8);
        fileContent = string.Concat(Enumerable.Repeat(baseFileContent, TextSizeMultiplier));
    }

    [Benchmark]
    public string Render()
    {
        return md.Render(fileContent);
    }
}