﻿using FluentAssertions;
using Markdown.TokenModels;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class LinkToken_Should
    {
        [TestCase("[Text](https://url.com/)", 0, "<a href=\"https://url.com/\">Text</a>")]
        [TestCase("Author - [Text](https://url.com/)", 9, "<a href=\"https://url.com/\">Text</a>")]
        [TestCase("About - [Text](https://url.com/) - article", 8, "<a href=\"https://url.com/\">Text</a>")]
        public void Create_CorrectArguments_ReturnsCorrectLinkToken(string rawToken, int position, string expected)
        {
            var actual = LinkToken.Create(rawToken, position).ToHtmlString();
            actual.Should().Be(expected);
        }

        [TestCase("[]")]
        [TestCase("[]()")]
        [TestCase("[Text]()")]
        [TestCase("[Text]")]
        [TestCase("[Text](")]
        [TestCase("[Text(url)")]
        [TestCase("[Text]url)")]
        [TestCase("[Text](url")]
        [TestCase("[Text]x(url")]
        [TestCase("[Text]](url)")]
        public void Create_IncorrectInputs_ReturnsNullToken(string rawToken)
        {
            var actual = LinkToken.Create(rawToken, 0);
            actual.Should().BeNull();
        }

        [TestCase("[Text](url)", 0, 11)]
        [TestCase("[Text](longerUrl)", 0, 17)]
        [TestCase("[longerText](url)", 0, 17)]
        [TestCase("non started [Text](url)", 12, 11)]
        public void GetLength_SomeLinkTokens_ReturnsCorrectInt(string rawToken, int startIndex, int expected) =>
            LinkToken.Create(rawToken, startIndex).MdTokenLength.Should().Be(expected);
    }
}