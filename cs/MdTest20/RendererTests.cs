using FluentAssertions;
using Markdown;
using Markdown.Tags;
using Markdown.Tokens;
using MdTest20.TestData;

namespace MdTest20;

public class RendererTests
{
    
    
    [TestCaseSource(typeof(RendererTestData), nameof(RendererTestData.ConstructorRendererTokenList))]
    public void HandleTokensTests(List<Token> tokens, List<Token> expectedTokens)
    {
       var renderer = new HtmlRenderer();
        renderer.HandleTokens(tokens).Should().BeEquivalentTo(expectedTokens);
    }
}