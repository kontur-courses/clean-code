using FluentAssertions;
using Markdown;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTests;

[TestFixture]
public class LinkTokenParserTest
{
    private LinkTokenParser parser;

    [SetUp]
    public void SetUp()
    {
        parser = new LinkTokenParser();
    }

    [TestCase("Ссылка", TestName = "Simple link")]
    [TestCase("Большая ссылка", TestName = "Link with several words in text")]
    [TestCase("Ссылка с _тегами_ __разными__", TestName = "Dont parse another tags in text")]
    [TestCase("ссылка", "https://href_with_tags", TestName = "Dont parse another tags in href")]
    public void Parser_ParseLinkWithArgument(string text, string href = "https://google.com")
    {
        var tokenText = $"[{text}]({href})";
        var expectedToken = new TokenWithArgument(TokenType.Link, text, href);
        
        var actualToken = parser.TryParseStringToToken(tokenText).Token;

        actualToken.Should().BeEquivalentTo(expectedToken);
    }

    [TestCase("[Ссылка(https://google.com)", TestName = "Dont parse without close text tag")]
    [TestCase("Ссылка](https://google.com)", TestName = "Dont parse without open text tag")]
    [TestCase("[Ссылка]https://google.com)", TestName = "Dont parse without open link tag")]
    [TestCase("[Ссылка](https://google.com", TestName = "Dont parse without close link tag")]
    public void Parser_DontParseUncorrectLinks(string text)
    {
        var parseResult = parser.TryParseStringToToken(text);

        parseResult.HasToken.Should().BeFalse();
    }
}
