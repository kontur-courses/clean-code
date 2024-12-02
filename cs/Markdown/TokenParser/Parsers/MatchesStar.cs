using System.Collections.Generic;
using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class MatchesStar
{
    public static (List<Node> MatchedNodes, int Consumed) MatchStar(TokenList tokens, IParser parser)
    {
        var matchedNodes = new List<Node>();
        var consumed = 0;

        while (true)
        {
            var node = parser.TryMatch(tokens.Offset(consumed));
            if (node == null)
                break;
            
            if (node.Type is TypeOfNode.Emphasis or TypeOfNode.Strong && node.Descendants != null && node.Descendants[0].Value == " ")
            {
                matchedNodes.Add(node.Descendants[0]);
                node.Descendants.RemoveAt(0);
            }
            
            matchedNodes.Add(node);
            consumed += node.Consumed;
        }

        return (matchedNodes, consumed);
    }
}