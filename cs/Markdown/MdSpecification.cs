using System;
using System.Collections.Generic;

namespace Markdown
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

        public static bool IsEscape(string str, int position)
        {
            return position - 1 >= 0 && str[position - 1] == '\\';
        }

        public static bool IsText(string str, int position, out string result)
        {
            result = string.Empty;

            for (var i = position; i < str.Length; i++)
            {
                if (Tags
                    .ContainsValue(str[i]
                        .ToString()))
                {
                    break;
                }

                result += str[i];
            }

            return true;
        }
    }
}