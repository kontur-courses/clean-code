using System.Collections.Generic;
using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class EmphasisParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        var node = TryMatchPartOfWordEmphasis(tokens);
        if (node != null)
        {
            return node;
        }
        
        var openingNodes = TryMatchOpeningEmphasis(tokens);
        if (openingNodes == null)
        {
            return null;
        }
        
        var newTokens = tokens.Offset(2);
        
        var (contentNodes, consumed) = MatchesStar.MatchStar(newTokens, new EmphasisCloseParser());
        openingNodes.AddRange(contentNodes);
        
        return TryMatchClosingEmphasis(newTokens, openingNodes, consumed);
    }

    private static Node? TryMatchPartOfWordEmphasis(TokenList tokens)
    {
        if (tokens.Peek(TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph))
        {
            return new Node(TypeOfNode.Emphasis, 5,new List<Node>{ new Node(TypeOfNode.Text, 1, Value: tokens[2].Value) });
        }

        if (tokens.Peek(TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore) && tokens.Count() > 4 && tokens[4].Type != TypeOfToken.Underscore)
        {
            return new Node(TypeOfNode.Emphasis, 4, new List<Node>{ new Node(TypeOfNode.Text, 1, Value: tokens[2].Value) });
        }

        if (tokens.Peek(TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph))
        {
            return new Node(TypeOfNode.Emphasis, 4,new List<Node>{ new Node(TypeOfNode.Text, 1, Value: tokens[1].Value) });
        }
        
        if (tokens.Peek(TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore) && tokens.Count() > 3 && tokens[3].Type != TypeOfToken.Underscore)
        {
            return new Node(TypeOfNode.Emphasis, 3, new List<Node>{ new Node(TypeOfNode.Text, 1, Value: tokens[1].Value) });
        }

        return null;
    }

    private static List<Node>? TryMatchOpeningEmphasis(TokenList tokens)
    {
        if (tokens.PeekOr(
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Number },
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Word }))
        {
            return new List<Node>();
        }

        if (tokens.PeekOr(
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Number },
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Word }))
        {
            return new List<Node> { new Node(TypeOfNode.Text, 1, Value: " ") };
        }

        return null;
    }

    private static Node? TryMatchClosingEmphasis(TokenList tokens, List<Node> nodes, int consumed)
    {
        var additionalConsumed = 0;

        if (tokens.PeekAtOr(consumed,
                new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph }))
        {
            additionalConsumed = 5;
        }
        else if (tokens.PeekAtOr(consumed,
                 new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Whitespace },
                 new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Whitespace }))
        {
            additionalConsumed = 4;
        }
        else
        {
            return null;
        }
        
        nodes.Add(new Node(TypeOfNode.Text, 1, Value: tokens.Offset(consumed)[0].Value));
        return new Node(TypeOfNode.Emphasis, consumed + additionalConsumed, nodes);
    }
}