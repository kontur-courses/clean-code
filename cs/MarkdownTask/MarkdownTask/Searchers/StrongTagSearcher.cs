using System.Collections.Generic;
using MarkdownTask.Styles;
using MarkdownTask.Tags;

namespace MarkdownTask.Searchers
{
    public class StrongTagSearcher : TagSearcher
    {
        private const char KeyChar = '_';
        private readonly TagStyleInfo tagStyleInfo = MdStyleKeeper.Styles[TagType.Strong];

        public StrongTagSearcher(string mdText, List<int> escapedChars) : base(mdText, escapedChars)
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
            if (CurrentPosition + tagStyleInfo.TagPrefix.Length >= MdText.Length)
                return false;

            if (CurrentPosition > 0 && EscapedChars.Contains(CurrentPosition - 1))
                return false;

            var nextCharIsValid = !char.IsWhiteSpace(MdText[CurrentPosition + tagStyleInfo.TagPrefix.Length])
                                  && !tagStyleInfo.TagPrefix.Contains("" +
                                                                      MdText[
                                                                          CurrentPosition +
                                                                          tagStyleInfo.TagPrefix.Length])
                                  && !char.IsNumber(MdText[CurrentPosition + tagStyleInfo.TagPrefix.Length]);

            return nextCharIsValid;
        }

        private Tag GetTagFromCurrentPosition()
        {
            var startPos = CurrentPosition;
            var length = tagStyleInfo.TagPrefix.Length;
            CurrentPosition += tagStyleInfo.TagPrefix.Length;

            for (; CurrentPosition < MdText.Length; CurrentPosition++)
            {
                length++;
                if (!IsTagStillAbleExist(MdText[CurrentPosition]))
                    return null;
                if (MdText[CurrentPosition] == KeyChar)
                    if (CurrentPosition + 1 < MdText.Length
                        && MdText[CurrentPosition + 1] == KeyChar)
                    {
                        length++;
                        return new Tag(startPos, length, tagStyleInfo);
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