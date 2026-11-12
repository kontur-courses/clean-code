using NUnit.Framework;
using FluentAssertions;
using System.Collections;
using Markdown.Nodes;
using Markdown.Nodes.Transformers;
using Markdown.Tokens;
using Markdown.Transformers;

namespace Markdown.Tests;

[TestFixture]
public class TokenParserTests
{
    public static IEnumerable HappyPathCases
    {
        get
        {
            yield return new TestCaseData<IReadOnlyList<Token>, IReadOnlyList<Node>>(
                    [new TextToken("Hello World!")],
                    [
                        new TextBlock([
                            new Text("Hello World!")
                        ])
                    ])
                .SetName("HappyPath: plain text");
            yield return new TestCaseData<IReadOnlyList<Token>, IReadOnlyList<Node>>(
                    [new TextToken("a"), new ParagraphBreakToken(), new TextToken("b")],
                    [
                        new TextBlock([
                            new Text("a")
                        ]),
                        new TextBlock([
                            new Text("b")
                        ])
                    ])
                .SetName("HappyPath: two paragraphs");
            yield return new TestCaseData<IReadOnlyList<Token>, IReadOnlyList<Node>>(
                    [],
                    [])
                .SetName("HappyPath: no tokens");
            yield return new TestCaseData<IReadOnlyList<Token>, IReadOnlyList<Node>>(
                    [
                        new TextToken("Hello "),
                        new UnderscoreToken(CanOpen: true, CanClose: false, IsInsideWord: false),
                        new TextToken("world"),
                        new UnderscoreToken(CanOpen: false, CanClose: true, IsInsideWord: false),
                        new TextToken("!")
                    ],
                    [
                        new TextBlock([
                            new Text("Hello "),
                            new Emphasis("world"),
                            new Text("!")
                        ])
                    ])
                .SetName("Emphasis: single underscores around a word");

            yield return new TestCaseData<IReadOnlyList<Token>, IReadOnlyList<Node>>(
                    [
                        new UnderscoreToken(CanOpen: true, CanClose: false, IsInsideWord: false),
                        new TextToken("a "),
                        new DoubleUnderscoreToken(CanOpen: true, CanClose: false, IsInsideWord: false),
                        new TextToken("b"),
                        new DoubleUnderscoreToken(CanOpen: false, CanClose: true, IsInsideWord: false),
                        new TextToken(" c"),
                        new UnderscoreToken(CanOpen: false, CanClose: true, IsInsideWord: false)
                    ],
                    [
                        new TextBlock([
                            new Emphasis("a __b__ c")
                        ])
                    ])
                .SetName("Emphasis: double underscores inside are plain text");

            yield return new TestCaseData<IReadOnlyList<Token>, IReadOnlyList<Node>>(
                    [
                        new DoubleUnderscoreToken(CanOpen: true, CanClose: false, IsInsideWord: false),
                        new TextToken("bold"),
                        new DoubleUnderscoreToken(CanOpen: false, CanClose: true, IsInsideWord: false)
                    ],
                    [
                        new TextBlock([
                            new Strong([new Text("bold")])
                        ])
                    ])
                .SetName("Strong: double underscores around a word");

            yield return new TestCaseData<IReadOnlyList<Token>, IReadOnlyList<Node>>(
                    [
                        new DoubleUnderscoreToken(CanOpen: true, CanClose: false, IsInsideWord: false),
                        new TextToken("a "),
                        new UnderscoreToken(CanOpen: true, CanClose: false, IsInsideWord: false),
                        new TextToken("b"),
                        new UnderscoreToken(CanOpen: false, CanClose: true, IsInsideWord: false),
                        new TextToken(" c"),
                        new DoubleUnderscoreToken(CanOpen: false, CanClose: true, IsInsideWord: false)
                    ],
                    [
                        new TextBlock([
                            new Strong([
                                new Text("a "),
                                new Emphasis("b"),
                                new Text(" c")
                            ])
                        ])
                    ])
                .SetName("Strong: emphasis nested inside");
        }
    }


    [TestCaseSource(nameof(HappyPathCases))]
    public void StringTokenizer_ReturnsExpectedArray(IReadOnlyList<Token> tokens, IReadOnlyList<Node> expected)
    {
        var result = TokenParser.Parse(tokens);

        result.Should().BeEquivalentTo(expected, o => o
            .PreferringRuntimeMemberTypes()
            .WithStrictOrdering());
    }
}