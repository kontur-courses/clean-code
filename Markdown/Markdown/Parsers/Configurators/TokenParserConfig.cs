using System.Collections.Generic;

namespace Markdown
{
    internal class TokenParserConfig
    {
        public List<string> Tokens { get; } = new();
        public List<Tag> InterruptTags { get; } = new();
        public List<Tag> NewLineTags { get; } = new();
        public OneTimeSettedValue<Tag> ShieldingSymbol { get; } = new();
        public Tag? LastAddedToken { get; set; }
        public TagRules TagRules { get; } = new();
        public Dictionary<Tag, List<Tag>> Shells { get; } = new();
    }
}