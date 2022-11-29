using System;
using FluentAssertions;
using Markdown.Abstractions;
using Markdown.Primitives;
using Markdown.Tests.TestCaseSources;
using NUnit.Framework;
using static Markdown.Tests.TestCaseSources.TokenizerSources;

namespace Markdown.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class TokenizerTests
{
    private ITokenizer tokenizer;

    [SetUp]
    public void SetUp()
    {
        tokenizer = new Tokenizer();
    }

    [TestCase(null, TestName = "{m}Null")]
    [TestCase("", TestName = "{m}EmptyString")]
    public void Tokenize_ShouldThrowArgumentException_On(string text)
    {
        Action act = () => tokenizer.Tokenize(text);

        act.Should().Throw<ArgumentException>();
    }

    [TestCase("T", TestName = "{m}OneCharacter")]
    [TestCase("Text", TestName = "{m}OneWord")]
    [TestCase("Some text", TestName = "{m}SeveralWords")]
    public void Tokenize_ReturnTextToken_On(string text)
    {
        var tokens = tokenizer.Tokenize(text);

        tokens.Should().ContainSingle(t => t == Tokens.Text(text));
    }

    [TestCaseSource(typeof(TokenizerSources), nameof(ItalicTestCaseData))]
    [TestCaseSource(typeof(TokenizerSources), nameof(BoldTestCaseData))]
    [TestCaseSource(typeof(TokenizerSources), nameof(EscapeTestCaseData))]
    [TestCaseSource(typeof(TokenizerSources), nameof(Header1TestCaseData))]
    public void Tokenize_ReturnCorrectTokens_On(string text, Token[] expectedTokens)
    {
        var tokens = tokenizer.Tokenize(text);

        tokens.Should().Equal(expectedTokens);
    }
}