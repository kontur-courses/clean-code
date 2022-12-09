using Markdown.Enums;
using System;

namespace Markdown.TokenNamespace
{
    public interface IToken : ICloneable
    {
        public TokenType Type { get; }
        public TagState TagState { get; }
        public string Content { get; }
    }
}
