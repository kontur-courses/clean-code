using System.Collections.Generic;

namespace MarkdownTask
{
    public class StrongTagSearcher : ITagSearcher
    {
        private const string TagSign = "__";
        private int currentPosition;

        public List<Tag> SearchForTags(string mdText)
        {
            var result = new List<Tag>();
            currentPosition = 0;

            while (currentPosition < mdText.Length)
            {
                if (mdText[currentPosition] == '_')
                    if (currentPosition + 1 < mdText.Length
                        && mdText[currentPosition + 1] == '_')
                    {
                        var tag = GetTagFromCurrentPosition(mdText);
                        if (tag is null)
                            continue;

                        result.Add(tag);
                    }

                currentPosition++;
            }

            return result;
        }

        private Tag GetTagFromCurrentPosition(string mdText)
        {
            var startPos = currentPosition;
            var length = 2;
            var tagOpened = true;
            currentPosition += 2;

            while (currentPosition < mdText.Length)
            {
                length++;
                if (mdText[currentPosition] == '_')
                    if (currentPosition + 1 < mdText.Length
                        && mdText[currentPosition + 1] == '_')
                    {
                        length++;
                        tagOpened = false;
                        break;
                    }

                currentPosition++;
            }

            return tagOpened ? null : new Tag(startPos, length, TagType.Strong);
        }
    }
}