using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.PerformanceTests;

[TestFixture]
public class MdPerformanceTests
{
    private IRenderer _mdRenderer;
    private PerformanceMeasurer _measurer;
    
    [SetUp]
    public void SetUp()
    {
        _mdRenderer = DefaultMdFactory.CreateMd();
        _measurer = new PerformanceMeasurer(Console.WriteLine);
    }

    [Test]
    [TestCase("Hello _world_!\n", 18000)]
    [TestCase("# _Hello_\n __world__!\n", 9000)]
    [TestCase("This __text _contains_ nested__ markdown\n", 10000)]
    [TestCase("This is _an example __of inversed__ nested_ markdown\n", 8000)]
    [TestCase("Text_12_3\n", 15000)]
    [TestCase("Text __that_12_3__ is in bold\n", 10000)]
    [TestCase("_begin_ning\n", 20000)]
    [TestCase("end_ing_\n", 20000)]
    [TestCase("mi__ddl__e\n", 15000)]
    [TestCase("This sh_ould not cha_nge\n", 10000)]
    [TestCase("This sh__o_uld_ wo__rk like this\n", 9000)]
    [TestCase("__Unpaired_ markdown\n", 20000)]
    [TestCase("Another _unpaired markdown__\n", 18000)]
    [TestCase("Intersecting _markdown __should_ work__ like this\n", 10000)]
    [TestCase("This should ____ remain the same\n", 15000)]
    [TestCase(@"This should \_not turn\_ into tags", 20000)]
    [TestCase(@"This should \remain the\ same", 20000)]
    public void PerformanceTest(string testInput, int stringRepetitions)
    {
        var str = ArrangePerformanceTest(testInput, stringRepetitions);
        Console.WriteLine($"Total length: {str.Length}");

        _measurer.MeasureAverageTime(() => _mdRenderer.Render(str), 10)
            .Should()
            .BeLessOrEqualTo(1000);
    }
    
    private string ArrangePerformanceTest(string input, int copyCount)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < copyCount; i++)
            sb.Append(input);
        return sb.ToString();
    }
}