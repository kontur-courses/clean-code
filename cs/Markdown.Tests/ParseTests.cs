using FluentAssertions;
using Markdown.Domain;
using static Markdown.MarkdownParser;

namespace Markdown.Tests;

public class ParseTests
{
    [TestCaseSource(nameof(BlockCases))]
    public void Parse_ReturnsExpectedBlocks(string text, IReadOnlyList<Node> expectedNodes)
        => AssertParsed(text, expectedNodes);

    [TestCaseSource(nameof(EmphasisCases))]
    public void Parse_ReturnsEmphasis(string text, IReadOnlyList<Node> expectedNodes)
        => AssertParsed(text, expectedNodes);

    [TestCaseSource(nameof(StrongCases))]
    public void Parse_ReturnsStrong(string text, IReadOnlyList<Node> expectedNodes)
        => AssertParsed(text, expectedNodes);

    [TestCaseSource(nameof(NestedAndMixedCases))]
    public void Parse_ReturnsNestedAndMixedTags(string text, IReadOnlyList<Node> expectedNodes)
        => AssertParsed(text, expectedNodes);

    [TestCaseSource(nameof(EscapingCases))]
    public void Parse_HandlesEscaping(string text, IReadOnlyList<Node> expectedNodes)
        => AssertParsed(text, expectedNodes);

    private static void AssertParsed(string text, IReadOnlyList<Node> expectedNodes)
    {
        Parse(text)
            .Should()
            .BeEquivalentTo(expectedNodes, option =>
                option.PreferringRuntimeMemberTypes()
                    .WithStrictOrdering());
    }

    private static IEnumerable<TestCaseData> BlockCases()
    {
        yield return new TestCaseData(
            "# header\n" +
            "paragraph",
            new Node[]
            {
                new HeaderNode
                {
                    Children = [new TextNode("header")]
                },
                new ParagraphNode
                {
                    Children = [new TextNode("paragraph")]
                }
            }).SetName("Header followed by paragraph");

        yield return new TestCaseData(
            "# Text",
            new Node[]
            {
                new HeaderNode
                {
                    Children = [new TextNode("Text")]
                }
            }).SetName("Header only");

        yield return new TestCaseData(
            "Text",
            new Node[]
            {
                new ParagraphNode
                {
                    Children = [new TextNode("Text")]
                }
            }).SetName("Paragraph only");
    }

    private static IEnumerable<TestCaseData> EmphasisCases()
    {
        yield return new TestCaseData(
            "_Text_",
            new Node[]
            {
                new ParagraphNode
                {
                    Children =
                    [
                        new EmphasisNode
                        {
                            Children = [new TextNode("Text")]
                        }
                    ]
                }
            }).SetName("Emphasis");
    }

    private static IEnumerable<TestCaseData> StrongCases()
    {
        yield return new TestCaseData(
            "__Text__",
            new Node[]
            {
                new ParagraphNode
                {
                    Children =
                    [
                        new StrongNode
                        {
                            Children = [new TextNode("Text")]
                        }
                    ]
                }
            }).SetName("Strong");
    }

    private static IEnumerable<TestCaseData> NestedAndMixedCases()
    {
        yield return new TestCaseData(
            "___Text___",
            new Node[]
            {
                new ParagraphNode
                {
                    Children =
                    [
                        new StrongNode
                        {
                            Children =
                            [
                                new EmphasisNode
                                {
                                    Children = [new TextNode("Text")]
                                }
                            ]
                        }
                    ]
                }
            }).SetName("Strong wrapping emphasis");

        yield return new TestCaseData(
            "__Strong _Emphasis___",
            new Node[]
            {
                new ParagraphNode
                {
                    Children =
                    [
                        new StrongNode
                        {
                            Children =
                            [
                                new TextNode("Strong "),
                                new EmphasisNode
                                {
                                    Children = [new TextNode("Emphasis")]
                                }
                            ]
                        }
                    ]
                }
            }).SetName("Emphasis inside strong with text");

        yield return new TestCaseData(
            "_em_ __strong__ _em_ __strong__",
            new Node[]
            {
                new ParagraphNode
                {
                    Children =
                    [
                        new EmphasisNode
                        {
                            Children = [new TextNode("em")]
                        },
                        new TextNode(" "),
                        new StrongNode
                        {
                            Children = [new TextNode("strong")]
                        },
                        new TextNode(" "),
                        new EmphasisNode
                        {
                            Children = [new TextNode("em")]
                        },
                        new TextNode(" "),
                        new StrongNode
                        {
                            Children = [new TextNode("strong")]
                        }
                    ]
                }
            }).SetName("Alternating emphasis and strong");
    }

    private static IEnumerable<TestCaseData> EscapingCases()
    {
        yield return new TestCaseData(
            @"\_em\_ \_\_strong\_\_",
            new Node[]
            {
                new ParagraphNode
                {
                    Children = [new TextNode("_em_ __strong__")]
                }
            }).SetName("Escaped underscores are plain text");
    }
}