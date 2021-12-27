using System.Collections.Generic;
using MarkdownTask.Styles;
using MarkdownTask.Tags;

namespace MarkdownTask.Searchers
{
    public class ItalicTagSearcher : TagSearcher
    {
        private const char KeyChar = '_';
        private readonly TagStyleInfo tagStyleInfo = MdStyleKeeper.Styles[TagType.Italic];

        public ItalicTagSearcher(string mdText) : base(mdText)
        {
        }

        public override List<Tag> SearchForTags(List<int> escapedChars)
        {
            base.PrepareToSearch();
            var result = new List<Tag>();

            for (; CurrentPosition < MdText.Length; CurrentPosition++)
                if (MdText[CurrentPosition] == KeyChar)
                    if (GetFullPrefix(tagStyleInfo) == tagStyleInfo.TagPrefix)
                        if (IsPossibleOpenItalicTag(MdText, escapedChars))
                        {
                            var tag = GetTagFromCurrentPosition(MdText);
                            if (tag is not null)
                                result.Add(tag);
                        }

            return result;
        }

        private bool IsPossibleOpenItalicTag(string mdText, List<int> escapedChars)
        {
            if (CurrentPosition + 1 >= mdText.Length)
                return false;

            if (CurrentPosition > 0 && escapedChars.Contains(CurrentPosition - 1))
                return false;

            var nextCharIsValid = !char.IsNumber(mdText[CurrentPosition + 1])
                                  && !char.IsWhiteSpace(mdText[CurrentPosition + 1])
                                  && mdText[CurrentPosition + 1] != KeyChar;

            return CurrentPosition - 1 < 0
                ? nextCharIsValid
                : nextCharIsValid && IsPreviousCharValid(mdText, escapedChars);
        }

        private bool IsPreviousCharValid(string mdText, List<int> escapedChars)
        {
            var isPreviousCharKeyChar = CurrentPosition - 1 >= 0 && mdText[CurrentPosition - 1] == KeyChar;
            var isCharBeforePreviousEscaped = CurrentPosition - 2 >= 0
                                              && escapedChars.Contains(CurrentPosition - 2);

            var isPreviousCharValid = isPreviousCharKeyChar && isCharBeforePreviousEscaped
                                      || !isPreviousCharKeyChar;

            return isPreviousCharValid;
        }

        private Tag GetTagFromCurrentPosition(string mdText)
        {
            var startPos = CurrentPosition;
            var length = tagStyleInfo.TagPrefix.Length;
            CurrentPosition++;

            for (; CurrentPosition < mdText.Length; CurrentPosition++)
            {
                length++;
                if (!IsTagStillAbleExist(mdText[CurrentPosition]))
                    return null;
                if (IsPossibleCloseTag(mdText))
                    return new Tag(startPos, length, tagStyleInfo);
            }

            return null;
        }

        private bool IsTagStillAbleExist(char currentChar)
        {
            return !char.IsWhiteSpace(currentChar) && !char.IsNumber(currentChar);
        }

        private bool IsPossibleCloseTag(string mdText)
        {
            var isTagClosedAtEndOfText = mdText[CurrentPosition] == KeyChar
                                         && CurrentPosition + 1 >= mdText.Length;
            if (isTagClosedAtEndOfText)
                return true;


            return mdText[CurrentPosition] == KeyChar
                   && mdText[CurrentPosition - 1] != KeyChar
                   && CurrentPosition + 1 < mdText.Length
                   && mdText[CurrentPosition + 1] != KeyChar;
        }
    }
}