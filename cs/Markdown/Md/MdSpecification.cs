using System.Collections.Generic;

namespace Markdown.Md
{
    public static class MdSpecification
    {
        public static Dictionary<MdType, string> Tags = new Dictionary<MdType, string>
        {
            {MdType.OpenEmphasis, "_"},
            {MdType.CloseEmphasis, "_"},
            {MdType.OpenStrongEmphasis, "__"},
            {MdType.CloseStrongEmphasis, "__"},
        };

        public static readonly Dictionary<MdType, string> HtmlRules = new Dictionary<MdType, string>
        {
            {MdType.Text, ""},
            {MdType.OpenEmphasis, "<ul>"},
            {MdType.CloseEmphasis, "</ul>"},
            {MdType.OpenStrongEmphasis, "<strong>"},
            {MdType.CloseStrongEmphasis, "</strong>"},
        };

        public static bool IsEscape(string str, int position)
        {
            return position - 1 >= 0 && str[position - 1] == '\\';
        }
    }
}