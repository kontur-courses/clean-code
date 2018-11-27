using Markdown.Renderers;

namespace Markdown.Md.HtmlTagHandlers
{
    public class TextHandler : TagHandler
    {
        public override string Handle(Tag tag, bool isOpeningTag)
        {
            if (tag.Type == MdSpecification.Text && isOpeningTag)
            {
                return tag.Value;
            }

            return Successor == null ? string.Empty : Successor.Handle(tag, isOpeningTag);
        }
    }
}