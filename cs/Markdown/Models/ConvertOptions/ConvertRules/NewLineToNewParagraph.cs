using Markdown.Models.Tags;
using Markdown.Models.Tags.HtmlTags;
using Markdown.Models.Tags.MdTags;

namespace Markdown.Models.ConvertOptions.ConvertRules
{
    class NewLineToNewParagraph : IConvertRule
    {
        public Tag From { get; }
        public Tag To { get; }

        public NewLineToNewParagraph()
        {
            From = new NewLine();
            To = new NewParagraph();
        }
    }
}
