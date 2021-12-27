using System.Collections.Generic;
using MarkdownTask.Styles;
using MarkdownTask.Tags;

namespace MarkdownTask.Searchers
{
    public class ItalicTagSearcher : TagSearcher
    {
        private const char KeyChar = '_';
        private readonly TagStyleInfo tagStyleInfo = MdStyleKeeper.Styles[TagType.Italic];

        public ItalicTagSearcher(string mdText, List<int> escapedChars) : base(mdText, escapedChars)
        {
        }

        public override List<Tag> SearchForTags()
        {
            base.PrepareToSearch();
            var result = new List<Tag>();

            for (; CurrentPosition < MdText.Length; CurrentPosition++)
                if (MdText[CurrentPosition] == KeyChar)
                    if (GetFullPrefix(tagStyleInfo) == tagStyleInfo.TagPrefix)
                        if (IsPossibleOpenItalicTag())
                        {
                            var tag = GetTagFromCurrentPosition();
                            if (tag is not null)
                                result.Add(tag);
                        }

            return result;
        }

        private bool IsPossibleOpenItalicTag()
        {
            if (CurrentPosition + tagStyleInfo.TagPrefix.Length >= MdText.Length)
                return false;

            if (IsPreviousCharEscapeChar())
                return false;

            var nextCharIsValid = !char.IsNumber(MdText[CurrentPosition + 1])
                                  && !char.IsWhiteSpace(MdText[CurrentPosition + 1])
                                  && MdText[CurrentPosition + 1] != KeyChar;

            return CurrentPosition - 1 < 0
                ? nextCharIsValid
                : nextCharIsValid && IsPreviousCharValid();
        }

        private bool IsPreviousCharEscapeChar()
        {
            return CurrentPosition > 0 && EscapedChars.Contains(CurrentPosition - 1);
        }

        private bool IsPreviousCharValid()
        {
            var isPreviousCharKeyChar = CurrentPosition - 1 >= 0 && MdText[CurrentPosition - 1] == KeyChar;
            var isCharBeforePreviousEscaped = CurrentPosition - 2 >= 0
                                              && EscapedChars.Contains(CurrentPosition - 2);

            var isPreviousCharValid = isPreviousCharKeyChar && isCharBeforePreviousEscaped
                                      || !isPreviousCharKeyChar;

            return isPreviousCharValid;
        }

        private Tag GetTagFromCurrentPosition()
        {
            var startPos = CurrentPosition;
            var length = tagStyleInfo.TagPrefix.Length;
            CurrentPosition++;

            for (; CurrentPosition < MdText.Length; CurrentPosition++)
            {
                if (IsCharNumberOrWhitespace(MdText[CurrentPosition]))
                    return null;
                if (IsPossibleCloseTag())
                {
                    length += tagStyleInfo.TagPrefix.Length;
                    return new Tag(startPos, length, tagStyleInfo);
                }

                length++;
            }

            return null;
        }

        private bool IsPossibleCloseTag()
        {
            var isTagClosedAtEndOfText = MdText[CurrentPosition] == KeyChar
                                         && CurrentPosition + 1 >= MdText.Length;
            if (isTagClosedAtEndOfText)
                return true;

            return MdText[CurrentPosition] == KeyChar
                   && CurrentPosition + 1 < MdText.Length
                   && MdText[CurrentPosition + 1] != KeyChar
                   && MdText[CurrentPosition - 1] != KeyChar;
        }
    }
}