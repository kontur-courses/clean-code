using System.Collections.Generic;
using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class StrongParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        var node = TryMatchPartOfWordStrong(tokens);
        if (node != null)
        {
            return node;
        }
        
        var openingNodes = TryMatchOpeningStrong(tokens);
        if (openingNodes == null)
        {
            return null;
        }
        
        var newTokens = tokens.Offset(3);
        
        var (contentNodes, consumed) = MatchesStar.MatchStar(newTokens, new StrongCloseParser());
        
        openingNodes.AddRange(contentNodes);
        
        return TryMatchClosingStrong(newTokens, openingNodes, consumed);
    }
    private static Node? TryMatchPartOfWordStrong(TokenList tokens)
    {
        if (tokens.Peek(TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Underscore,TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph))
        {
            return new Node(TypeOfNode.Strong, 7, new List<Node>{ new Node(TypeOfNode.Text, 1, Value: tokens[3].Value) });
        }

        if (tokens.Peek(TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore))
        {
            return new Node(TypeOfNode.Strong, 6, new List<Node>{ new Node(TypeOfNode.Text, 1, Value: tokens[3].Value) });
        }

        if (tokens.Peek(TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph))
        {
            return new Node(TypeOfNode.Strong, 6, new List<Node>{ new Node(TypeOfNode.Text, 1, Value: tokens[2].Value) });
        }

        if (tokens.Peek(TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore))
        {
            return new Node(TypeOfNode.Strong, 5, new List<Node>{ new Node(TypeOfNode.Text, 1, Value: tokens[2].Value) });
        }

        return null;
    }
    

    private static List<Node>? TryMatchOpeningStrong(TokenList tokens)
    {
        if (tokens.PeekOr(
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Number },
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Word }))
        {
            return new List<Node>();
        }

        if (tokens.PeekOr(
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Number },
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Word }))
        {
            return new List<Node> { new Node(TypeOfNode.Text, 1, Value: " ") };
        }

        return null;
    }

    private static Node? TryMatchClosingStrong(TokenList tokens, List<Node> nodes, int consumed)
    {
        var additionalConsumed = 0;

        if (tokens.PeekAtOr(consumed,
                new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph }))
        {
            additionalConsumed = 7;
        }
        else if (tokens.PeekAtOr(consumed,
                 new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace },
                 new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace }))
        {
            additionalConsumed = 6;
        }
        else
        {
            return null;
        }
        
        nodes.Add(new Node(TypeOfNode.Text,1, Value: tokens.Offset(consumed)[0].Value));
        return new Node(TypeOfNode.Strong, consumed + additionalConsumed, nodes);
    }
}