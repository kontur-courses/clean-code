using System.Collections.Generic;
using MarkdownTask.Styles;
using MarkdownTask.Tags;

namespace MarkdownTask.Searchers
{
    public class HeaderTagSearcher : TagSearcher
    {
        private const char KeyChar = '#';
        private readonly TagStyleInfo tagStyleInfo = MdStyleKeeper.Styles[TagType.Header];

        public HeaderTagSearcher(string mdText, List<int> escapedChars) : base(mdText, escapedChars)
        {
        }

        public override List<Tag> SearchForTags()
        {
            base.PrepareToSearch();
            var result = new List<Tag>();

            for (; CurrentPosition < MdText.Length; CurrentPosition++)
                if (MdText[CurrentPosition] == KeyChar)
                    if (GetFullPrefix(tagStyleInfo) == tagStyleInfo.TagPrefix)
                        if (IsPossibleOpenTag())
                        {
                            var tag = GetTagFromCurrentPosition();
                            if (tag is not null)
                                result.Add(tag);
                        }

            return result;
        }

        private bool IsPossibleOpenTag()
        {
            const int requiredCountOfNewLineChars = 2;

            if (CurrentPosition == 0)
                return true;

            if (EscapedChars.Contains(CurrentPosition - 1))
                return false;

            var isTagAtEndOfText = CurrentPosition + tagStyleInfo.TagPrefix.Length >= MdText.Length;
            var isAbleToLookupBeforeTag = CurrentPosition - requiredCountOfNewLineChars >= 0;

            if (isTagAtEndOfText || !isAbleToLookupBeforeTag)
                return false;

            var isDoubleNewLineBeforeTag = MdText[CurrentPosition - 1] == '\n'
                                           && MdText[CurrentPosition - 2] == '\n';

            return isDoubleNewLineBeforeTag;
        }

        private Tag GetTagFromCurrentPosition()
        {
            var startPos = CurrentPosition;
            var length = tagStyleInfo.TagPrefix.Length;
            CurrentPosition += tagStyleInfo.TagPrefix.Length;

            for (; CurrentPosition < MdText.Length; CurrentPosition++)
            {
                length++;
                if (MdText[CurrentPosition] == '\n')
                    if (CurrentPosition + 1 < MdText.Length
                        && MdText[CurrentPosition + 1] == '\n')
                    {
                        length--;
                        break;
                    }
            }

            return new Tag(startPos, length, tagStyleInfo);
        }
    }
}