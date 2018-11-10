using System.Collections.Generic;

namespace Markdown
{
    class DoubleUnderscoreElement : MarkdownElementBase
    {
        public DoubleUnderscoreElement(int start, int end, IReadOnlyList<IMarkdownElement> innerElements)
            : base(start, end, innerElements, "__", new []{ typeof(UnderscoreElement) })
        { }
    }
}
