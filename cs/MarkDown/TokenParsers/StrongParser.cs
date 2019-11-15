using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDown.TokenParsers
{
   public class StrongParser : TokenParser
    {
        public override List<TokenType> ShieldingTokens => new List<TokenType>();
        public override TokenType Type => TokenType.Strong;
        public override (string from, string to) OpeningTags => ("__", @"<strong>");
        public override (string from, string to) ClosingTags => ("__", @"</strong>");
    }
}