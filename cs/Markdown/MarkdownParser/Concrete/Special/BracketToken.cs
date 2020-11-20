using System;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Special
{
    public class SpecialToken : Token
    {
        public SpecialTokenType Type { get; }

        public SpecialToken(int startPosition, SpecialTokenType type) : base(startPosition, GetValue(type))
        {
            Type = type;
        }

        public static string GetValue(SpecialTokenType tokenType) => tokenType switch
        {
            SpecialTokenType.OpeningSquareBracket => "[",
            SpecialTokenType.ClosingSquareBracket => "]",
            SpecialTokenType.OpeningRoundBracket => "(",
            SpecialTokenType.ClosingRoundBracket => ")",
            _ => throw new ArgumentOutOfRangeException(nameof(tokenType),
                $"{nameof(tokenType)} has unexpected value {tokenType}")
        };
    }
}