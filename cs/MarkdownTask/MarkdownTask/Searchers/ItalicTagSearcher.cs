using System.Collections.Generic;
using MarkdownTask.Styles;
using MarkdownTask.Tags;

namespace MarkdownTask.Searchers
{
    public class ItalicTagSearcher : ITagSearcher
    {
        private const char KeyChar = '_';
        private readonly StyleInfo styleInfo = MdStyleKeeper.Styles[TagType.Italic];
        private int currentPosition;

        public List<Tag> SearchForTags(string mdText)
        {
            PrepareToSearch();
            mdText = mdText.Trim();
            var result = new List<Tag>();

            for (; currentPosition < mdText.Length; currentPosition++)
                if (mdText[currentPosition] == KeyChar)
                {
                    var fullPrefix = GetFullPrefix(mdText);
                    if (fullPrefix == styleInfo.TagPrefix)
                        if (IsPossibleOpenItalicTag(mdText))
                        {
                            var tag = GetTagFromCurrentPosition(mdText);
                            if (tag is not null)
                                result.Add(tag);
                        }
                }

            return result;
        }

        private string GetFullPrefix(string mdText)
        {
            return "" + mdText[currentPosition];
        }

        private void PrepareToSearch()
        {
            currentPosition = 0;
        }

        private bool IsPossibleOpenItalicTag(string mdText)
        {
            if (currentPosition + 1 >= mdText.Length)
                return false;

            var nextCharIsValid = !char.IsNumber(mdText[currentPosition + 1])
                                  && !char.IsWhiteSpace(mdText[currentPosition + 1])
                                  && mdText[currentPosition + 1] != KeyChar;

            return currentPosition - 1 < 0
                ? nextCharIsValid
                : nextCharIsValid && mdText[currentPosition - 1] != KeyChar;
        }

        private Tag GetTagFromCurrentPosition(string mdText)
        {
            var startPos = currentPosition;
            var length = styleInfo.TagPrefix.Length;
            currentPosition++;

            for (; currentPosition < mdText.Length; currentPosition++)
            {
                length++;
                if (!IsTagStillAbleExist(mdText[currentPosition]))
                    return null;
                if (mdText[currentPosition] == KeyChar)
                    return new Tag(startPos, length, styleInfo);
            }

            return null;
        }

        private bool IsTagStillAbleExist(char currentChar)
        {
            return !char.IsWhiteSpace(currentChar) && !char.IsNumber(currentChar);
        }
    }
}