using System.Collections.Generic;
using System.Text;
using Markdown.TagEvents;

namespace Markdown
{
    public class MarkdownToHtmlTranslator
    {
        private readonly List<TagEvent> tagEvents;

        public MarkdownToHtmlTranslator(List<TagEvent> tagEvents)
        {
            this.tagEvents = tagEvents;
        }

        public string Translate()
        {
            var htmlResult = new StringBuilder();
            foreach (var tagEvent in tagEvents)
                htmlResult.Append(GetHtmlPartFrom(tagEvent));
            return htmlResult.ToString();
        }

        private string GetHtmlPartFrom(TagEvent tagEvent)
        {
            if (tagEvent.IsHashtagHeader()) return "<h1>";
            if (tagEvent.IsNewLineHeader() || tagEvent.IsRightHeader()) return "</h1>";
            if (tagEvent.IsLeftUnderliner()) return "<em>";
            if (tagEvent.IsRightUnderliner()) return "</em>";
            if (tagEvent.IsLeftDoubleUnderliner()) return "<strong>";
            if (tagEvent.IsRightDoubleUnderliner()) return "</strong>";
            return tagEvent.Content;
        }
    }
}
