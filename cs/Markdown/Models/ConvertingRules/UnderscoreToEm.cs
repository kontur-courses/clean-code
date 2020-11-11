using Markdown.Models.Tags;
using Markdown.Models.Tags.HtmlTags;
using Markdown.Models.Tags.MdTags;

namespace Markdown.Models.ConvertingRules
{
    internal class UnderscoreToEm : IConvertRule
    {
        public Tag From { get; }
        public Tag To { get; }

        public UnderscoreToEm()
        {
            From = new Underscore();
            To = new Em();
        }
    }
}
