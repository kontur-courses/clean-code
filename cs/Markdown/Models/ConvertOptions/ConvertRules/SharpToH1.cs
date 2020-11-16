using Markdown.Models.Tags;
using Markdown.Models.Tags.HtmlTags;
using Markdown.Models.Tags.MdTags;

namespace Markdown.Models.ConvertOptions.ConvertRules
{
    internal class SharpToH1 : IConvertRule
    {
        public Tag From { get; }
        public Tag To { get; }

        public SharpToH1()
        {
            From = new Sharp();
            To = new H1();
        }
    }
}
