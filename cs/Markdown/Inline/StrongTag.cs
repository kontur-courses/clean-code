using System.Collections.Generic;

namespace Markdown.Inline
{
    public class StrongTag : IInlineTag
    {
        public List<IInlineTag> Content { get; }

        public StrongTag(List<IInlineTag> content)
        {
            Content = content;
        }
    }
}
