using System.Collections.Generic;
using MarkdownTask.Styles;
using MarkdownTask.Tags;

namespace MarkdownTask.Searchers
{
    public class HeaderTagSearcher : TagSearcher
    {
        private const char KeyChar = '#';
        private readonly TagStyleInfo tagStyleInfo = MdStyleKeeper.Styles[TagType.Header];

        public HeaderTagSearcher(string mdText) : base(mdText)
        {
        }

        public override List<Tag> SearchForTags(List<int> escapedChars)
        {
            base.PrepareToSearch();
            var result = new List<Tag>();

            for (; CurrentPosition < MdText.Length; CurrentPosition++)
                if (MdText[CurrentPosition] == KeyChar)
                    if (GetFullPrefix(tagStyleInfo) == tagStyleInfo.TagPrefix)
                        if (IsPossibleOpenTag(MdText, escapedChars))
                        {
                            var tag = GetTagFromCurrentPosition(MdText);
                            if (tag is not null)
                                result.Add(tag);
                        }

            return result;
        }

        private bool IsPossibleOpenTag(string mdText, List<int> escapedChars)
        {
            const int requiredCountOfNewLineChars = 2;

            if (CurrentPosition == 0)
                return true;

            if (escapedChars.Contains(CurrentPosition - 1))
                return false;

            var isTagAtEndOfText = CurrentPosition + tagStyleInfo.TagPrefix.Length >= mdText.Length;
            var isAbleToLookupBeforeTag = CurrentPosition - requiredCountOfNewLineChars >= 0;

            if (isTagAtEndOfText || !isAbleToLookupBeforeTag)
                return false;

            var isDoubleNewLineBeforeTag = mdText[CurrentPosition - 1] == '\n'
                                           && mdText[CurrentPosition - 2] == '\n';

            return isDoubleNewLineBeforeTag;
        }

        private Tag GetTagFromCurrentPosition(string mdText)
        {
            var startPos = CurrentPosition;
            var length = tagStyleInfo.TagPrefix.Length;
            CurrentPosition += tagStyleInfo.TagPrefix.Length;

            for (; CurrentPosition < mdText.Length; CurrentPosition++)
            {
                length++;
                if (mdText[CurrentPosition] == '\n')
                    if (CurrentPosition + 1 < mdText.Length
                        && mdText[CurrentPosition + 1] == '\n')
                    {
                        length--;
                        break;
                    }
            }

            return new Tag(startPos, length, tagStyleInfo);
        }
    }
}