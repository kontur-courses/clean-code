using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkdownConvertor.ITokens
{
    public class TagToken : IToken
    {
        private static readonly Dictionary<string, Tuple<TokenType, int>> TagRanks =
            new Dictionary<string, Tuple<TokenType, int>>
            {
                { "_", Tuple.Create(TokenType.DoubleTag, 2) },
                { "__", Tuple.Create(TokenType.DoubleTag, 1) },
                { "# ", Tuple.Create(TokenType.SingleTag, 1) },
                { "- ", Tuple.Create(TokenType.SingleTag, 1) }
            };

        public TagToken(string value, bool isOpening = false)
        {
            Value = value;
            IsOpening = isOpening;
            TokenType = TagRanks[value].Item1;
            Rank = TagRanks[value].Item2;
        }

        public string Value { get; }
        public TokenType TokenType { get; }
        public int Rank { get; }
        public bool IsOpening { get; }

        public static bool IsValueDoubleTag(string value)
        {
            return TagRanks[value].Item1 is TokenType.DoubleTag;
        }

        public static HashSet<string> GetTags()
        {
            return TagRanks.Keys.ToHashSet();
        }
    }
}