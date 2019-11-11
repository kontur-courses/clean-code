using System.Collections.Generic;

namespace Markdown.MarkdownDocument.Inline
{
    public class Link : IInlineWithContent
    {
        public Link(IEnumerable<IInline> content, IEnumerable<IInline> address)
        {
            Content = content;
            Address = address;
        }

        public IEnumerable<IInline> Content { get; set; }
        
        public IEnumerable<IInline> Address { get; set; }
    }
}