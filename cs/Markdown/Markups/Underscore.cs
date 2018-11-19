using System;
using System.Collections.Generic;

namespace Markdown.Markups
{
    public class Underscore : Markup
    {
        public Underscore() : base("_", "_", "em", new List<Type>())
        {

        }
    }
}
