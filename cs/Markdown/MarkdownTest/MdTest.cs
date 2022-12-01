﻿using FluentAssertions;
using Markdown;

namespace MarkdownTest;

[TestFixture]
public class MdTest
{
    private Md md;

    [SetUp]
    public void SetUp()
    {
        md = new Md();
    }

    [TestCase("", "", TestName = "EmptyStringTest")]
    [TestCase("a", "a", TestName = "TextWithoutTag")]
    [TestCase("_", "_", TestName = "SingleTag")]
    [TestCase("__", "__", TestName = "TwoUnderscores")]
    [TestCase("_a_", "<em>a</em>", TestName = "OneSimpleTag")]
    [TestCase("_a_ _b_", "<em>a</em> <em>b</em>", TestName = "TwoSimpleTag")]
    [TestCase("_a__b_", "<em>a</em><em>b</em>", TestName = "TwoSimpleTagWithoutSpace")]
    [TestCase("_a_ _b_ _c_", "<em>a</em> <em>b</em> <em>c</em>", TestName = "ThreeSimpleTag")]
    [TestCase("_a _bc_ _bc_ d_", "<em>a <em>bc</em> <em>bc</em> d</em>", TestName = "NestedTags")]
    public void EmTagTest(string mdstring, string result)
    {
        md.Render(mdstring)
            .Should()
            .Be(result);
        md = new Md();
        md.Render(" " + mdstring)
            .Should()
            .Be(" " + result);
        md = new Md();
        md.Render(mdstring + " ")
            .Should()
            .Be(result + " ");
        md.Render(" " + mdstring + " ")
            .Should()
            .Be(" " + result + " ");
    }
}