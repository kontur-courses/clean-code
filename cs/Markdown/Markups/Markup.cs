using System;
using System.Collections.Generic;

namespace Markdown.Markups
{
    public abstract class Markup
    {
        private readonly List<Type> nestedMarkupTypes;

        protected Markup(List<Type> markupTypes)
        {
            nestedMarkupTypes = markupTypes;
        }

        public bool Contains(Type type)
        {
            return nestedMarkupTypes.Contains(type);
        }
    }
}
