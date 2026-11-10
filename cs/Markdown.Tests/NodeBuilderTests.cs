using FluentAssertions;
using Markdown.Domain;
using static Markdown.Parsing.NodeBuilder;

namespace Markdown.Tests;

public class NodeBuilderTests
{
    [TestCaseSource(nameof(BlockCases))]
    public void Build_ReturnsExpectedBlocks(string text, Node[] expectedNodes)
        => AssertParsed(text, expectedNodes);

    [TestCaseSource(nameof(EmphasisCases))]
    public void Build_ReturnsEmphasis(string text, Node[] expectedNodes)
        => AssertParsed(text, expectedNodes);

    [TestCaseSource(nameof(StrongCases))]
    public void Build_ReturnsStrong(string text, Node[] expectedNodes)
        => AssertParsed(text, expectedNodes);

    [TestCaseSource(nameof(NestedAndMixedCases))]
    public void Build_ReturnsNestedAndMixedTags(string text, Node[] expectedNodes)
        => AssertParsed(text, expectedNodes);

    [TestCaseSource(nameof(EscapingCases))]
    public void Build_HandlesEscaping(string text, Node[] expectedNodes)
        => AssertParsed(text, expectedNodes);

    [TestCaseSource(nameof(UnorderedListCases))]
    public void Build_HandlesUnorderedListCases(string text, Node[] expectedNodes)
        => AssertParsed(text, expectedNodes);

    private static void AssertParsed(string text, Node[] expectedNodes)
    {
        Build(text)
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

        yield return new TestCaseData(
            "Text\n" +
            "Also same paragraph",
            new Node[]
            {
                new ParagraphNode
                {
                    Children = [new TextNode("Text Also same paragraph")]
                }
            }).SetName("Paragraph only, split by lines");

        yield return new TestCaseData(
            """
            # header


            Above is empty line
            """,
            new Node[]
            {
                new HeaderNode
                {
                    Children = [new TextNode("header")]
                },
                new ParagraphNode
                {
                    Children = [new TextNode("Above is empty line")]
                }
            }).SetName("Skip empty lines");
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

    private static IEnumerable<TestCaseData> UnorderedListCases()
    {
        yield return new TestCaseData(
            """
            - Item 1
            - Item 2
            - Item 3
            """,
            new Node[]
            {
                new UnorderedListNode
                {
                    Children =
                    [
                        new ListItemNode { Children = [new TextNode("Item 1")] },
                        new ListItemNode { Children = [new TextNode("Item 2")] },
                        new ListItemNode { Children = [new TextNode("Item 3")] }
                    ]
                }
            }).SetName("Unordered list with multiple items");


        yield return new TestCaseData(
            """
            - Item 1
            - Item 2
            
            - Item 21
            """,
            new Node[]
            {
                new UnorderedListNode
                {
                    Children =
                    [
                        new ListItemNode { Children = [new TextNode("Item 1")] },
                        new ListItemNode { Children = [new TextNode("Item 2")] },
                    ]
                },
                new UnorderedListNode
                {
                    Children =
                    [
                        new ListItemNode { Children = [new TextNode("Item 21")] }
                    ]
                }
            }).SetName("Multiple unordered lists");
    }
}