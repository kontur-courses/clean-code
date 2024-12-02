using System.Collections.Generic;
using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class StrongCloseParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        if (IsClosingStrong(tokens, 0))
        {
            return null;
        }
        
        var node = MatchesFirst.MatchFirst(tokens, new StrongParser());
        if (node != null)
        {
            return node;
        }

        node = MatchesFirst.MatchFirst(tokens, new EmphasisParser());
        if (node != null)
        {
            return node;
        }

        return tokens[0].Type != TypeOfToken.EndOfParagraph ? new Node(TypeOfNode.Text, 1, Value: tokens[0].Value) : null;
    }
    
    public static bool IsClosingStrong(TokenList tokens, int i)
    {
        return tokens.PeekAtOr(i,
            new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace },
            new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace },
            new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph }
        );
    }
}