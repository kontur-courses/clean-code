using System;

namespace Markdown
{
    public interface IToken : ICloneable
    {
        public TokenType Type { get; }
        public TagState TagState { get; }
        public string Content { get; }
    }
}
