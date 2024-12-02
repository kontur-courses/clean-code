using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class IntersectionParser : IParser
{
    private delegate bool TryMatchDelegate(TokenList tokens, int consumed, out int additionalConsumed);

    public Node? TryMatch(TokenList tokens)
    {
        return TryMatchPattern(tokens, TryMatchOpeningStrong, TryMatchClosingStrong, TryMatchOpeningEmphasis, TryMatchClosingEmphasis)
               ?? TryMatchPattern(tokens, TryMatchOpeningEmphasis, TryMatchClosingEmphasis, TryMatchOpeningStrong, TryMatchClosingStrong);
    }
    
    private static Node? TryMatchPattern(TokenList tokens, TryMatchDelegate tryMatchOpening, TryMatchDelegate tryMatchClosing, 
        TryMatchDelegate tryMatchNestedOpening, TryMatchDelegate tryMatchNestedClosing)
    {
        if (!tryMatchOpening(tokens, 0, out var additionalConsumed))
            return null;

        var balance = 0;
        var i = additionalConsumed;

        while (i < tokens.Count())
        {
            if (tryMatchClosing(tokens, i, out additionalConsumed))
            {
                i += additionalConsumed;
                break;
            }

            if (tryMatchNestedOpening(tokens, i, out additionalConsumed))
            {
                balance++;
                i += additionalConsumed;
                continue;
            }

            if (tryMatchNestedClosing(tokens, i, out additionalConsumed))
            {
                balance--;
                i += additionalConsumed;
                continue;
            }

            i++;
        }

        return balance > 0 ? CreateIntersectionNode(tokens, i) : null;
    }
    
    private static Node CreateIntersectionNode(TokenList tokens, int end)
    {
        var descendants = tokens
            .Take(end)
            .Where(token => token.Type is not (TypeOfToken.StartOfParagraph or TypeOfToken.EndOfParagraph))
            .Select(token => new Node(TypeOfNode.Text, 1, Value: token.Value))
            .ToList();

        return new Node(TypeOfNode.Intersection, end, descendants);
    }
    
    private static bool TryMatchOpeningStrong(TokenList tokens, int consumed, out int additionalConsumed)
    {
        additionalConsumed = 3;
        
        return tokens.PeekAtOr(consumed,
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Number },
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Word },
            new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Number },
            new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Word });
    }
    
    private static bool TryMatchOpeningEmphasis(TokenList tokens, int consumed, out int additionalConsumed)
    {
        additionalConsumed = 2;
        
        return tokens.PeekAtOr(consumed,
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Number },
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Word },
            new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Number },
            new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Word });
    }
    
    private static bool TryMatchClosingStrong(TokenList tokens, int consumed, out int additionalConsumed)
    {
        additionalConsumed = 3;
        
        return tokens.PeekAtOr(consumed,
            new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace },
            new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace });
    }
    
    private static bool TryMatchClosingEmphasis(TokenList tokens, int consumed, out int additionalConsumed)
    {
        additionalConsumed = 2;
        
        return tokens.PeekAtOr(consumed,
            new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Whitespace },
            new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Whitespace });
    }
}