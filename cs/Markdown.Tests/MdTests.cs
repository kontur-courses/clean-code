using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using FluentAssertions;

namespace Markdown.Tests;

public class Tests
{
    public Md Md { get; set; }

    [SetUp]
    public void Setup()
    {
        Md = new Md(new HtmlConverter(), new Tokenizer());
    }

    [TestCase("_text text text_", ExpectedResult = "<em>text text text</em>", TestName = "TextInItalicTag")]
    [TestCase("__text text text__", ExpectedResult = "<strong>text text text</strong>", TestName = "TextInStrongTag")]
    [TestCase("#a\n", ExpectedResult = "<h1>a</h1>\n", TestName = "HeaderTag")]
    [TestCase("_12_3", ExpectedResult = "_12_3", TestName = "Italic_In_Numbers_Not_Tagged")]
    [TestCase("__12__3", ExpectedResult = "__12__3", TestName = "Strong_In_Numbers_Not_Tagged")]
    [TestCase("__", ExpectedResult = "__", TestName = "EmptyItalicTag")]
    [TestCase("____", ExpectedResult = "____", TestName = "EmptyStrongTag")]
    [TestCase("в _нач_але", ExpectedResult = "в <em>нач</em>але", TestName = "Italic_Tagged_Part_Word_At_Start")]
    [TestCase("сер_еди_не", ExpectedResult = "сер<em>еди</em>не", TestName = "Italic_Tagged_Part_Word_At_Middle")]
    [TestCase("кон_це_", ExpectedResult = "кон<em>це</em>", TestName = "Italic_Tagged_Part_Word_At_End")]
    [TestCase("ра_зных сл_овах", ExpectedResult = "ра_зных сл_овах", TestName = "Italic_Not_Tagged_If_Tags_InDifferentWords")]
    [TestCase("в __нач__але", ExpectedResult = "в <strong>нач</strong>але", TestName = "Strong_Tagged_Part_Word_At_Start")]
    [TestCase("сер__еди__не", ExpectedResult = "сер<strong>еди</strong>не", TestName = "Strong_Tagged_Part_Word_At_Middle")]
    [TestCase("кон__це__", ExpectedResult = "кон<strong>це</strong>", TestName = "Strong_Tagged_Part_Word_At_End")]
    [TestCase("ра__зных сл__овах", ExpectedResult = "ра__зных сл__овах", TestName = "Strong_Not_Tagged_If_Tags_InDifferentWords")]

    public string RenderTest(string line)
    {
        var result = Md.Render(line);
        return result;
    }
    [TestCase("__text _text_ text__", ExpectedResult = "<strong>text <em>text</em> text</strong>", TestName = "Italic_In_Strong_Tagged")]
    [TestCase("_text __text__ text_", ExpectedResult = "<em>text __text__ text</em>", TestName = "Strong_In_Italic_Not_Tagged")]
    [TestCase("__text _text__ text_", ExpectedResult = "__text _text__ text_", TestName = "Intersections_StrongT_And_Italic_NotTagged")]
    [TestCase("__text_", ExpectedResult = "__text_", TestName = "Unpaired_Characters_Not_Considered_Tags")]
    [TestCase("#Header __text _text_ text__", ExpectedResult = "<h1>Header <strong>text <em>text</em> text</strong></h1>", TestName = "Header_Can_Contain_Other_Markup_Elements")]
    public string Render_TagInteractionTest(string line)
    {
        var result = Md.Render(line);
        return result;
    }
    [TestCase(@"\_text text\_", ExpectedResult = "_text text_", TestName = "{m}_Escape_Tag")]
    [TestCase(@"te\xt text\ \text", ExpectedResult = @"te\xt text\ \text", TestName = "{m}_Disappear_Only_If_Shielding")]
    [TestCase(@"\\_text\\_", ExpectedResult = "<em>text</em>", TestName = "{m}_Shielding_Shielding_Chars")]
    public string Render_Shielding_Should(string line)
    {
        var result = Md.Render(line);

        return result;
    }
    [Test]
    public void Render_Should_Fail_OnNull()
    {
        Action action = () => Md.Render(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            var str = (string)TestContext.CurrentContext.Test.Arguments[0]!;
            TestContext.WriteLine("Input string: {0}", str);
            TestContext.WriteLine("Output string: {0}", Md.Render(str));
        }
    }

    [Test]
    public void PerformanceTest()
    {
        var sb = new StringBuilder();
        var line = @"#_aa_ __b__ /_a/_";
        sb.AppendLine(line);
        var stopwatch= new Stopwatch();
        stopwatch.Start();
        Md.Render(sb.ToString());
        stopwatch.Stop();

        var time = stopwatch.ElapsedTicks;

        for (int i = 0; i < 5; i++)
        {
            sb.Append(line);
            stopwatch.Start();
            Md.Render(sb.ToString());
            stopwatch.Stop();

            var current = stopwatch.ElapsedTicks;
            time.Should().BeLessThan(current);
            time = current;
        }
    }
}