using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace Markdown
{
    internal class TokenParserConfig
    {
        public List<Token> Tokens { get; } = new();
        public (char Symbol, bool Setted) ShieldingSymbol { get; set; }
        public Tag LastAddedToken { get; set; }
        public TagRules TagRules { get; } = new();
    }
}