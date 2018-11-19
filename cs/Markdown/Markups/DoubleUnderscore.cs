using System;
using System.Collections.Generic;

namespace Markdown.Markups
{
    public class DoubleUnderscore : Markup
    {
        public DoubleUnderscore() : base("__", "__", "strong", new List<Type> { typeof(Underscore) })
        {

        }
    }
}
