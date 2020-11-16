using System.Collections.Generic;
using Markdown.Models.Tags;

namespace Markdown.Models
{
    interface ITaggedToken
    {
        public Tag Tag { get; }
        public string Value { get; }
        public List<ITaggedToken> InnerTokens { get; }
    }
}
