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
                        result.Add(GetTagFromCurrentPosition(mdText));

                currentPosition++;
            }

            return result;
        }

        private Tag GetTagFromCurrentPosition(string mdText)
        {
            var startPos = currentPosition;
            var length = 2;
            currentPosition += 2;

            while (currentPosition < mdText.Length)
            {
                length++;
                if (mdText[currentPosition] == '_')
                    if (currentPosition + 1 < mdText.Length
                        && mdText[currentPosition + 1] == '_')
                    {
                        length++;
                        break;
                    }

                currentPosition++;
            }

            return new Tag(startPos, length, TagType.Strong);
        }
    }
}