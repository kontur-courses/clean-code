﻿using System.Collections;
using FluentAssertions;
using Markdown.Filter.MarkdownFilters;
using Markdown.Lexer;
using Markdown.Tokens;
using Markdown.Tokens.Types;
using MarkdownTests.Lexer.TestCases;

namespace MarkdownTests.Lexer;

public class MarkdownLexerTests
{
    private MarkdownLexer lexer = TestDataFactory.Lexer;
    private readonly MarkdownFilter filter = new();

    [Test]
    public void Constructor_ThrowsArgumentException_WhenFilterIsNull()
    {
        Assert.Throws<ArgumentException>(() => lexer = new MarkdownLexer(null!, new HashSet<ITokenType>()));
    }

    [Test]
    public void Constructor_CorrectlyRegistersType_OnCorrectInput()
    {
        var type = new EmphasisToken();

        var singleTypeLexer = new MarkdownLexerBuilder(filter)
            .WithTokenType(type)
            .Build();

        singleTypeLexer.RegisteredTokenTypes["_"]
            .Should()
            .Be(type);
    }
    
    [Test]
    public void Tokenize_ThrowsArgumentException_WhenInputStringIsNull()
    {
        Assert.Throws<ArgumentException>(() => lexer.Tokenize(null!));
    }

    [Test]
    public void Tokenize_ReturnsOnlyLineItself_OnZeroRegisteredTokenTypes()
    {
        var emptyLexer = new MarkdownLexer(filter, new HashSet<ITokenType>());
        var result = emptyLexer.Tokenize("line without tokens");

        EnsureExpectedTokenAt(result.Tokens, 0, "line without tokens");
        EnsureExpectedCollectionSize(result.Tokens, 1);
    }

    [Test]
    public void Tokenize_ReturnsOnlyLineItself_WhenNoTokensMatched()
    {
        var result = lexer.Tokenize("line without matching tokens");

        EnsureExpectedTokenAt(result.Tokens, 0, "line without matching tokens");
        EnsureExpectedCollectionSize(result.Tokens, 1);
    }

    [TestCaseSource(typeof(LexerTestCases), nameof(LexerTestCases.NoValidationTests))]
    public List<Token> Tokenize_ReturnsCorrectResult_WhenNoValidationRequired(string line)
    {
        return lexer.Tokenize(line).Tokens;
    }

    [TestCaseSource(typeof(LexerTestCases), nameof(LexerTestCases.EscapeSymbolsTests))]
    public List<Token> Tokenize_ReturnsCorrectResult_WithEscapeSymbols(string line)
    {
        return lexer.Tokenize(line).Tokens;
    }

    [TestCaseSource(typeof(LexerTestCases), nameof(LexerTestCases.TextTokenTests))]
    public List<Token> Tokenize_ReturnsCorrectTextTokens_WhenValidationIsRequired(string line)
    {
        return lexer.Tokenize(line).Tokens;
    }

    private static void EnsureExpectedTokenAt(IReadOnlyList<Token> tokens, int index, string value)
    {
        tokens[index].GetRepresentation()
            .Should()
            .Be(value);
    }

    private static void EnsureExpectedCollectionSize(ICollection collection, int expectedSize)
    {
        collection.Count
            .Should()
            .Be(expectedSize);
    }
}