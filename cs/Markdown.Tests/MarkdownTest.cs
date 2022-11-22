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

    [TestCase("_Some_ word _with_ tags", "<em>Some</em> word <em>with</em> tags")]
    [TestCase("#Some# word #with# tags", "<h1>Some</h1> word <h1>with</h1> tags")]
    [TestCase("#Some# word _with_ tags", "<h1>Some</h1> word <em>with</em> tags")]
    [TestCase("#Some# _word_ _with_ #tags#", "<h1>Some</h1> <em>word</em> <em>with</em> <h1>tags</h1>")]
    public void Renderer_OneCharMarkdownTagSeveral_LineWithHtml(string mdInput, string result)
    {
        markdown!.Render(mdInput).Should().Be(result);
    }
}