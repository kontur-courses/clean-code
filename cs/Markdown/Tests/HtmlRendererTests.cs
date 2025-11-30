using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class HtmlRendererTests
{
    private IRenderer renderer;
    
    [SetUp]
    public void SetUp()
    {
        renderer = new HtmlRenderer();
    }
    
    [TestCaseSource(nameof(CasesWhenListWithOneToken))]
    [Description("Checks each tag")]
    public void Parse_ReturnsString_WhenListWithOneToken(IEnumerable<Token> inputTokens, string inputText, string expectedResult)
    {
        var result = renderer.Render(inputTokens, inputText);
        
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [TestCaseSource(nameof(CasesWhenListWithMultipleNestedTokens))]
    public void Parse_ReturnsString_WhenListWithMultipleNestedTokens(IEnumerable<Token> inputTokens, string inputText, string expectedResult)
    {
        var result = renderer.Render(inputTokens, inputText);
        
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    public static IEnumerable<TestCaseData> CasesWhenListWithOneToken()
    {
        yield return new TestCaseData(new List<Token> { new (TagType.None, "Human") }, "Human", "Human");
        yield return new TestCaseData(new List<Token> { new (TagType.Italic, "Human") }, "Human", "<em>Human</em>");
        yield return new TestCaseData(new List<Token> { new (TagType.Bold, "Human") }, "Human", "<strong>Human</strong>");
        yield return new TestCaseData(new List<Token> { new (TagType.Escaping, "\\") }, "Human", @"\\");
        yield return new TestCaseData(new List<Token> { new (TagType.Header, "Human") }, "Human", "<h1>Human</h1>");
    }
    
    public static IEnumerable<TestCaseData> CasesWhenListWithMultipleNestedTokens()
    {
        yield return new TestCaseData(new List<Token> { new (TagType.Header, "Human1 Human2", [
            new Token(TagType.Bold, "Human1"),
            new Token(TagType.None, " "),
            new Token(TagType.Italic, "Human2")
        ]) }, "Human1 Human2", "<h1><strong>Human1</strong> <em>Human2</em></h1>");
        
        yield return new TestCaseData(new List<Token> { new (TagType.Header, "Human1 Human2", new List<Token>()
        {
            new (TagType.Bold, "Human1"),
            new (TagType.None, " "),
            new (TagType.Italic, "Human2"),
        }) }, "Human1 Human2", "<h1><strong>Human1</strong> <em>Human2</em></h1>");
    }
}