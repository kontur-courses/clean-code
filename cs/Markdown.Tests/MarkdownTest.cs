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
}