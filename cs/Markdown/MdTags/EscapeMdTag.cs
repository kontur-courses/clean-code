using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags;

internal class EscapeMdTag : IMdTag, IHtmlConverter
{
    private const string MdTag = @"\";
    private const string EscapeCharacters = @"\#-_![<";

    public TagType TagType => TagType.Normal;

    public bool IsMdTag(string mdString, int startIndex, out int tagLength)
    {
        var escapeCharIndex = startIndex + MdTag.Length;
        tagLength = MdTag.Length + 1;
        return TagSearchHelper.IsSubstring(mdString, MdTag, startIndex)
            && mdString.Length > escapeCharIndex
            && EscapeCharacters.Contains(mdString[escapeCharIndex]);
    }

    public string GetHtmlString(string mdString, int startIndex)
    {
        var escapeCharIndex = startIndex + MdTag.Length;
        return mdString[escapeCharIndex].ToString();
    }
}
