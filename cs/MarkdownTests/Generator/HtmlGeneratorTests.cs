using FluentAssertions;
using Markdown.Domains;
using Markdown.Domains.Nodes;

// ReSharper disable UseCollectionExpression

namespace MarkdownTest.Generator;

public class HtmlGeneratorTests
{
    private static IEnumerable<TestCaseData> RootNodeTestCases()
    {
        yield return new TestCaseData(
            new RootNode(
                new List<Node>
                {
                    new ItalicNode(new List<Node> { new TextNode("Test") }),
                    new NewLineNode(),
                    new HeaderNode(2, new List<Node>
                    {
                        new TextNode("Header"),
                        new TextNode(" "),
                        new TextNode("##"),
                        new TextNode(" ")
                    }),
                    new NewLineNode(),
                    new BoldNode(new List<Node> { new TextNode("word") }),
                    new TextNode(" "),
                    new ItalicNode(new List<Node> { new TextNode("word") })
                }),
            "<em>Test</em><br/><h2>Header ## </h2><br/><strong>word</strong> <em>word</em>"
        );
    }

    [Test]
    [TestCaseSource(nameof(RootNodeTestCases))]
    public void ToHtml_ShouldParse_Correctly(RootNode node, string expectedText)
    {
        var result = node.ToHtml();

        result.Should().BeEquivalentTo(expectedText);
    }
}