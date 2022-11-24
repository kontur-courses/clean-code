using System;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

public class Tests
{
    private Markdown? markdown;
    
    [SetUp]
    public void Setup()
    {
        markdown = new Markdown();
    }

    [TestCase("")]
    [TestCase(null)]
    public void Renderer_NullOrEmpty_ArgumentNullException(string mdInput)
    {
        var rendererCheck = (Action) (() => markdown!.Render(mdInput));
        rendererCheck.Should().Throw<ArgumentException>();
    }

    [TestCase("_Something_", "<em>Something</em>")]
    [TestCase("#Something#", "<h1>Something</h1>")]
    public void Renderer_OneCharMarkdownTag_LineWithHtml(string mdInput, string result)
    {
        markdown!.Render(mdInput).Should().Be(result);
    }
    
    [Test]
    public void Renderer_TwoCharsMarkdownTag_LineWithHtml()
    {
        markdown!.Render("__Something__").Should().Be("<strong>Something</strong>");
    }

    [TestCase("_Some_ word _with_ tags", "<em>Some</em> word <em>with</em> tags")]
    [TestCase("#Some# word #with# tags", "<h1>Some</h1> word <h1>with</h1> tags")]
    [TestCase("#Some# word _with_ tags", "<h1>Some</h1> word <em>with</em> tags")]
    [TestCase("#Some# __word__ _with_ tags", "<h1>Some</h1> <strong>word</strong> <em>with</em> tags")]
    [TestCase("#Some# _word_ _with_ #tags#", "<h1>Some</h1> <em>word</em> <em>with</em> <h1>tags</h1>")]
    public void Renderer_MarkdownTagSeveral_LineWithHtml(string mdInput, string result)
    {
        markdown!.Render(mdInput).Should().Be(result);
    }
    
    [TestCase("_Some word #with# tags_", "<em>Some word <h1>with</h1> tags</em>")]
    [TestCase("#Some _word with_ tags#", "<h1>Some <em>word with</em> tags</h1>")]
    [TestCase("__Some _word with_ tags__", "<strong>Some <em>word with</em> tags</strong>")]
    [TestCase("_Some __word with__ tags_", "<em>Some __word with__ tags</em>")]
    [TestCase("#Some _word __with a lot__ of_ tags#", 
        "<h1>Some <em>word __with a lot__ of</em> tags</h1>")]
    public void Renderer_MarkdownTagInside_LineWithHtml(string mdInput, string result)
    {
        markdown!.Render(mdInput).Should().Be(result);
    }

    [TestCase("__Some words _with tags__", "<strong>Some words _with tags</strong>")]
    [TestCase("#Some __words _with tags#", "<h1>Some __words _with tags</h1>")]
    public void Renderer_MarkdownTagWithOneCharTagInside_LineWithHtmlAndOneTagInside(string mdInput, string result)
    {
        markdown!.Render(mdInput).Should().Be(result);
    }

    [TestCase("__Some _word #hi")]
    [TestCase("__Some _word #")]
    [TestCase("__Some word")]
    [TestCase("Some word_")]
    [TestCase("Some _ word")]
    [TestCase("Some word _")]
    [TestCase("Some _word _")]
    public void Renderer_MarkdownTagsWithoutClose_InputLine(string mdInput)
    {
        markdown!.Render(mdInput).Should().Be(mdInput);
    }

    [TestCase("Some ____word #hi")]
    [TestCase("Some word ____ #hi")]
    [TestCase("Some wo____rd #hi")]
    public void Renderer_ClearLineInTags_InputLine(string mdInput)
    {
        markdown!.Render(mdInput).Should().Be(mdInput);
    }

    [TestCase("Some __word _hi__ something_ text")]
    public void Renderer_IntersectionTags_InputLine(string mdInput)
    {
        markdown!.Render(mdInput).Should().Be(mdInput);
    }

    [TestCase("Some 1_3 words", "Some 1_3 words")]
    [TestCase("Some _13 words", "Some _13 words")]
    [TestCase("S_ome _13 wor_ds", "S_ome _13 wor_ds")]
    [TestCase("_Some _13 words_", "<em>Some _13 words</em>")]
    public void Renderer_TagsBetweenNumbers_TagsNotChangedBetweenNumbers(string mdInput, string result)
    {
        markdown!.Render(mdInput).Should().Be(result);
    }

    [TestCase("Some _wd_f", "Some <em>wd</em>f")]
    [TestCase("So_me wd_f", "So_me wd_f")]
    [TestCase("Lo_fd_s", "Lo<em>fd</em>s")]
    public void Renderer_TagsInWord_TagsChanged(string mdInput, string result)
    {
        markdown!.Render(mdInput).Should().Be(result);
    }
}