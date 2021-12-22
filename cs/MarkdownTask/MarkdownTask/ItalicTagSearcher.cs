using System.Collections.Generic;

namespace MarkdownTask
{
    public class ItalicTagSearcher : ITagSearcher
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
                {
                    if (IsPossibleOpenTag(mdText))
                    {
                        var tag = GetTagFromCurrentPosition(mdText);
                        if (tag is not null)
                            result.Add(tag);
                    }
                    else
                    {
                        currentPosition++;
                    }
                }

                currentPosition++;
            }

            return result;
        }

        private Tag GetTagFromCurrentPosition(string mdText)
        {
            var startPos = currentPosition;
            var length = 1;
            var tagOpened = true;
            currentPosition++;

            while (currentPosition < mdText.Length)
            {
                length++;
                if (mdText[currentPosition] == ' ')
                    break;
                if (mdText[currentPosition] == TagSign)
                {
                    tagOpened = false;
                    break;
                }

                currentPosition++;
            }

            return tagOpened ? null : new Tag(startPos, length, TagType.Italic);
        }

        private bool IsPossibleOpenTag(string mdText)
        {
            return currentPosition + 1 < mdText.Length
                   && mdText[currentPosition + 1] != ' ' && mdText[currentPosition + 1] != '_';
        }
    }
}