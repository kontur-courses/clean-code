using System;
using System.Collections.Generic;

namespace Markdown.Markups
{
    public class DoubleUnderscore : Markup
    {
        public readonly string opening = "__";
        public readonly string closing = "__";

        public DoubleUnderscore() : base(new List<Type> { typeof(Underscore) })
        {

        }
    }
}
