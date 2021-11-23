using System.Collections.Generic;

namespace Markdown
{
    internal class TokenParserConfig
    {
        public List<Token> Tokens { get; } = new();
        public (Tag Symbol, bool Setted) ShieldingSymbol { get; set; }
        public Tag LastAddedToken { get; set; }
        public TagRules TagRules { get; } = new();
    }
}