using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class EmphasisCloseParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        if (IsClosingEmphasis(tokens))
        {
            return null;
        }
        
        var node = MatchesFirst.MatchFirst(tokens, new EmphasisParser());
        if (node != null)
        {
            return node;
        }

        return tokens.Any() ? new Node(TypeOfNode.Text, 1, Value: tokens[0].Value) : null;
    }

    public static bool IsClosingEmphasis(TokenList tokens)
    {
        return tokens.PeekOr(
            new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Whitespace },
            new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Whitespace },
            new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph }
        );
    }
}