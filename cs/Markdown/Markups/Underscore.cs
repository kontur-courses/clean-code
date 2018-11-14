using System;
using System.Collections.Generic;

namespace Markdown.Markups
{
    public class Underscore : Markup
    {
        public readonly string opening = "_";
        public readonly string closing = "_";

        public Underscore() : base(new List<Type>())
        {

        }
    }
}
