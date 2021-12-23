using System.Collections.Generic;

namespace MarkdownTask
{
    public class StrongTagSearcher : ITagSearcher
    {
        private int currentPosition;

        public string TagPrefix => "__";

        public List<Tag> SearchForTags(string mdText)
        {
            PrepareToSearch();

            var result = new List<Tag>();

            for (; currentPosition < mdText.Length; currentPosition++)
                if (TagPrefix.StartsWith("" + mdText[currentPosition]))
                {
                    var fullPrefix = GetFullPrefix(mdText);
                    if (fullPrefix == TagPrefix)
                        if (IsPossibleOpenTag(mdText))
                        {
                            var tag = GetTagFromCurrentPosition(mdText);
                            if (tag is not null)
                                result.Add(tag);
                        }
                    //else
                    //{
                    //    currentPosition += 2;
                    //}
                }

            return result;
        }

        private string GetFullPrefix(string mdText)
        {
            return currentPosition + 1 < mdText.Length
                ? "" + mdText[currentPosition] + mdText[currentPosition + 1]
                : "" + mdText[currentPosition];
        }

        private void PrepareToSearch()
        {
            currentPosition = 0;
        }

        private Tag GetTagFromCurrentPosition(string mdText)
        {
            var startPos = currentPosition;
            var length = TagPrefix.Length;
            currentPosition += TagPrefix.Length;

            for (; currentPosition < mdText.Length; currentPosition++)
            {
                length++;
                if (!IsTagStillAbleExist(mdText[currentPosition]))
                    return null;
                if (TagPrefix.StartsWith("" + mdText[currentPosition]))
                    if (currentPosition + 1 < mdText.Length
                        && TagPrefix.EndsWith("" + mdText[currentPosition + 1]))
                    {
                        length++;
                        return new Tag(startPos, length, TagType.Strong);
                    }
            }

            return null;
        }

        private bool IsTagStillAbleExist(char currentChar)
        {
            return !char.IsWhiteSpace(currentChar) && !char.IsNumber(currentChar);
        }

        private bool IsPossibleOpenTag(string mdText)
        {
            if (currentPosition + TagPrefix.Length >= mdText.Length)
                return false;

            var nextCharIsValid = !char.IsWhiteSpace(mdText[currentPosition + TagPrefix.Length])
                                  && !TagPrefix.Contains("" + mdText[currentPosition + TagPrefix.Length])
                                  && !char.IsNumber(mdText[currentPosition + TagPrefix.Length]);

            return nextCharIsValid;
        }
    }
}