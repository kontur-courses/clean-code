using System;
using System.Collections.Generic;

namespace Markdown
{
    class UnderscoreElement : MarkdownElementBase
    {
        public UnderscoreElement(int start, int end, IReadOnlyList<IMarkdownElement> innerElements)
            : base(start, end, innerElements, "_", new Type[] { })
        { }
    }
}
