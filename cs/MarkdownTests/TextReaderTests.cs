using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Markdown;
using Markdown.Reading;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class TextReaderTests
{
    [Test]
    public void Constructor_EmptyText_Throw()
    {
        new Action(() => { new TextReader(""); })
            .Should().Throw<ArgumentException>();
    }

    [Test]
    public void ReadNextToken_SingleCharacter_FirstTokenBeMinValue()
    {
        var reader = new TextReader("t");
        var readResult = reader.ReadNextToken();
        var currentToken = reader.Current;

        using (new AssertionScope())
        {
            readResult.Should().BeTrue();
            currentToken.Should().NotBeNull();
            currentToken!.Position.Should().Be(-1);
            currentToken.Symbol.Should().Be('\0');
        }
    }

    [Test]
    public void ReadNextToken_SingleCharacter_LastTokenBeMinValue()
    {
        var reader = new TextReader("t");
        reader.ReadNextToken();
        reader.ReadNextToken();
        var readResult = reader.ReadNextToken();
        var currentToken = reader.Current;

        using (new AssertionScope())
        {
            readResult.Should().BeTrue();
            currentToken.Should().NotBeNull();
            currentToken!.Position.Should().Be(1);
            currentToken.Symbol.Should().Be('\0');
        }
    }


    [Test]
    public void ReadNextToken_SingleCharacter_SecondTokenBeRight()
    {
        var reader = new TextReader("t");
        reader.ReadNextToken();
        var readResult = reader.ReadNextToken();
        var currentToken = reader.Current;

        using (new AssertionScope())
        {
            readResult.Should().BeTrue();
            currentToken.Should().NotBeNull();
            currentToken!.Position.Should().Be(0);
            currentToken.Symbol.Should().Be('t');
        }
    }

    [Test]
    public void ReadNextToken_ManyCharacter_AllTokensBeRight()
    {
        var reader = new TextReader("text");

        var expectedTokens = new List<Token>();
        var actualTokens = new List<Token>();

        expectedTokens.Add(new Token('\0', -1, true));
        expectedTokens.Add(new Token('t', 0));
        expectedTokens.Add(new Token('e', 1));
        expectedTokens.Add(new Token('x', 2));
        expectedTokens.Add(new Token('t', 3));
        expectedTokens.Add(new Token('\0', 4, true));

        while (reader.ReadNextToken())
        {
            actualTokens.Add(reader.Current!);
        }

        actualTokens.Should().BeEquivalentTo(expectedTokens);
    }
}