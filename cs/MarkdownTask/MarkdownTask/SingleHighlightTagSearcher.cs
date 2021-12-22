using System.Collections.Generic;

namespace MarkdownTask
{
    public class SingleHighlightTagSearcher : ITagSearcher
    {
        private const char TagSign = '_';
        private int currentPosition;

        public List<Tag> SearchForTags(string mdText)
        {
            var result = new List<Tag>();
            currentPosition = 0;

            while (currentPosition < mdText.Length)
            {
                if (mdText[currentPosition] == TagSign)
                    result.Add(GetTagFromCurrentPosition(mdText));

                currentPosition++;
            }

            return result;
        }

        private Tag GetTagFromCurrentPosition(string mdText)
        {
            var startPos = currentPosition;
            var length = 1;
            currentPosition++;

            while (currentPosition < mdText.Length)
            {
                length++;
                if (mdText[currentPosition] == TagSign)
                    break;
                currentPosition++;
            }

            return new Tag(startPos, length, TagType.SingleHighlight);
        }
    }
}