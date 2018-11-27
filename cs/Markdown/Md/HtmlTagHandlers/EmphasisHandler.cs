using System;
using Markdown.Renderers;

namespace Markdown.Md.HtmlTagHandlers
{
    public class EmphasisHandler : TagHandler
    {
        public override string Handle(Tag tag, bool isOpeningTag)
        {
            if (tag.Type == MdSpecification.Emphasis)
            {
                return isOpeningTag ? "<ul>" : "</ul>";
            }

            if (Successor == null)
            {
                throw new InvalidOperationException(
                    "Can't transfer control to the next chain element because it was null");
            }

            return Successor.Handle(tag, isOpeningTag);
        }
    }
}