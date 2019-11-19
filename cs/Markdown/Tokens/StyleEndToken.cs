using System;

namespace Markdown.Tokens
{
    internal class StyleEndToken : StyleToken
    {
        public StyleEndToken(Type style) : base(style) { }
        public override string ToText() => string.Empty;
    }
}
