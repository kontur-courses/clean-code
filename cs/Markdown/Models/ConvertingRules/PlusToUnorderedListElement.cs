using Markdown.Models.Tags;
using Markdown.Models.Tags.HtmlTags;
using Markdown.Models.Tags.MdTags;

namespace Markdown.Models.ConvertingRules
{
    internal class PlusToUnorderedListElement : IConvertRule
    {
        public Tag From { get; }
        public Tag To { get; }

        public PlusToUnorderedListElement()
        {
            From = new Plus();
            To = new UnorderedListElement();
        }
    }
}
