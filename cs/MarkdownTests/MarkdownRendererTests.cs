using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using Markdown.Models;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownRendererTests
    {
        [Test]
        public void RenderMatches_SingleMatch()
        {
            var renderer = new MarkdownRenderer("_italic_");
            var matches = new List<TokenMatch> {new() {Start = 0, Length = 8, Token = MarkdownTokensFactory.Italic()}};

            var actual = renderer.RenderMatches(matches);

            actual.Should().Be("<em>italic</em>");
        }

        [Test]
        public void RenderMatches_TwoMatches()
        {
            var renderer = new MarkdownRenderer("_italic_ __bold__");
            var matches = new List<TokenMatch>
            {
                new() {Start = 0, Length = 8, Token = MarkdownTokensFactory.Italic()},
                new() {Start = 9, Length = 8, Token = MarkdownTokensFactory.Bold()}
            };

            var actual = renderer.RenderMatches(matches);

            actual.Should().Be("<em>italic</em> <strong>bold</strong>");
        }

        [Test]
        public void RenderMatches_NestedMatches()
        {
            var renderer = new MarkdownRenderer("__bo _italic_ ld__");
            var matches = new List<TokenMatch>
            {
                new() {Start = 0, Length = 18, Token = MarkdownTokensFactory.Bold()},
                new() {Start = 5, Length = 8, Token = MarkdownTokensFactory.Italic()}
            };

            var actual = renderer.RenderMatches(matches);

            actual.Should().Be("<strong>bo <em>italic</em> ld</strong>");
        }
    }
}