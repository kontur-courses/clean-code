using Markdown.Tokens;

namespace Markdown.Nodes.Transformers;

public static class TokenParser
{
    public sealed class BlockParser(IReadOnlyList<IBlockRule> rules, InlineParser inline)
    {
        public IReadOnlyList<BlockNode> Parse(IReadOnlyList<Token> tokens)
        {
            throw new NotImplementedException();
        }
    }
    
    public sealed class InlineParser(IReadOnlyList<IPairingRule> rules)
    {
        public IReadOnlyList<TextNode> Parse(IReadOnlyList<Token> line)
        {
            throw new NotImplementedException();
        }
    }
    
    
    
    
    public static IReadOnlyList<Node> Parse(IReadOnlyList<Token> tokens)
    {
        var nodes = new List<BlockNode>();

        foreach (var tokenLine in tokens.SplitByParagraphBreak())
        {
            var isHeader = tokenLine is [HeaderMarkerToken, ..];
            
            var inlineNodes = ParseTokenLine(tokenLine);
            BlockNode block = isHeader ? new HeaderBlock(inlineNodes) : new TextBlock(inlineNodes);
            nodes.Add(block);
        }
        
        return nodes;
    }

    public static IReadOnlyList<TextNode> ParseTokenLine(IReadOnlyList<Token> tokenLine)
    {
        var inlineNodes = new List<TextNode>();
        foreach (var token in tokenLine)
        {
            if (token is TextToken textToken)
            {
                inlineNodes.Add(new Text(textToken.Value));
            }
        }

        return inlineNodes;
    }
    
    
    
}