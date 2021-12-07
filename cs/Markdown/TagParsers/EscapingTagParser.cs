using System.Collections.Generic;
using Markdown.TagEvents;

namespace Markdown.TagParsers
{
    public class EscapingTagParser : ITagParser
    {
        private readonly List<TagEvent> tagEvents;

        public EscapingTagParser(List<TagEvent> tagEvents)
        {
            this.tagEvents = tagEvents;
        }

        public List<TagEvent> Parse()
        {
            for (var tagIndex = 1; tagIndex < tagEvents.Count; tagIndex++)
            {
                if (tagEvents[tagIndex - 1].Name == TagName.Escape)
                    ProcessTagEscaping(tagIndex);
            } 
            return tagEvents;
        }

        private void ProcessTagEscaping(int tagIndex)
        {
            var previosTag = tagEvents[tagIndex - 1];
            var currentTag = tagEvents[tagIndex];
            switch (currentTag.Name)
            {
                case TagName.Underliner:
                case TagName.DoubleUnderliner:
                case TagName.Escape:
                case TagName.Header:
                case TagName.NewLine:
                    ConvertTagToWordTag(currentTag, currentTag.Content);
                    ConvertTagToWordTag(previosTag, "");
                    break;
                case TagName.Eof:
                case TagName.Word:
                case TagName.Number:
                case TagName.Whitespace:
                    ConvertTagToWordTag(previosTag, previosTag.Content);
                    return;
            }
        }

        private static void ConvertTagToWordTag(TagEvent currentTag, string content)
        {
            currentTag.Side = TagSide.None;
            currentTag.Name = TagName.Word;
            currentTag.Content = content;
        }
    }
}