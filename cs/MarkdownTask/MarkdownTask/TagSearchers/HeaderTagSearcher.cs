using System.Collections.Generic;
using MarkdownTask.Tags;

namespace MarkdownTask.TagSearchers
{
    public class HeaderTagSearcher : ITagSearcher
    {
        private int currentPosition;
        public string TagPrefix => "# ";

        public List<Tag> SearchForTags(string mdText)
        {
            PrepareToSearch();
            mdText = mdText.Trim();
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

        private bool IsPossibleOpenTag(string mdText)
        {
            const int requiredCountOfNewLineChars = 2;

            if (currentPosition == 0)
                return true;

            var isTagAtEndOfText = currentPosition + TagPrefix.Length >= mdText.Length;
            var isAbleToLookupBeforeTag = currentPosition - requiredCountOfNewLineChars >= 0;

            if (isTagAtEndOfText || !isAbleToLookupBeforeTag)
                return false;

            var isDoubleNewLineBeforeTag = mdText[currentPosition - 1] == '\n'
                                           && mdText[currentPosition - 2] == '\n';

            return isDoubleNewLineBeforeTag;
        }

        private Tag GetTagFromCurrentPosition(string mdText)
        {
            var startPos = currentPosition;
            var length = TagPrefix.Length;
            currentPosition += TagPrefix.Length;

            for (; currentPosition < mdText.Length; currentPosition++)
            {
                length++;
                if (mdText[currentPosition] == '\n')
                    if (currentPosition + 1 < mdText.Length
                        && mdText[currentPosition + 1] == '\n')
                        return new Tag(startPos, length, TagType.Header);
            }

            return new Tag(startPos, length, TagType.Header);
        }
    }
}