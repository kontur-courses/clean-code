﻿using System.Text;

namespace Markdown.Tests;

[TestFixture]
internal class ComplexTests
{
    [Test]
    public void Render_ShouldNotRenderBoldANdCursive_WhenIntersect()
    {
        Markdown.Md.Render("Text _with __text_ and__")
            .Should().Be(@"Text _with __text_ and__");

        Markdown.Md.Render("_a __b c_ d _e f__ g_")
            .Should().Be(@"_a __b c_ d _e f__ g_");

        Markdown.Md.Render("_a b__ c_ d _e f__ g_ _abcd_ _a __b c_ d__")
            .Should().Be(@"_a b__ c_ d _e f__ g_ <em>abcd<\em> _a __b c_ d__");

        Markdown.Md.Render("_a __b c_ d _e f__ g __h i_ j__")
            .Should().Be(@"_a __b c_ d _e f__ g __h i_ j__");

        Markdown.Md.Render("_a __b__ __c d_ e f__")
            .Should().Be(@"_a <strong>b<\strong> __c d_ e f__");
    }

    [Test]
    public void Render_ShouldNotRenderBold_WhenInCursive()
    {
        Markdown.Md.Render("Text _with __text__ and_")
            .Should().Be(@"Text <em>with __text__ and<\em>");
    }

    [Test]
    public void Render_ShouldRenderCursive_WhenInBold()
    {
        Markdown.Md.Render("Text __with _text_ and__")
            .Should().Be(@"Text <strong>with <em>text<\em> and<\strong>");
    }


    [Test, MaxTime(3000)]
    public void TimeTest()
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < 250000; i++)
        {
            sb.Append("_a_ ");
        }

        string s = Markdown.Md.Render(sb.ToString());
    }
}