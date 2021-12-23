using System.Collections.Generic;
using MarkdownTask.Tags;

namespace MarkdownTask.TagSearchers
{
    public class ItalicTagSearcher : ITagSearcher
    {
        private int currentPosition;
        public string TagPrefix => "_";

        public List<Tag> SearchForTags(string mdText)
        {
            PrepareToSearch();
            mdText = mdText.Trim();
            var result = new List<Tag>();

            for (; currentPosition < mdText.Length; currentPosition++)
                if (IsCharItalicTag(mdText[currentPosition]))
                    if (IsPossibleOpenItalicTag(mdText))
                    {
                        var tag = GetTagFromCurrentPosition(mdText);
                        if (tag is not null)
                            result.Add(tag);
                    }

            return result;
        }

        private void PrepareToSearch()
        {
            currentPosition = 0;
        }

        private bool IsCharItalicTag(char ch)
        {
            return "" + ch == TagPrefix;
        }

        private bool IsPossibleOpenItalicTag(string mdText)
        {
            if (currentPosition + 1 >= mdText.Length)
                return false;

            var nextCharIsValid = !char.IsNumber(mdText[currentPosition + 1])
                                  && !char.IsWhiteSpace(mdText[currentPosition + 1])
                                  && !IsCharItalicTag(mdText[currentPosition + 1]);

            return currentPosition - 1 < 0
                ? nextCharIsValid
                : nextCharIsValid && !IsCharItalicTag(mdText[currentPosition - 1]);
        }

        private Tag GetTagFromCurrentPosition(string mdText)
        {
            var startPos = currentPosition;
            var length = TagPrefix.Length;
            currentPosition++;

            for (; currentPosition < mdText.Length; currentPosition++)
            {
                length++;
                if (!IsTagStillAbleExist(mdText[currentPosition]))
                    return null;
                if (IsCharItalicTag(mdText[currentPosition]))
                    return new Tag(startPos, length, TagType.Italic);
            }

            return null;
        }

        private bool IsTagStillAbleExist(char currentChar)
        {
            return !char.IsWhiteSpace(currentChar) && !char.IsNumber(currentChar);
        }
    }
}