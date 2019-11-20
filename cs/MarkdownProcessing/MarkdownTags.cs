using System.Collections.Generic;
using MarkdownProcessing.Tokens;

namespace MarkdownProcessing
{
    public static class MarkdownTags
    {
        public static readonly Dictionary<string, TokenType> PossibleTags = new Dictionary<string, TokenType>
        {
            {"_", TokenType.Italic},
            {"__", TokenType.Bold}
        };

        public const string TagSymbols = "_";
    }
}