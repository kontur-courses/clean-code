using System;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class Renderer_Should
    {
        [TestCase("", TestName = "When string is empty")]
        [TestCase(null, TestName = "When string is null")]
        public void Render_ThrowException(string originalText)
        {
            var act = new Action(() => Renderer.Render(originalText));
            
            act.Should().Throw<ArgumentException>();
        }
    }
}