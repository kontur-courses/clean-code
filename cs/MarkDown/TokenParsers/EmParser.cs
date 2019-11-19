using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDown.TokenParsers
{
    public class EMParser : TokenParser
    {
        public override List<TokenType> EscapingTokens => new List<TokenType> { TokenType.Strong };
        public override TokenType Type => TokenType.EM;
        public override (string from, string to) OpeningTags => ("_", @"<em>");
        public override (string from, string to) ClosingTags => ("_", @"</em>");
    }
}