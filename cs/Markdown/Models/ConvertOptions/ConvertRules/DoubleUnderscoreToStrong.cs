using Markdown.Models.Tags;
using Markdown.Models.Tags.HtmlTags;
using Markdown.Models.Tags.MdTags;

namespace Markdown.Models.ConvertOptions.ConvertRules
{
    internal class DoubleUnderscoreToStrong : IConvertRule
    {
        public Tag From { get; }
        public Tag To { get; }

        public DoubleUnderscoreToStrong()
        {
            From = new DoubleUnderscore();
            To = new Strong();
        }
    }
}
