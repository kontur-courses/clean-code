using System;

namespace Markdown.Tokens
{
    internal class StyleBeginToken : StyleToken
    {
        public StyleBeginToken(Type style) : base(style) { }
        public override string ToText() => string.Empty;
    }
}
