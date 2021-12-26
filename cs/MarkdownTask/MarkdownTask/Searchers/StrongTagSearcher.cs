using System.Collections.Generic;
using MarkdownTask.Styles;
using MarkdownTask.Tags;

namespace MarkdownTask.Searchers
{
    public class StrongTagSearcher : ITagSearcher
    {
        private const char KeyChar = '_';
        private readonly StyleInfo styleInfo = MdStyleKeeper.Styles[TagType.Strong];
        private int currentPosition;

        public List<Tag> SearchForTags(string mdText, List<int> escapedChars)
        {
            PrepareToSearch();
            mdText = mdText.Trim();
            var result = new List<Tag>();

            for (; currentPosition < mdText.Length; currentPosition++)

                if (mdText[currentPosition] == KeyChar)
                {
                    var fullPrefix = GetFullPrefix(mdText);
                    if (fullPrefix == styleInfo.TagPrefix)
                        if (IsPossibleOpenTag(mdText, escapedChars))
                        {
                            var tag = GetTagFromCurrentPosition(mdText);
                            if (tag is not null)
                                result.Add(tag);
                        }
                }

            return result;
        }

        private void PrepareToSearch()
        {
            currentPosition = 0;
        }

        private string GetFullPrefix(string mdText)
        {
            return currentPosition + 1 < mdText.Length
                ? "" + mdText[currentPosition] + mdText[currentPosition + 1]
                : "" + mdText[currentPosition];
        }

        private bool IsPossibleOpenTag(string mdText, List<int> escapedChars)
        {
            if (currentPosition + styleInfo.TagPrefix.Length >= mdText.Length)
                return false;

            if (currentPosition > 0 && escapedChars.Contains(currentPosition - 1))
                return false;

            var nextCharIsValid = !char.IsWhiteSpace(mdText[currentPosition + styleInfo.TagPrefix.Length])
                                  && !styleInfo.TagPrefix.Contains("" +
                                                                   mdText[currentPosition + styleInfo.TagPrefix.Length])
                                  && !char.IsNumber(mdText[currentPosition + styleInfo.TagPrefix.Length]);

            return nextCharIsValid;
        }

        private Tag GetTagFromCurrentPosition(string mdText)
        {
            var startPos = currentPosition;
            var length = styleInfo.TagPrefix.Length;
            currentPosition += styleInfo.TagPrefix.Length;

            for (; currentPosition < mdText.Length; currentPosition++)
            {
                length++;
                if (!IsTagStillAbleExist(mdText[currentPosition]))
                    return null;
                if (mdText[currentPosition] == KeyChar)
                    if (currentPosition + 1 < mdText.Length
                        && mdText[currentPosition + 1] == KeyChar)
                    {
                        length++;
                        return new Tag(startPos, length, styleInfo);
                    }
            }

            return null;
        }

        private bool IsTagStillAbleExist(char currentChar)
        {
            return !char.IsWhiteSpace(currentChar) && !char.IsNumber(currentChar);
        }
    }
}