using System.Collections.Generic;
using Markdown.Models.Tags;

namespace Markdown.Models
{
    internal class TextToken : ITaggedToken
    {
        public Tag Tag { get; }
        public string Value { get; }
        public List<ITaggedToken> InnerTokens => new List<ITaggedToken>();

        public TextToken(string value)
        {
            Tag = new Tag();
            Value = value;
        }
    }
}
