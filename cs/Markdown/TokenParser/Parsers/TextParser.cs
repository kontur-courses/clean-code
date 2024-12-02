using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class TextParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        if (tokens.MatchesTextPatternWithBoundary())
        {
            return CreateTextNode(tokens, 1, 3);
        }

        if (tokens.MatchesTextPatternWithoutEndBoundary())
        {
            return CreateTextNode(tokens, 1, 2);
        }

        if (tokens.MatchesTextPatternStartingWithBoundary())
        {
            return CreateTextNode(tokens, 0, 2);
        }

        if (tokens.MatchesStandaloneTextPattern())
        {
            return CreateTextNode(tokens, 0, 1);
        }

        return null;
    }

    private static Node CreateTextNode(TokenList tokens, int valueIndex, int consumedTokens)
    {
        return new Node(TypeOfNode.Text, consumedTokens, Value: tokens[valueIndex].Value);
    }
}

public static class TokenListExtensions
{
    public static bool MatchesTextPatternWithBoundary(this TokenList tokens)
    {
        return tokens.PeekOr(
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Number, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Word, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Hash, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Bullet, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Whitespace, TypeOfToken.EndOfParagraph }
        );
    }

    public static bool MatchesTextPatternWithoutEndBoundary(this TokenList tokens)
    {
        return tokens.PeekOr(
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Number },
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Word },
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Hash },
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Bullet },
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore },
            new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Whitespace }
        );
    }

    public static bool MatchesTextPatternStartingWithBoundary(this TokenList tokens)
    {
        return tokens.PeekOr(
            new[] { TypeOfToken.Number, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.Word, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.Hash, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.Bullet, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
            new[] { TypeOfToken.Whitespace, TypeOfToken.EndOfParagraph }
        );
    }

    public static bool MatchesStandaloneTextPattern(this TokenList tokens)
    {
        return tokens.PeekOr(
            new[] { TypeOfToken.Number },
            new[] { TypeOfToken.Word },
            new[] { TypeOfToken.Hash },
            new[] { TypeOfToken.Bullet },
            new[] { TypeOfToken.Underscore },
            new[] { TypeOfToken.Whitespace }
        );
    }
}