using System;

namespace Markdown.Tokens
{
    internal abstract class StyleToken : Token
    {
        public Type StyleType { get; private set; }
        public StyleToken(Type style) 
        {
            StyleType = style;
        }
    }
}
