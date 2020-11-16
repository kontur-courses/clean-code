using System.Collections.Generic;
using System.Linq;
using Markdown.Models.Tags;

namespace Markdown.Models
{
    internal class TaggedToken : ITaggedToken
    {
        public Tag Tag { get; }
        public string Value => string.Join("", InnerTokens.Select(token => token.Value));
        public List<ITaggedToken> InnerTokens { get; }

        public TaggedToken(string value, Tag tag)
        {
            Tag = tag;
            InnerTokens = new List<ITaggedToken>();
            if (!string.IsNullOrEmpty(value))
                InnerTokens.Add(new TextToken(value));

        }
    }
}
