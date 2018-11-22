using System;
using Markdown.Renderers;

namespace Markdown.Md.HtmlTagHandlers
{
    public class EmphasisHandler : HtmlTagHandler
    {
        private bool isOpening;

        public override string Handle(Tag tag)
        {
            if (tag.Type == MdSpecification.Emphasis)
            {
                isOpening = !isOpening;

                return isOpening ? "<ul>" : "</ul>";
            }

            if (Successor == null)
            {
                throw new InvalidOperationException(
                    "Can't transfer control to the next chain element because it was null");
            }

            return Successor.Handle(tag);
        }
    }
}