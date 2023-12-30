using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown;
using Markdown.Tags;
using MdTest20.TestData;

namespace MdTest20;

public class MdTests
{
    private Md sut;

    private readonly Dictionary<string?, TagType> tagDictionary = new()
    {
        { "_", TagType.Italic },
        { "__", TagType.Bold },
        { "# ", TagType.Heading },
        { "* ", TagType.Bulleted },
        { "## ", TagType.Heading },
        { "### ", TagType.Heading },
    };

    [SetUp]
    public void Setup()
    {
        sut = new Md(tagDictionary);
    }

    [TestCaseSource(typeof(MdTestData), nameof(MdTestData.ItalicTagsTestsConstructor))]
    [TestCaseSource(typeof(MdTestData), nameof(MdTestData.EscapeTagsTestsConstructor))]
    [TestCaseSource(typeof(MdTestData), nameof(MdTestData.BlobTagsTestsConstructor))]
    [TestCaseSource(typeof(MdTestData), nameof(MdTestData.PairedTagsTestsConstructor))]
    [TestCaseSource(typeof(MdTestData), nameof(MdTestData.HeaderTagsTestsConstructor))]
    [TestCaseSource(typeof(MdTestData), nameof(MdTestData.BulletedTagsTestsConstructor))]
    public void Render_HandleTextWithTags(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [Test]
    public void LinearTimeComlexityTest()
    {
        const double linearCoefficient = 2;
        var sb = new StringBuilder();
        string testText;
        var mdExpression = "# I'm __living__ life do or _die_, what can I say\n" +
                           "* I'm 23 now will I ever live to see 24\n" +
                           "* The way things is going I don't know\n" +
                           "* # Tell m_e wh_y are we so blind to see\n" +
                           "__That the ones we hurt are you and me.__";
        sb.Append(mdExpression);
        var stopwatch = new Stopwatch();
        testText = sb.ToString();
        sut.Render(testText);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        stopwatch.Start();
        sut.Render(mdExpression);
        stopwatch.Stop();
        var previous = stopwatch.ElapsedTicks;
        for (var i = 0; i < 50; i++)
        {
            sb.Append(mdExpression);
            testText = sb.ToString();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            stopwatch.Restart();
            sut.Render( testText);
            stopwatch.Stop();
            Assert.That(stopwatch.ElapsedTicks / previous, Is.LessThanOrEqualTo(linearCoefficient));
            previous = stopwatch.ElapsedTicks;
        }
    }
}