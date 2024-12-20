using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags;

internal class UnderscoresBetweenNumbersIgnoreTag : IMdTag
{
    public TagType TagType => TagType.Ignore;

    public bool IsMdTag(string mdString, int startIndex, out int tagLength)
    {
        if (char.IsDigit(mdString[startIndex]))
        {
            var i = startIndex + 1;

            for (; i < mdString.Length && mdString[i] == '_'; i++) ;

            if (i < mdString.Length && i > startIndex + 1 && char.IsDigit(mdString[startIndex]))
            {
                tagLength = i - startIndex + 1;
                return true;
            }
        }

        tagLength = default;
        return false;
    }
}
