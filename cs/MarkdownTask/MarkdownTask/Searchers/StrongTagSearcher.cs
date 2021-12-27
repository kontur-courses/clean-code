using System.Collections.Generic;
using MarkdownTask.Styles;
using MarkdownTask.Tags;

namespace MarkdownTask.Searchers
{
    public class StrongTagSearcher : TagSearcher
    {
        private const char KeyChar = '_';
        private readonly TagStyleInfo tagStyleInfo = MdStyleKeeper.Styles[TagType.Strong];

        public StrongTagSearcher(string mdText) : base(mdText)
        {
        }

        public override List<Tag> SearchForTags(List<int> escapedChars)
        {
            base.PrepareToSearch();
            var result = new List<Tag>();

            for (; CurrentPosition < MdText.Length; CurrentPosition++)

                if (MdText[CurrentPosition] == KeyChar)
                {
                    var fullPrefix = GetFullPrefix(MdText);
                    if (fullPrefix == tagStyleInfo.TagPrefix)
                        if (IsPossibleOpenTag(MdText, escapedChars))
                        {
                            var tag = GetTagFromCurrentPosition(MdText);
                            if (tag is not null)
                                result.Add(tag);
                        }
                }

            return result;
        }

        private string GetFullPrefix(string mdText)
        {
            return CurrentPosition + 1 < mdText.Length
                ? "" + mdText[CurrentPosition] + mdText[CurrentPosition + 1]
                : "" + mdText[CurrentPosition];
        }

        private bool IsPossibleOpenTag(string mdText, List<int> escapedChars)
        {
            if (CurrentPosition + tagStyleInfo.TagPrefix.Length >= mdText.Length)
                return false;

            if (CurrentPosition > 0 && escapedChars.Contains(CurrentPosition - 1))
                return false;

            var nextCharIsValid = !char.IsWhiteSpace(mdText[CurrentPosition + tagStyleInfo.TagPrefix.Length])
                                  && !tagStyleInfo.TagPrefix.Contains("" +
                                                                      mdText[
                                                                          CurrentPosition +
                                                                          tagStyleInfo.TagPrefix.Length])
                                  && !char.IsNumber(mdText[CurrentPosition + tagStyleInfo.TagPrefix.Length]);

            return nextCharIsValid;
        }

        private Tag GetTagFromCurrentPosition(string mdText)
        {
            var startPos = CurrentPosition;
            var length = tagStyleInfo.TagPrefix.Length;
            CurrentPosition += tagStyleInfo.TagPrefix.Length;

            for (; CurrentPosition < mdText.Length; CurrentPosition++)
            {
                length++;
                if (!IsTagStillAbleExist(mdText[CurrentPosition]))
                    return null;
                if (mdText[CurrentPosition] == KeyChar)
                    if (CurrentPosition + 1 < mdText.Length
                        && mdText[CurrentPosition + 1] == KeyChar)
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