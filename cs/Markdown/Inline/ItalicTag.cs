using System.Collections.Generic;

namespace Markdown.Inline

{
    public class ItalicTag : IInlineTag
    {
        public List<IInlineTag> Content { get; }

        public ItalicTag(List<IInlineTag> content)
        {
            Content = content;
        }
    }
}
