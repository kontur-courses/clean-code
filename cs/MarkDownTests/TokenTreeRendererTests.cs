using FluentAssertions;
using Markdown.TokenTreeRenderer;
using Markdown.Tokens;

namespace MarkDownTests;

public class TokenTreeRendererTests
{
    [Test]
    public void TokenTreeRenderer_ShouldConvertTokenCollectionToTree()
    {
        var input = new List<Token>()
        {
            new ParagraphToken(0, 20),
            new LiteralToken(1, 5, "12345"),
            new BoldToken(6, 9),
            new ItalicsToken(7, 8),
            new LiteralToken(21, 26, "232wer")
        };

        var output = new List<Token>()
        {
            new ParagraphToken(0, 20)
            {
                Tokens = {
                    new LiteralToken(1, 5, "12345"),
                    new BoldToken(6, 9)
                    {
                        Tokens =
                        {
                            new ItalicsToken(7, 8)
                        }
                    }
                }
            },
            new LiteralToken(21, 26, "232wer")
        };
        var treeRenderer = new TokenTreeRenderer();
        var res = treeRenderer.ConvertTokensToHtml(input);
        res.Should().BeEquivalentTo(output);
    }
    
}