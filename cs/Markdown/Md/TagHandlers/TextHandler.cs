using System.Collections.Generic;
using System.Text;

namespace Markdown.Md.TagHandlers
{
    public class TextHandler : TagHandler
    {
        public override TokenNode Handle(string str, int position, IReadOnlyCollection<ITokenNode> openingTokenNodes)
        {
            if (IsText(str, position, out var result))
            {
                if (result == "")
                {
                    result += str[position];
                }

                return new TokenNode(MdSpecification.Text, result);
            }

            Successor?.Handle(str, position, openingTokenNodes);

            return new TokenNode(MdSpecification.Text, "");
        }

        public static bool IsText(string str, int position, out string result)
        {
            result = string.Empty;
            var sb = new StringBuilder();

            for (var i = position; i < str.Length; i++)
            {
                if (MdSpecification.Tags
                    .ContainsValue(str[i]
                        .ToString()))
                {
                    break;
                }

                sb.Append(str[i]);
            }

            result = sb.ToString();

            return true;
        }
    }
}