using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Abstractions;
using Markdown.Extensions;
using Markdown.Parsers;
using Markdown.Primitives;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class TokenCollectionParserTests
{
    private ITokenCollectionParser parser;

    [SetUp]
    public void SetUp()
    {
        parser = new TokenCollectionParser();
    }

    [Test]
    public void Parse_ShouldThrowArgumentException_OnNull()
    {
        Action act = () => parser.Parse(null);

        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Parse_ShouldReturnEmptyTagNodes_OnEmptyTokens()
    {
        var tokens = Array.Empty<Token>();
        var expected = Array.Empty<TagNode>();

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }


    [Test]
    public void Parse_ShouldReturnTextTagNode_OnTextToken()
    {
        var tokens = new[] { Tokens.Text("A") };
        var expected = new[] { Tokens.Text("A").ToTagNode() };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldParseTextWithSeveralWords()
    {
        var tokens = new[] { Tokens.Text("Two words") };
        var expected = new[] { Tokens.Text("Two words").ToTagNode() };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldTransformEscapeToText()
    {
        var tokens = new[] { Tokens.Escape };
        var expected = new[] { Tokens.Escape.ToTextToken().ToTagNode() };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldEscapeNextSymbol()
    {
        var tokens = new[] { Tokens.Escape, Tokens.Header1, Tokens.Text("A") };
        var expected = new[] { CreateTextToken(tokens).ToTagNode() };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldEscapeCharacterNotEscapeNewLine()
    {
        var tokens = new[] { Tokens.Escape, Tokens.NewLine };
        var expected = new[] { CreateTextToken(Tokens.Escape, Tokens.NewLine).ToTagNode() };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldEscapeFormattingCharacter_WhenSingleCharacter()
    {
        var tokens = new[] { Tokens.Escape, Tokens.Italic, Tokens.Text("A"), Tokens.Italic };
        var expected = new[] { CreateTextToken(Tokens.Italic, Tokens.Text("A"), Tokens.Italic).ToTagNode() };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldEscapeFormattingCharacter_WhenDoubleCharacter()
    {
        var tokens = new[] { Tokens.Escape, Tokens.Escape, Tokens.Italic, Tokens.Text("A"), Tokens.Italic };
        var expected = new[]
        {
            Tokens.Escape.ToTextToken().ToTagNode(),
            new TagNode(Tags.Italic(Tokens.Italic.Value), Tags.Text("A").ToTagNode())
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldApplyFormat_WhenCharactersSurroundsWord(Token token)
    {
        var tokens = new[] { token, Tokens.Text("A"), token };
        var expected = new[] { new TagNode(token.ToTag(), Tags.Text("A").ToTagNode()) };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldTransformFormatSymbolToText_WhenSingleBeforeText(Token token)
    {
        var tokens = new[] { token, Tokens.Text("A") };
        var expected = new[]
        {
            CreateTextToken(token, Tokens.Text("A")).ToTagNode()
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldTransformFormatSymbolToText_WhenSingleAfterText(Token token)
    {
        var tokens = new[] { Tokens.Text("A"), token };
        var expected = new[]
        {
            CreateTextToken(Tokens.Text("A"), token).ToTagNode()
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldNotApplyFormat_WhenBeforeWhitespace(Token token)
    {
        var tokens = new[] { token, Tokens.Text(" A"), token };
        var expected = new[]
        {
            CreateTextToken(token, Tokens.Text(" A"), token).ToTagNode()
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldNotApplyUnderline_WhenClosingAfterWhitespace(Token token)
    {
        var tokens = new[] { token, Tokens.Text("A "), token };
        var expected = new[]
        {
            CreateTextToken(token, Tokens.Text("A "), token).ToTagNode()
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldApplyItalic_WhenItalicSurroundedBold()
    {
        var tokens = new[]
        {
            Tokens.Bold,
            Tokens.Text("A "),
            Tokens.Italic,
            Tokens.Text("B"),
            Tokens.Italic,
            Tokens.Text(" C"),
            Tokens.Bold
        };
        var expected = new[]
        {
            new TagNode(Tags.Bold(Tokens.Bold.Value),
                new[]
                {
                    Tokens.Text("A ").ToTagNode(),
                    new TagNode(Tags.Italic(Tokens.Italic.Value), Tokens.Text("B").ToTagNode()),
                    Tokens.Text(" C").ToTagNode()
                })
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldNotApplyBold_WhenBoldSurroundedItalic()
    {
        var tokens = new[]
        {
            Tokens.Italic,
            Tokens.Text("A "),
            Tokens.Bold,
            Tokens.Text("B"),
            Tokens.Bold,
            Tokens.Text(" C"),
            Tokens.Italic
        };
        var expected = new[] { new TagNode(Tags.Italic(Tokens.Italic.Value), Tags.Text("A __B__ C").ToTagNode()) };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldNotApplyFormat_WhenSurroundsNumber(Token token)
    {
        var tokens = new[] { token, Tokens.Text("12"), token };
        var expected = new[]
        {
            CreateTextToken(token, Tokens.Text("12"), token).ToTagNode()
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldApplyFormat_WhenSurroundsBeginningOfWord(Token token)
    {
        var tokens = new[]
        {
            token,
            Tokens.Text("A"),
            token,
            Tokens.Text("BC")
        };
        var expected = new[]
        {
            new TagNode(token.ToTag(), Tokens.Text("A").ToTagNode()),
            Tokens.Text("BC").ToTagNode()
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldApplyFormat_WhenSurroundsMiddleOfWord(Token token)
    {
        var tokens = new[]
        {
            Tokens.Text("A"),
            token,
            Tokens.Text("B"),
            token,
            Tokens.Text("C")
        };
        var expected = new[]
        {
            Tokens.Text("A").ToTagNode(),
            new TagNode(token.ToTag(), Tokens.Text("B").ToTagNode()),
            Tokens.Text("C").ToTagNode()
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldApplyFormat_WhenSurroundsEndOfWord(Token token)
    {
        var tokens = new[]
        {
            Tokens.Text("AB"),
            token,
            Tokens.Text("C"),
            token
        };
        var expected = new[]
        {
            Tokens.Text("AB").ToTagNode(),
            new TagNode(token.ToTag(), Tokens.Text("C").ToTagNode())
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldNotApplyFormat_WhenClosingUnderlineAfterWhitespace(Token token)
    {
        var tokens = new[]
        {
            token,
            Tokens.Text("AC "),
            token,
            Tokens.Text("DE"),
        };
        var expected = new[]
        {
            CreateTextToken(tokens).ToTagNode()
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldNotApplyFormat_WhenOpenItalicBetweenBold()
    {
        var tokens = new[]
        {
            Tokens.Bold,
            Tokens.Text("A "),
            Tokens.Italic,
            Tokens.Text("B C"),
            Tokens.Bold
        };
        var expected = new[] { CreateTextToken(tokens).ToTagNode() };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldNotApplyFormat_WhenOpenBoldBetweenItalic()
    {
        var tokens = new[]
        {
            Tokens.Italic,
            Tokens.Text("A "),
            Tokens.Bold,
            Tokens.Text("B C"),
            Tokens.Italic
        };
        var expected = new[] { CreateTextToken(tokens).ToTagNode() };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldNotApplyFormat_OnEmptyText(Token token)
    {
        var tokens = new[] { token, token };
        var expected = new[]
        {
            CreateTextToken(token, token).ToTagNode()
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldApplyFormatHeader_OnText()
    {
        var tokens = new[] { Tokens.Header1, Tokens.Text("A") };
        var expected = new[]
        {
            new TagNode(Tags.Header1(Tokens.Header1.Value), Tokens.Text("A").ToTagNode())
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldFormatHeader_OnFormattingText(Token token)
    {
        var tokens = new[]
        {
            Tokens.Header1,
            Tokens.Text("A "),
            token,
            Tokens.Text("B"),
            token
        };
        var expected = new[]
        {
            new TagNode(Tags.Header1(Tokens.Header1.Value), new[]
            {
                Tokens.Text("A ").ToTagNode(),
                new TagNode(token.ToTag(), Tokens.Text("B").ToTagNode())
            })
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldFlushFormatting_WhenNewLine(Token token)
    {
        var tokens = new[]
        {
            token,
            Tokens.Text("A"),
            Tokens.NewLine,
            Tokens.Text("B"),
            token
        };
        var expected = new[]
        {
            CreateTextToken(token,
                Tokens.Text("A"),
                Tokens.NewLine,
                Tokens.Text("B"),
                token
            ).ToTagNode()
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldCreateLink()
    {
        var tokens = new[]
        {
            Tokens.OpenSquareBracket,
            Tokens.Text("A"),
            Tokens.CloseSquareBracket,
            Tokens.OpenCircleBracket,
            Tokens.Text("B"),
            Tokens.CloseCircleBracket
        };
        var expected = new[]
        {
            new TagNode(Tags.Link("B"), Tags.Text("A").ToTagNode())
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldReopenLinkOnDoubleOpenSquareBracket()
    {
        var tokens = new[]
        {
            Tokens.OpenSquareBracket,
            Tokens.Text("A"),
            Tokens.OpenSquareBracket,
            Tokens.Text("A"),
            Tokens.CloseSquareBracket,
            Tokens.OpenCircleBracket,
            Tokens.Text("B"),
            Tokens.CloseCircleBracket
        };
        var expected = new[]
        {
            Tags.Text("[A").ToTagNode(),
            new TagNode(Tags.Link("B"), Tags.Text("A").ToTagNode())
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [Test]
    public void Parse_ShouldNotCreateLink_WhenHasCharactersBetweenNameAndLink()
    {
        var tokens = new[]
        {
            Tokens.OpenSquareBracket,
            Tokens.Text("A"),
            Tokens.CloseSquareBracket,
            Tokens.Text("B"),
            Tokens.OpenCircleBracket,
            Tokens.Text("C"),
            Tokens.CloseCircleBracket
        };
        var expected = new[]
        {
            CreateTextToken(tokens).ToTagNode()
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    [TestCaseSource(nameof(ItalicAndBoldTokens))]
    public void Parse_ShouldCreateLink_WhenFormattingInsideName(Token token)
    {
        var tokens = new[]
        {
            Tokens.OpenSquareBracket,
            token,
            Tokens.Text("A"),
            token,
            Tokens.CloseSquareBracket,
            Tokens.OpenCircleBracket,
            Tokens.Text("B"),
            Tokens.CloseCircleBracket
        };
        var expected = new[]
        {
            new TagNode(Tags.Link("B"),
                new TagNode(token.ToTag(),
                    Tags.Text("A").ToTagNode()))
        };

        var tagNodes = parser.Parse(tokens);

        tagNodes.Should().Equal(expected);
    }

    public static IEnumerable<Token> ItalicAndBoldTokens()
    {
        yield return Tokens.Italic;
        yield return Tokens.Bold;
    }

    private Token CreateTextToken(params Token[] tokens)
        => Tokens.Text(string.Join("", tokens.Select(x => x.Value)));
}