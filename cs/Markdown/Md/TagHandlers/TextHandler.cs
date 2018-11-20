using System.Collections.Generic;

namespace Markdown.Md.TagHandlers
{
    public class TextHandler : TagHandler
    {
        public override TokenNode Handle(string str, int position, Stack<TokenNode> openingTokens)
        {
            if (IsText(str, position, out var result))
            {
                if (result == "")
                {
                    result += str[position];
                }

                return new TokenNode(MdSpecification.Text, result);
            }

            Successor?.Handle(str, position, openingTokens);

            return new TokenNode(MdSpecification.Text, "");
        }

        public static bool IsText(string str, int position, out string result)
        {
            result = string.Empty;

            for (var i = position; i < str.Length; i++)
            {
                if (MdSpecification.Tags
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